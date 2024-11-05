Shader "Custom/WaterWaves"
{
     Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.5, 0.7, 1) // Blue-green water color
        _WaveStrength ("Wave Strength", Float) = 0.1          // Controls wave height
        _WaveFrequency ("Wave Frequency", Float) = 2.0        // Controls wave density
        _WaveSpeed ("Wave Speed", Float) = 1.0                // Controls speed of wave movement
        _DarkenFactor ("Darken Factor", Float) = 0.5          // Controls how much darker the low areas are
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float waveHeight;  // To pass the displaced height to the surf function
        };

        // User-defined properties
        float4 _BaseColor;
        float _WaveStrength;
        float _WaveFrequency;
        float _WaveSpeed;
        float _DarkenFactor;

        // Vertex function to displace vertices
        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // Apply sine and cosine waves to the x and y axes for horizontal displacement
            float waveX = sin(v.vertex.x * _WaveFrequency + _Time.y * _WaveSpeed);
            float waveY = cos(v.vertex.y * _WaveFrequency + _Time.y * _WaveSpeed);

            // Calculate total wave displacement
            float waveDisplacement = (waveX + waveY) * _WaveStrength;

            // Displace the vertex along the z-axis (upwards) to create wave height
            v.vertex.z += waveDisplacement;

            // Pass the displaced height to the surf function
            o.waveHeight = v.vertex.z;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate the darkness factor based on the displaced z position
            // Assuming the wave height ranges from -_WaveStrength to +_WaveStrength
            float minHeight = -_WaveStrength;
            float maxHeight = _WaveStrength;

            // Map the z position to a 0-1 range (0 = lowest point, 1 = highest point)
            float heightFactor = saturate((IN.waveHeight - minHeight) / (maxHeight - minHeight));

            // Use the heightFactor to blend between the base color and a darkened color
            float3 darkenedColor = _BaseColor.rgb * _DarkenFactor;  // Darkened base color
            o.Albedo = lerp(darkenedColor, _BaseColor.rgb, heightFactor);

            // Optional: Adjust metallic and smoothness for a shiny, reflective water surface
            o.Metallic = 2;
            o.Smoothness = 2;
        }
        ENDCG
    }
    FallBack "Diffuse"
}