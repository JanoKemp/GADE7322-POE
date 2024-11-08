Shader "Custom/GrassBlowing"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.8, 0.2, 1)  // Light green color
        _WaveColor ("Wave Color", Color) = (0.0, 0.5, 0.0, 1)  // Darker green wave color
        _WaveSpeed ("Wave Speed", Float) = 1.0                 // Speed of wave
        _WaveFrequency ("Wave Frequency", Float) = 5.0         // Frequency of wave
        _WaveAmplitude ("Wave Amplitude", Float) = 0.5         // Amplitude of wave
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Name "GrassBlowingPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float waveFactor : TEXCOORD1; // Pass wave factor to fragment shader
            };

            float4 _BaseColor;
            float4 _WaveColor;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Calculate wave pattern based on world position and time
                float wave = sin((input.worldPos.x + _Time.y * _WaveSpeed) * _WaveFrequency) * _WaveAmplitude;

                // Pass the wave value for blending in the fragment shader
                output.waveFactor = wave;

                // Transform the vertex position to clip space
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Interpolate between base color and wave color based on wave factor
                float3 blendedColor = lerp(_BaseColor.rgb, _WaveColor.rgb, input.waveFactor);

                return half4(blendedColor, _BaseColor.a);
            }

            ENDHLSL
        }
    }

    FallBack "Diffuse"
}
