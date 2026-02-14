Shader "Custom/GlowingOutline_WithTracerBands"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0, 0, 1)
        _OutlineColor ("Outline Color", Color) = (0.2, 0.6, 1, 1)
        _AlphaTex ("Alpha Mask (RGBA)", 2D) = "white" {}
        _PulseSpeed ("Pulse Speed", Float) = 2
        _ScrollSpeed1 ("Band Rotation Speed", Float) = 1
        _HoverTrigger ("Hover Trigger", Range(0, 1)) = 0
        _OutlineStrength ("Outline Strength", Range(0, 5)) = 1.5
        _BandEdgeSharpness ("Band Edge Sharpness", Range(1, 10)) = 4

        // Tracer bands
        _TracerAlpha ("Tracer Alpha", Range(0, 1)) = 0.2
        _TracerOffset ("Tracer Offset Angle", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Glowing Outline with Tracer Bands"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD1;
                float3 localPos : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            float4 _BaseColor;
            float4 _OutlineColor;
            float _Alpha;
            float _PulseSpeed;
            float _ScrollSpeed1;
            float _HoverTrigger;
            float _OutlineStrength;
            float _BandEdgeSharpness;
            float _TracerAlpha;
            float _TracerOffset;

            TEXTURE2D(_AlphaTex);
            SAMPLER(sampler_AlphaTex);

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldViewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.localPos = v.vertex.xyz;
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 norm = normalize(i.worldNormal);
                float3 viewDir = normalize(i.worldViewDir);

                // Fresnel mask for rim
                float fresnel = pow(1.0 - saturate(dot(norm, viewDir)), _OutlineStrength);
                float bandEdgeMask = pow(1.0 - saturate(dot(norm, viewDir)), _BandEdgeSharpness);

                // Breathing pulse effect
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float fresnelGlow = fresnel * lerp(pulse, 1.0, _HoverTrigger);

                // Band rotation angle
                float angle = atan2(i.localPos.z, i.localPos.x);
                angle = angle / (2.0 * 3.1415926); // Normalize to [-0.5, 0.5]
                float scrollTime = _Time.y * _ScrollSpeed1;

                // --- Main glow bands (two opposite positions)
                float rot1 = frac(angle - scrollTime);
                float band1 = smoothstep(0.0, 0.05, rot1) * (1.0 - smoothstep(0.08, 0.13, rot1));

                float rot2 = frac(angle + 0.5 - scrollTime);
                float band2 = smoothstep(0.0, 0.05, rot2) * (1.0 - smoothstep(0.08, 0.13, rot2));

                float bandGlow = (band1 + band2) * bandEdgeMask * _HoverTrigger;

                // --- Tracer bands (offset, thinner, transparent)
                float rotTracer1 = frac(angle - scrollTime + _TracerOffset);
                float tracer1 = smoothstep(0.0, 0.015, rotTracer1) * (1.0 - smoothstep(0.02, 0.03, rotTracer1));

                float rotTracer2 = frac(angle + 0.5 - scrollTime + _TracerOffset);
                float tracer2 = smoothstep(0.0, 0.015, rotTracer2) * (1.0 - smoothstep(0.02, 0.03, rotTracer2));

                float tracerGlow = (tracer1 + tracer2) * bandEdgeMask * _HoverTrigger * _TracerAlpha;

                // Final glow output
                float alphaMask = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, i.uv).r;
                
                float4 glow = _OutlineColor * (fresnelGlow + bandGlow * 1.5 + tracerGlow);
                glow.a *= alphaMask;

                float4 baseColor = _BaseColor;
                baseColor.a *= alphaMask * 0.5;

                return baseColor + glow;
            }
            ENDHLSL
        }
    }
}
