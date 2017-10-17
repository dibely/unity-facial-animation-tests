Shader "Eye"
{
	Properties 
	{
		_Color("_Color", Color) = (1,1,1,0)
		_Shininess("Shininess", Range(0.01,1) ) = 1
		_MainTex("_MainTex", 2D) = "black" {}
		_OffsetX("_OffsetX", Range(-0.5,0.5) ) = 0
		_OffsetY("_OffsetY", Range(-0.5,0.5) ) = 0
	}
	
	SubShader 
	{
		Tags
		{
			"Queue"="Geometry"
			"IgnoreProjector"="False"
			"RenderType"="Opaque"
		}

		
		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Fog{
		}


		CGPROGRAM
		#pragma surface surf BlinnPhongEditor  vertex:vert
		#pragma target 2.0

		float4 _Color;
		float _Shininess;
		sampler2D _MainTex;
		float _OffsetX;
		float _OffsetY;

		struct EditorSurfaceOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
			half4 Custom;
		};
			
		inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light) {
			half3 spec = light.a * s.Gloss;
			half4 c;
			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
			c.a = s.Alpha;
			return c;
		}

		inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);
				
			half diff = max (0, dot ( lightDir, s.Normal ));
				
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0);
				
			half4 res;
			res.rgb = _LightColor0.rgb * diff;
			res.w = spec * Luminance (_LightColor0.rgb);
			res *= atten * 2.0;

			return LightingBlinnPhongEditor_PrePass( s, res );
		}
			
		struct Input {
			float2 uv_MainTex;
		};

		void vert (inout appdata_full v, out Input o) {
			o.uv_MainTex = v.texcoord;
		}
			

		void surf (Input IN, inout EditorSurfaceOutput o) {
			o.Normal = float3(0.0,0.0,1.0);
			o.Alpha = 1.0;
			o.Emission = 0.0;
			o.Custom = 0.0;
				
			float2 Split0=(IN.uv_MainTex.xy);
			float Add0=_OffsetX.x + Split0.x;
			float Add1=_OffsetY.x + Split0.y;
			float4 Tex2D0=tex2D(_MainTex, float2(Add0, Add1));
			float4 Multiply0=_Color * Tex2D0;
			o.Albedo = Multiply0;
			o.Specular = _Shininess.xxxx;
			o.Gloss = _Color.w;
			o.Normal = normalize(o.Normal);
		}
		ENDCG
	}
	Fallback "Diffuse"
}