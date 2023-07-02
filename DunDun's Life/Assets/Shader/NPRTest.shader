Shader "Custom/NPRTest"
{
    Properties{
        _Color("Color",Color)=(1,1,1,1)
        _MainTex("MainTex",2D)="white"{}
        _Ramp("Ramp Texture",2D)="white"{}//用于控制漫反射的渐变纹理
        _Outline("Outline",Range(0,1))=0.1//用于控制轮廓线的宽度
        _outlineColor("OutLineColor",Color)=(0,0,0,1)
        _Specular("Specular",Color)=(1,1,1,1)
        _SpecularScale("SpecularScale",Range(0,0.1))=0.01//用于计算控制高光反射的阈值
    }
    Subshader{
        pass{
            NAME"OUTLINE"
            Cull Front//使用“Cull”命令把正面的三角形面片剔除，只渲染背面
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _Ramp;
            float _Outline;
            fixed4 _OutlineColor;
            fixed4 _Specular;
            float _SpecularScale;
            float4 _MainTex_ST;
            struct v2f
            {
                float4 pos:POSITION;
                float2 uv:TEXCOORD0;
                float3 worldNormal:TEXCOORD1;
                float3 worldPos:TEXCOORD2;
                float4 vertColor:COLOR;
            };

            struct a2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float4 texcoord:TEXCOORD0;
                float4 tangent:TANGENT;
                float4 vertColor:COLOR;
            };
            v2f vert(a2v v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f,o);
                float4 pos=UnityObjectToClipPos(v.vertex);
                float3 normal=mul((float3x3)UNITY_MATRIX_IT_MV,v.tangent.xyz);
                float3 ndcnormal=normalize(TransformViewToProjection(normal.xyz))*pos.w;
                float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));//将近裁剪面右上角位置的顶点变换到观察空间
                float aspect = abs(nearUpperRight.y / nearUpperRight.x);//求得屏幕宽高比
                ndcnormal.x *= aspect;
                pos.xy+=0.01*_Outline*ndcnormal.xy*v.vertColor.a;
                o.pos=pos;
                return o;
            }
            float4 frag(v2f i):SV_Target{
                return float4((_OutlineColor*i.vertColor).rgb,0);
            }
            ENDCG
        }
        Pass{
            Tags{"LightMode"="ForwardBase"}
            Cull Back
            CGPROGRAM
            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _Ramp;
            float _Outline;
            fixed4 _OutlineColor;
            fixed4 _Specular;
            float _SpecularScale;
            float4 _MainTex_ST;
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            
            struct v2f
            {
                float4 pos:POSITION;
                float2 uv:TEXCOORD0;
                float3 worldNormal:TEXCOORD1;
                float3 worldPos:TEXCOORD2;
                SHADOW_COORDS(3)
            };

            struct a2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float4 texcoord:TEXCOORD0;
            };
            v2f vert(a2v v)
            {
                v2f o;
                o.pos=UnityObjectToClipPos(v.vertex);
                o.uv=TRANSFORM_TEX(v.texcoord,_MainTex);
                o.worldNormal=mul(v.normal,(float3x3)unity_WorldToObject);
                o.worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;
                TRANSFER_SHADOW(o);
                return o;
            }
            float4 frag(v2f i):SV_Target{
                fixed3 WorldNormal=normalize(i.worldNormal);
                fixed3 WorldLightDir=normalize(UnityWorldSpaceLightDir(i.worldPos));
                fixed3 WorldViewDir=normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed3 WorldHalfDir=normalize(WorldLightDir+WorldViewDir);
                fixed4 c=tex2D(_MainTex,i.uv);
                fixed3 albedo=c.rgb*_Color.rgb;
                fixed3 ambient=UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
                UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);//计算当前世界坐标下的阴影值
                fixed diff=dot(WorldNormal,WorldHalfDir);//半兰伯特漫反射系数
                diff=(diff*0.5+0.5)*atten;//与阴影值相乘得到最终的漫反射系数
                fixed3 diffuse=_LightColor0.rgb*albedo*tex2D(_Ramp,float2(diff,diff)).rgb;
                fixed spec=dot(WorldNormal,WorldHalfDir);
                fixed w=fwidth(spec)*2.0;
                //使用“step(0.0001,_SpecularScale)是为了在_SpecularScale为0时，可以完全消除高光反射的光照”
                fixed3 specular=_Specular.rgb*lerp(0,1,smoothstep(-w,w,spec+_SpecularScale-1))*step(0.0001,_SpecularScale);
                return fixed4(ambient+diffuse+specular,1.0);
            }
            ENDCG
            }
    }
    FallBack "Diffuse"
}
