Shader "Custom/Glitch" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
        float2 _MainTex_TexelSize;

        float2 _ScanLineJitter; // (displacement, threshold)
        float2 _VerticalJump;   // (amount, time)
        float _HorizontalShake;
        float2 _ColorDrift;     // (amount, time)

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

        float nrand(float x, float y)
        {
            return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
        }

		void surf (Input i, inout SurfaceOutputStandard o)
        {

            float u = i.uv_MainTex.x;
            float v = i.uv_MainTex.y;

            // Scan line jitter
            float jitter = nrand(v, _Time.x) * 2 - 1;
            jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

            // Vertical jump
            float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

            // Horizontal shake
            float shake = (nrand(_Time.x, 2) - 0.5) * _HorizontalShake;

            // Color drift
            float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

            half4 src1 = tex2D(_MainTex, frac(float2(u + jitter + shake, jump)));
            half4 src2 = tex2D(_MainTex, frac(float2(u + jitter + shake + drift, jump)));

            // Color with glitch effect
            fixed4 c = half4(src1.r, src2.g, src1.b, 1);

			o.Albedo = c.rgb * _Color;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
