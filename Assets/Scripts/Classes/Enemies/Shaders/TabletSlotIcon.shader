Shader "Custom/WaveGradientMovingTex_Builtin"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _Speed ("Main Texture Scroll Speed", Vector) = (0.1, 0.1, 0.0, 0.0)
        _GradientSpeed ("Gradient Scroll Speed", Vector) = (0.2, 0.0, 0.0, 0.0) // ✅ new
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveFrequency ("Wave Frequency", Float) = 4.0
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _Albedo ("Albedo Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _GradientTex;
            float4 _GradientTex_ST;

            float4 _Speed;
            float4 _GradientSpeed; // ✅ new

            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;
            float4 _Albedo;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvGradient : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float time = _Time.y;

                // Scroll UVs
                float2 mainUV = v.uv + _Speed.xy * time;
                float wave = sin(v.uv.x * _WaveFrequency + time * _WaveSpeed) * _WaveAmplitude;
                float2 gradientUV = float2(v.uv.x, wave * 0.5 + 0.5) + _GradientSpeed.xy * time;

                o.uvMain = TRANSFORM_TEX(mainUV, _MainTex);
                o.uvGradient = TRANSFORM_TEX(gradientUV, _GradientTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 grad = tex2D(_GradientTex, i.uvGradient);
                fixed4 tex = tex2D(_MainTex, i.uvMain);
                return grad * tex * _Albedo;
            }
            ENDCG
        }
    }
}
