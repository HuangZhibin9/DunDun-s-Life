Shader "Custom/NPRTest"
{
    Properties
    {
        _Color("Color",Color)=(1,1,1,1)
        _MainTex("MainTex",2D)="white"{}
        _Outline("Outline",Range(0,1))=0.1//用于控制轮廓线的宽度
        _outlineColor("OutLineColor",Color)=(0,0,0,1)
        _MainColor("MainColor",Color)=(1,1,1,1)
        _ShadowColor("ShadowColor",Color)=(0.7,0.7,0.8,1)
        _ShadowRange("ShadowRange",Range(0,1))=0.5
        _ShadowSmooth("ShadowSmooth",Range(0,1))=0.2
    }
    Subshader
    {
        pass
        {
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
            fixed4 _MainColor;
            fixed4 _ShadowColor;
            float _ShadowRange;
            float _ShadowSmooth;
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
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                float4 pos = UnityObjectToClipPos(v.vertex);
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.tangent.xyz);
                float3 ndcnormal = normalize(TransformViewToProjection(normal.xyz)) * pos.w;
                float4 nearUpperRight = mul(unity_CameraInvProjection,
                                            float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));
                //将近裁剪面右上角位置的顶点变换到观察空间
                float aspect = abs(nearUpperRight.y / nearUpperRight.x); //求得屏幕宽高比
                ndcnormal.x *= aspect;
                pos.xy += 0.01 * _Outline * ndcnormal.xy * v.vertColor.a;
                o.pos = pos;
                return o;
            }

            float4 frag(v2f i):SV_Target
            {
                return float4((_OutlineColor * i.vertColor).rgb, 0);
            }
            ENDCG
        }
        Pass
        {
            Tags
            {
                "LightMode"="ForwardBase"
            }
            Cull Back
            CGPROGRAM
            fixed4 _Color;
            sampler2D _MainTex;
            float _Outline;
            fixed4 _OutlineColor;
            float4 _MainTex_ST;
            fixed4 _MainColor;
            fixed4 _ShadowColor;
            float _ShadowRange;
            float _ShadowSmooth;


            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include"UnityCG.cginc"

            struct v2f
            {
                float4 pos:SV_POSITION;
                float2 uv:TEXCOORD0;
                float3 worldnormal:TEXCOORD1;
                float3 worldpos:TEXCOORD2;
            };

            struct a2v
            {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
                float4 uv:TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldnormal = UnityObjectToWorldNormal(v.normal);
                o.worldpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i):SV_Target
            {
                fixed4 col = 1;
                fixed3 WorldNormal = normalize(i.worldnormal);
                fixed3 WorldLightDir = normalize(UnityWorldSpaceLightDir(i.worldpos));
                fixed3 WorldViewDir = normalize(UnityWorldSpaceViewDir(i.worldpos));
                fixed3 WorldHalfDir = normalize(WorldLightDir + WorldViewDir);
                fixed4 c = tex2D(_MainTex, i.uv);
                fixed halflambert = dot(WorldNormal,WorldLightDir)*0.5+0.5;
                fixed ramp=smoothstep(0,_ShadowSmooth,halflambert-_ShadowRange);
                fixed3 diffuse=lerp(_ShadowColor,_MainColor,ramp);
                diffuse*=c;
                col.rgb=_LightColor0*diffuse;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}