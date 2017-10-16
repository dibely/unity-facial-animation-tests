Shader "Custom/OctopusEyeLid"
{
	Properties 
	{
		_Color("_Color", Color) = (1,1,1,0.3529412)
		_Shininess("Shininess", Range(0.01,1) ) = 0.886
		_EyeLidTexture("_EyeLidTexture", 2D) = "black" {}
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
		sampler2D _EyeLidTexture;

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
			float2 uv3_EyeLidTexture;
		};

		void vert (inout appdata_full v, out Input o) {
			o.uv3_EyeLidTexture = v.texcoord;
		}
			
		void surf (Input IN, inout EditorSurfaceOutput o) {
			o.Normal = float3(0.0,0.0,1.0);
			o.Alpha = 1.0;
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Gloss = 0.0;
			o.Specular = 0.0;
			o.Custom = 0.0;
				
			float4 Tex2D0=tex2D(_EyeLidTexture,(IN.uv3_EyeLidTexture.xyxy).xy);
			float4 Multiply0=_Color * Tex2D0;
			float4 Splat0=_Color.w;
			float4 Subtract0=Tex2D0.aaaa - float4( 0.5,0.5,0.5,0.5 );
			float4 Master0_1_NoInput = float4(0,0,1,1);
			float4 Master0_2_NoInput = float4(0,0,0,0);
			float4 Master0_5_NoInput = float4(1,1,1,1);
			float4 Master0_7_NoInput = float4(0,0,0,0);
			clip( Subtract0 );
			o.Albedo = Multiply0;
			o.Specular = _Shininess.xxxx;
			o.Gloss = Splat0;
			o.Normal = normalize(o.Normal);
		}
		ENDCG
	}
	Fallback "Diffuse"
}