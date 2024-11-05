Shader "Custom/GrassBlowing"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.8, 0.2, 1) // Light green color
        _WaveColor ("Wave Color", Color) = (0.0, 0.5, 0.0, 1)  // Darker green wave color
        _WaveSpeed ("Wave Speed", Float) = 1.0                 // Speed of wave
        _WaveFrequency ("Wave Frequency", Float) = 5.0         // Frequency of wave
        _WaveAmplitude ("Wave Amplitude", Float) = 0.5         // Amplitude of wave
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        // User-defined properties
        float4 _BaseColor;
        float4 _WaveColor;
        float _WaveSpeed;
        float _WaveFrequency;
        float _WaveAmplitude;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate time-based wave pattern
            float wave = sin((IN.worldPos.x + _Time.y * _WaveSpeed) * _WaveFrequency) * _WaveAmplitude;

            // Interpolate between base color and wave color based on the wave pattern
            float4 color = lerp(_BaseColor, _WaveColor, wave);

            // Set the albedo (diffuse color) of the shader output
            o.Albedo = color.rgb;

            // Optional: Set metallic and smoothness to zero for a more matte look
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
