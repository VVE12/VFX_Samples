Shader "CustomDissolve"
{
    Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		[HDR]_Color ("Main Color", Color) = (1,1,1,1)
		[HDR]_EdgeColor1 ("Edge Color 1", Color) = (1.0, 1.0, 1.0, 1.0)
		[HDR]_EdgeColor2 ("Edge Color 2", Color) = (1.0, 1.0, 1.0, 1.0)
		_Level ("Dissolve Level", Range (0.0, 1.0)) = 0.1
		_Edges ("Edge Width", Range (0.0, 1.0)) = 0.1
	}
	
	SubShader
	{
		Tags 
		{ 
			"RenderPipeline" = "UniversalPipeline"
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			
			Cull Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			half4 _EdgeColor1;
			half4 _EdgeColor2;
			half _Level;
			half _Edges;
			half4 _Color;

			struct Attributes
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
				half4 color: COLOR;
			};

			struct Varyings
			{
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				half4 color: COLOR;
			};		
			
			Varyings vert (Attributes v)
			{
				Varyings o;
				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;
				return o;
			}
			
			half4 frag (Varyings i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv) * i.color;
				half noiseTex = tex2D(_NoiseTex, i.uv).r;

				clip(noiseTex - _Level);
				
				half lerpValue = (noiseTex - _Level) / _Edges;

				if (noiseTex < col.a && noiseTex < _Level + _Edges)
				{
					col = lerp(_EdgeColor1, _EdgeColor2, saturate(lerpValue));
				}

				return col;
			}
			ENDHLSL
		}
	}
}