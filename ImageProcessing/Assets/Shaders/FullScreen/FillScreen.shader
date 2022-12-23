Shader "Hidden/FillScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InvertUVs ("Invert UVs", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            uniform float4 _InvertUVs;

            fixed4 frag (v2f_img i) : SV_Target
            {
                i.uv = lerp(i.uv, 1 - i.uv, _InvertUVs.xy);
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
