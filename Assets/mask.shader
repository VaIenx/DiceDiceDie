Shader "Custom/PixelWithMask"
{
    Properties
    {
        _MainTex   ("Haupt-Textur",  2D) = "white" {}
        _MaskTex   ("Masken-Textur", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(0.005, 0.2)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4    _MainTex_ST;
            float     _PixelSize;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f    { float4 pos    : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Maske auslesen (gleiche UVs wie Haupt-Textur)
                fixed4 mask = tex2D(_MaskTex, i.uv);

                // Blau-Kanal > 0.5 → Auge → scharf lassen
                float isEye = step(0.5, mask.b);

                // Scharfer Sample
                fixed4 sharp = tex2D(_MainTex, i.uv);

                // Pixelierter Sample (UVs auf Raster snappen)
                float2 pixelUV = floor(i.uv / _PixelSize) * _PixelSize
                                + _PixelSize * 0.5;
                fixed4 pixelated = tex2D(_MainTex, pixelUV);

                // isEye=1 → scharf, isEye=0 → pixeliert
                return lerp(pixelated, sharp, isEye);
            }
            ENDCG
        }
    }
}