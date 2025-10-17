Shader "Lpk/LightModel/ToonLightBase_Opaque"
{
    Properties
    {
        _BaseMap            ("Texture", 2D) = "white" {}
        _BaseColor          ("Color", Color) = (0.5,0.5,0.5,1)
        _NormalMap          ("Normal Map", 2D) = "bump" {}
        _Roughness          ("Roughness", Range(0,1)) = 0.5
        _RoughnessMap       ("Roughness Map", 2D) = "white" {}

        [Space]
        _ShadowStep         ("Shadow Step", Range(0,1)) = 0.5
        _ShadowStepSmooth   ("Shadow Step Smooth", Range(0,1)) = 0.04

        [Space]
        _SpecularStep       ("Specular Step", Range(0,1)) = 0.6
        _SpecularStepSmooth ("Specular Step Smooth", Range(0,1)) = 0.05
        [HDR]_SpecularColor ("Specular Color", Color) = (1,1,1,1)

        [Space]
        _RimStep            ("Rim Step", Range(0,1)) = 0.65
        _RimStepSmooth      ("Rim Step Smooth", Range(0,1)) = 0.4
        _RimColor           ("Rim Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
        ZWrite On
        Blend Off
        Cull Back

        // ===== Main Toon Pass =====
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            TEXTURE2D(_RoughnessMap); SAMPLER(sampler_RoughnessMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _ShadowStep;
                float _ShadowStepSmooth;
                float _SpecularStep;
                float _SpecularStepSmooth;
                float4 _SpecularColor;
                float _RimStepSmooth;
                float _RimStep;
                float4 _RimColor;
                float4 _BaseMap_ST;
                float4 _NormalMap_ST;
                float4 _RoughnessMap_ST;
                float _Roughness;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv            : TEXCOORD0;
                float3 normalWS      : TEXCOORD1;
                float3 tangentWS     : TEXCOORD2;
                float3 bitangentWS   : TEXCOORD3;
                float3 viewDirWS     : TEXCOORD4;
                float4 shadowCoord   : TEXCOORD5;
                float4 fogCoord      : TEXCOORD6;
                float3 positionWS    : TEXCOORD7;
                float4 positionCS    : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float3 posWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentOS.w;
                float3 viewDirWS = GetCameraPositionWS() - posWS;

                output.positionCS = TransformWorldToHClip(posWS);
                output.positionWS = posWS;
                output.uv = input.uv;
                output.normalWS = normalWS;
                output.tangentWS = tangentWS;
                output.bitangentWS = bitangentWS;
                output.viewDirWS = viewDirWS;
                output.shadowCoord = TransformWorldToShadowCoord(posWS);
                output.fogCoord = ComputeFogFactor(output.positionCS.z);

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float2 uv = input.uv;

                // Sample textures
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv * _BaseMap_ST.xy + _BaseMap_ST.zw);
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uv * _NormalMap_ST.xy + _NormalMap_ST.zw));
                float roughnessMap = SAMPLE_TEXTURE2D(_RoughnessMap, sampler_RoughnessMap, uv * _RoughnessMap_ST.xy + _RoughnessMap_ST.zw).r;
                float roughness = saturate((roughnessMap + baseMap.a + _Roughness) / 3.0);

                float3 N = normalize(TransformTangentToWorld(normalTS, float3x3(
                    normalize(input.tangentWS),
                    normalize(input.bitangentWS),
                    normalize(input.normalWS)
                )));
                float3 V = normalize(input.viewDirWS);
                float3 L = normalize(_MainLightPosition.xyz);
                float3 H = normalize(V + L);

                float NV = dot(N, V);
                float NH = dot(N, H);
                float NL = dot(N, L);
                NL = saturate(NL * 0.5 + 0.5);

                float specularPower = 1.0 - roughness;
                float specularNH = smoothstep(
                    (1 - _SpecularStep * specularPower) - _SpecularStepSmooth * specularPower,
                    (1 - _SpecularStep * specularPower) + _SpecularStepSmooth * specularPower,
                    NH);

                float shadowNL = smoothstep(_ShadowStep - _ShadowStepSmooth, _ShadowStep + _ShadowStepSmooth, NL);
                float shadow = MainLightRealtimeShadow(input.shadowCoord);

                float rim = smoothstep((1 - _RimStep) - _RimStepSmooth * 0.5, (1 - _RimStep) + _RimStepSmooth * 0.5, 0.5 - NV);

                float3 diffuse = _MainLightColor.rgb * baseMap.rgb * _BaseColor.rgb * shadowNL * shadow;
                float3 specular = _SpecularColor.rgb * shadow * shadowNL * specularNH;
                float3 ambient = rim * _RimColor.rgb + SampleSH(N) * _BaseColor.rgb * baseMap.rgb;

                float3 finalColor = diffuse + ambient + specular;
                finalColor = MixFog(finalColor, input.fogCoord);

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }

        // ===== ShadowCaster Pass =====
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            ZWrite On
            Cull Back
            Blend Off

            HLSLPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings ShadowVert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 ShadowFrag(Varyings input) : SV_Target
            {
                return half4(1,1,1,1);
            }
            ENDHLSL
        }
    }
}
