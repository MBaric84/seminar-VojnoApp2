Shader "Unlit/MapTransparentShader" {
    Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Transparency ("Transparency", Range(0,1)) = 1.0
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 
        
        Pass {  
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Transparency; // New transparency variable
                
                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord);
                    col.a *= _Transparency; // Apply transparency
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
            ENDCG
        }
    }

}
