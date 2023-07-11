Shader "Custom/TBS"
{
     Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_ColorBlue("Color Blue", Color) = (0,0,1,1)
		_ColorYellow("Color Yellow", Color) = (1,1,0,1)
		_Alpha("Alpha", Range(0,1)) = 0
		_Beta("Beta", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf ToneShading fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		#include "UnityPBSLighting.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _ColorBlue;
		fixed4 _ColorYellow;
		half _Alpha;
		half _Beta;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

		half3 BRDF3_Direct_TBS(half3 diffColor, half3 specColor, half rlPow4, half smoothness, half3 warm, half3 cool)
		{
			half LUT_RANGE = 16.0; // must match range in NHxRoughness() function in GeneratedTextures.cpp
			// Lookup texture to save instructions
			half specular = tex2D(unity_NHxRoughness, half2(rlPow4, SmoothnessToPerceptualRoughness(smoothness))).r * LUT_RANGE;
#if defined(_SPECULARHIGHLIGHTS_OFF)
			specular = 0.0;
#endif

			return diffColor + specular * specColor;
		}

		half3 BRDF3_Indirect_TBS(half3 diffColor, half3 specColor, UnityIndirect indirect,
 half grazingTerm, half fresnelTerm, half3 warm, half3 cool)
		{
			fixed t = (1 + indirect.diffuse) * 0.5;
			half3 c = lerp(cool, warm, 1 - t);//indirect.diffuse * diffColor;
			c += indirect.specular * lerp(specColor, grazingTerm, fresnelTerm);
			return c;
		}

		half4 BRDF3_Unity_PBS_TBS(half3 diffColor, half3 specColor, half oneMinusReflectivity, half smoothness,
			float3 normal, float3 viewDir,
			UnityLight light, UnityIndirect gi)
		{
			float3 reflDir = reflect(viewDir, normal);

			half nl = saturate(dot(normal, light.dir));
			half nv = saturate(dot(normal, viewDir));

			// Vectorize Pow4 to save instructions
			half2 rlPow4AndFresnelTerm = Pow4(float2(dot(reflDir, light.dir), 1 - nv));  
			half rlPow4 = rlPow4AndFresnelTerm.x; 
			half fresnelTerm = rlPow4AndFresnelTerm.y;

			half grazingTerm = saturate(smoothness + (1 - oneMinusReflectivity));

			fixed3 cool = lerp(_ColorBlue.rgb, diffColor, _Alpha);
			fixed3 warm = lerp(_ColorYellow.rgb, diffColor, _Beta);

			half3 color = BRDF3_Direct_TBS(diffColor, specColor, rlPow4, smoothness, warm, cool);
			color *= light.color * nl;
			color += BRDF3_Indirect_TBS(diffColor, specColor, gi, grazingTerm, fresnelTerm, warm, cool);

			return half4(color, 1);
		}

		inline half4 LightingToneShading(SurfaceOutputStandard s, float3 viewDir, UnityGI gi)
		{
			s.Normal = normalize(s.Normal);

			half oneMinusReflectivity;
			half3 specColor;
			s.Albedo = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, /*out*/ specColor, /*out*/ oneMinusReflectivity);

			// shader relies on pre-multiply alpha-blend (_SrcBlend = One, _DstBlend = OneMinusSrcAlpha)
			// this is necessary to handle transparency in physically correct way - only diffuse component gets affected by alpha
			half outputAlpha;
			s.Albedo = PreMultiplyAlpha(s.Albedo, s.Alpha, oneMinusReflectivity, /*out*/ outputAlpha);

			half4 c = BRDF3_Unity_PBS_TBS(s.Albedo, specColor, oneMinusReflectivity, 
s.Smoothness, s.Normal, viewDir, gi.light, gi.indirect);
			c.a = outputAlpha;
			return c;
		}

		inline void LightingToneShading_GI(
			SurfaceOutputStandard s,
			UnityGIInput data,
			inout UnityGI gi)
		{
#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
			gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
#else
			Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(s.Smoothness, data.worldViewDir, s.Normal,
 lerp(unity_ColorSpaceDielectricSpec.rgb, s.Albedo, s.Metallic));
			gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);
#endif
		}

        ENDCG
    }
    FallBack "Diffuse"
}
