Shader "Lpk/LightModel/OverlayRotatingBase"
{
    Properties
    {
        _BaseMap            ("Overlay Texture (Rotating, Transparent)", 2D) = "white" {}
        _TopMap             ("Background Texture (Static)", 2D)            = "white" {}
        _TopMap_ST          ("TopMap Tiling/Offset", Vector)              = (1,1,0,0)

        _BaseColor          ("Color Tint", Color)                          = (1,1,1,1)

        _AlphaMask          ("Alpha Mask (Static)", 2D)                    = "white" {}
        _AlphaMask_ST       ("Alpha Mask ST", Vector)                      = (1,1,0,0)

        _AlphaMask2         ("Alpha Mask 2 (Rotating)", 2D)                = "white" {}
        _AlphaMask2_ST      ("Alpha Mask2 ST", Vector)                     = (1,1,0,0)
        _AlphaMask2Speed    ("Alpha Mask2 Rotation Speed", Float)          = 30
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha // <- Standard alpha blending

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);       SAMPLER(sampler_BaseMap);
            TEXTURE2D(_TopMap);        SAMPLER(sampler_TopMap);
            TEXTURE2D(_AlphaMask);     SAMPLER(sampler_AlphaMask);
            TEXTURE2D(_AlphaMask2);    SAMPLER(sampler_AlphaMask2);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _TopMap_ST;

                float4 _AlphaMask_ST;
                float4 _AlphaMask2_ST;
                float  _AlphaMask2Speed;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv         : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            float2 RotateUV(float2 uv, float angleDeg, float2 center)
            {
                float rad = radians(angleDeg);
                float c = cos(rad);
                float s = sin(rad);
                uv -= center;
                float2 rotated = float2(uv.x * c - uv.y * s, uv.x * s + uv.y * c);
                return rotated + center;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float2 uv = input.uv;

                // === Compute rotating angle for base + mask2 ===
                float angle = _AlphaMask2Speed * _Time.y;

                // === Rotated UV for base map ===
                float2 baseUV = RotateUV(uv, angle, float2(0.5, 0.5));
 float outline = 0.0;
float2 offsets[4] = {
    float2(1.0, 0.0), float2(-1.0, 0.0),
    float2(0.0, 1.0), float2(0.0, -1.0)
};
float2 texelSize = 1.0 / float2(_ScreenParams.x, _ScreenParams.y);

for (int i = 0; i < 4; ++i)
{
    float2 sampleUV = baseUV + offsets[i] * texelSize * 1.5; // 1.5 is thickness factor
    float alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, sampleUV).a;
    outline = max(outline, alpha);
}

// Use this expanded outline alpha
float4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, baseUV);
baseCol.a = max(baseCol.a, outline);

                // === Top texture (static background) ===
                float2 topUV = uv * _TopMap_ST.xy + _TopMap_ST.zw;
                float4 topCol = SAMPLE_TEXTURE2D(_TopMap, sampler_TopMap, topUV);

                // === Alpha Mask 1 (static, Y clamped) ===
                float2 maskUV = uv * _AlphaMask_ST.xy + _AlphaMask_ST.zw;
                maskUV.y = clamp(maskUV.y, 0.0, 1.0);
                float mask1 = SAMPLE_TEXTURE2D(_AlphaMask, sampler_AlphaMask, maskUV).r;

                // === Alpha Mask 2 (rotating) ===
                float2 mask2UV = RotateUV(uv, angle, float2(0.5, 0.5)) * _AlphaMask2_ST.xy + _AlphaMask2_ST.zw;
                float mask2 = SAMPLE_TEXTURE2D(_AlphaMask2, sampler_AlphaMask2, mask2UV).r;

                // === Combine masks
                float combinedMask = mask1 * mask2;

                // Apply alpha masking (discard pixel if too transparent)
                clip(combinedMask - 0.5);



                
                // === Overlay baseCol (transparent) over topCol (background)
                float baseAlpha = saturate(baseCol.a * combinedMask * 1);
                float3 finalRGB = lerp(topCol.rgb, baseCol.rgb, baseAlpha);

                float finalAlpha = lerp(topCol.a, 1, baseAlpha); // you can adjust this logic if needed

                float4 finalColor = float4(finalRGB, finalAlpha);
                finalColor *= _BaseColor;

                return finalColor;
            }

            ENDHLSL
        }
    }
}
