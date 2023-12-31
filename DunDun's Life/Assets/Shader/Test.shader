Shader "Custom/Test"
{
    Properties{
        _Color("Color",Color)=(1,1,1,1)
        _MainTex("MainTex",2D)="white"{}
        _Bumpmap("Bumpmap",2D)="bump"{}
        _BumpmapScale("BumpScale",Float)=1.0
        _Specular("Specular",Color)=(1,1,1,1)
        _Gloss("gloss",Range(8.0,256))=20
    }
    Subshader{
        pass{
            Tags{"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Bumpmap;
            float4 _Bumpmap_ST;
            float _BumpScale;
            fixed4 _Specular;
            float _Gloss;

            struct a2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float4 tangent:TANGENT;
                float4 texcoord:TEXCOORD0;
            };

            struct v2f
            {
                float4 pos:SV_POSITION;
                float4 uv:TEXCOORD0;
                float3 lightdir:TEXCOORD1;//存储变换后的光照方向和视角方向
                float3 viewdir:TEXCOORD2;
            };
            v2f vert(a2v v)
            {
                v2f o;
                o.pos=UnityObjectToClipPos(v.vertex);
                o.uv.xy=v.texcoord.xy*_MainTex_ST.xy+_MainTex_ST.zw;
                o.uv.zw=v.texcoord.xy*_Bumpmap_ST.xy+_Bumpmap_ST.zw;
                TANGENT_SPACE_ROTATION;
                o.lightdir=mul(rotation,ObjSpaceLightDir(v.vertex)).xyz;
                o.viewdir=mul(rotation,ObjSpaceViewDir(v.vertex)).xyz;
                return o;
            }
            fixed4 frag(v2f i):SV_Target{
                fixed3 tangentlightdir=normalize(i.lightdir);
                fixed3 tangentviewdir=normalize(i.viewdir);//归一化处理
                fixed4 GetNormal=tex2D(_Bumpmap,i.uv.zw);//对Bumpmap进行采样
                fixed3 tangentNormal;
                tangentNormal=UnpackNormal(GetNormal);
                tangentNormal.xy*=_BumpScale;
                tangentNormal.z=sqrt(1.0-saturate(dot(tangentNormal.xy,tangentNormal.xy)));
                fixed3 albedo=tex2D(_MainTex,i.uv).rgb*_Color.rgb;
                fixed3 ambient=UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
                fixed3 diffuse=_LightColor0.rgb*albedo*max(0,dot(tangentNormal,tangentlightdir));
                fixed3 halfdir=normalize(tangentlightdir+tangentviewdir);
                fixed3 specualr=_LightColor0.rgb*_Specular.rgb*pow(max(0,dot(tangentNormal,halfdir)),_Gloss);
                return fixed4(ambient+diffuse+specualr,1.0);
            }
            ENDCG
        }
    }
    FallBack "Specular"
}
