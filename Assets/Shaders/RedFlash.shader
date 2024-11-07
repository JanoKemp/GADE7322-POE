Shader "Custom/RedFlash"
{
    Properties
    {
        _FlashColor("Flash Color", Color) = (1, 0, 0, 1) // Red color by default
        _FlashStrength("Flash Strength", Range(0, 1)) = 0.0 // 0 = no flash, 1 = full flash
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            float4 _FlashColor;
            float _FlashStrength;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return lerp(half4(0, 0, 0, 0), _FlashColor, _FlashStrength);
            }
            ENDCG
        }
    }
}
