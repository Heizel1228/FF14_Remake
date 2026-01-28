Shader "Custom/UnlitTransparentWithColor"
{
    // Define the properties for the shader
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}   // The texture (e.g., target ring PNG)
        _Color("Color Tint", Color) = (1,1,1,1) // The tint color, including alpha for transparency
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                // Define vertex input and output structures
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                // Declare shader properties
                sampler2D _MainTex;      // Texture for the ring
                float4 _Color;           // Tint color with transparency

                // Vertex shader: transform vertices to screen space
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                // Fragment shader: apply texture and tint color
                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 texColor = tex2D(_MainTex, i.uv); // Sample the texture color
                    return texColor * _Color;               // Multiply by the tint color (supports alpha)
                }
                ENDCG
            }
        }

            FallBack "Unlit/Transparent"
}