Shader "Custom/Cloak" {

	Properties{
		// Cloak off
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		// Transition
		_TransitionMap("TransitionMap", 2D) = "white" {}
		_TransitionScale("TransitionScale", Range(0,20)) = 1.0
		_Transition("Transition", Range(0,1)) = 0
		// Cloak on
		_DistortionMap("DistortionMap", 2D) = "black" {}
		_DistortionScale("DistortionScale", Range(0,1)) = 0.0
		_RippleScale("RippleScale", Range(0, 20)) = 0.0
		_RippleSpeed("RippleSpeed", Range(0, 20)) = 0.0
	}

	SubShader {

		Tags{"RenderType"="Opaque"  "Queue"="Transparent+0"  "IsEmissive"="true"}
		Cull Back
		GrabPass {"_GrabTexture"}

		CGPROGRAM

		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows

		struct Input {
			float4 screenPos : TEXCOORD0;
			float2 uv_MainTex : TEXCOORD1;
		};

		// Cloak off
		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Trainsition
		sampler2D _TransitionMap;
		float _TransitionScale;
		float _Transition;

		// Cloak on
		sampler2D _DistortionMap;
		float _DistortionScale;
		uniform sampler2D _GrabTexture;
		float _RippleScale;
		float _RippleSpeed;

		void surf(Input i, inout SurfaceOutputStandard o) {

			fixed4 t = tex2D(_TransitionMap, i.screenPos * _TransitionScale);

			if (t.r - _Transition > 0) {
				
				// Standard unity surface
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, i.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;

			} else {

				o.Albedo = float3(0, 0, 0);

				// Calculate distorted position
				float2 uv_distort = (_Time.y * _RippleSpeed) + i.screenPos.xy;
				uv_distort *= _RippleScale;
				fixed4 distort = tex2D(_DistortionMap, uv_distort) * _DistortionScale;
				i.screenPos += distort;

				// Get color at distorted position
				float4 final = tex2Dproj(_GrabTexture, i.screenPos);

				o.Emission = final.rgb;
				o.Metallic = 0;
				o.Smoothness = 0;
				o.Alpha = 0;

			}

		}

		ENDCG

	}
	Fallback "Diffuse"

}