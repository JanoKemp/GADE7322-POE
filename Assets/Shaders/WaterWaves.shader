Shader "Custom/WaterWaves"
{
     Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.5, 0.7, 1) // Blue-green water color
        _WaveStrength ("Wave Strength", Float) = 0.1          // Controls wave height
        _WaveFrequency ("Wave Frequency", Float) = 2.0        // Controls wave density
        _WaveSpeed ("Wave Speed", Float) = 1.0                // Controls speed of wave movement
        _DarkenFactor ("Darken Factor", Float) = 0.5          // Controls darkness in low areas
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Name "WaterWavesPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float waveHeight : TEXCOORD1; // Pass wave height to fragment shader
            };

            float4 _BaseColor;
            float _WaveStrength;
            float _WaveFrequency;
            float _WaveSpeed;
            float _DarkenFactor;

            Varyings vert(Attributes input)
            {
                Varyings output;
                float time = _Time.y * _WaveSpeed;

                // Apply sine and cosine waves for wave displacement
                float waveX = sin(input.positionOS.x * _WaveFrequency + time);
                float waveY = cos(input.positionOS.y * _WaveFrequency + time);

                float waveDisplacement = (waveX + waveY) * _WaveStrength;

                // Offset vertex position
                float3 displacedPosition = input.positionOS.xyz;
                displacedPosition.z += waveDisplacement;

                // Transform position to clip space
                output.positionHCS = TransformObjectToHClip(displacedPosition);
                output.uv = input.uv;
                output.waveHeight = waveDisplacement;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Map wave height (-_WaveStrength to +_WaveStrength) to [0, 1]
                float minHeight = -_WaveStrength;
                float maxHeight = _WaveStrength;
                float heightFactor = saturate((input.waveHeight - minHeight) / (maxHeight - minHeight));

                // Darken low points
                float3 darkenedColor = _BaseColor.rgb * _DarkenFactor;
                float3 finalColor = lerp(darkenedColor, _BaseColor.rgb, heightFactor);

                return half4(finalColor, _BaseColor.a);
            }

            ENDHLSL
        }
    }
}