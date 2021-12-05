Shader "Particles/Slash"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_EmissionTex("EmissionTex", 2D) = "white" {}
		_Opacity("Opacity", Float) = 20
		_Dissolve("Dissolve", 2D) = "white" {}
		_SpeedMainTexUVNoise("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 5
		_Remap("Remap", Vector) = (-2,1,0,0)
		_AddColor("Add Color", Color) = (0,0,0,0)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		ZTest LEqual

		Pass 
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				half4 uv : TEXCOORD0;
				half4 uv2 : TEXCOORD2;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				half4 uv : TEXCOORD0;
				half2 emissionUV: TEXCOORD1;
				half4 uv2 : TEXCOORD2;
				half4 dissolveUV : TEXCOORD3;
			};

			sampler2D _MainTexture;
			sampler2D _EmissionTex;
			sampler2D _Dissolve;

			float4 _MainTexture_ST;
			float4 _EmissionTex_ST;
			float4 _Dissolve_ST;

			float4 _SpeedMainTexUVNoise;
			half4 _AddColor;
			half2 _Remap;
			half _Emission;
			half _Opacity;

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				o.uv2.xy = TRANSFORM_TEX(v.uv2.xy, _MainTexture);
				o.uv2.zw = v.uv2.zw;
				o.emissionUV = TRANSFORM_TEX(v.uv, _EmissionTex);
				o.dissolveUV.xy = TRANSFORM_TEX(v.uv.xy, _Dissolve);
				o.dissolveUV.zw = v.uv.zw;

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				half3 emission = tex2D(_EmissionTex, i.emissionUV).rgb;
				half3 tunedEmission = half3((emission.x + i.uv2.z), emission.y, emission.z);
				half3 remmapedEmission = clamp(_Remap.x + tunedEmission * (_Remap.y - _Remap.x), 0, 1);
				float2 scrollNoiseXY = _Time.y * _SpeedMainTexUVNoise.xy + i.uv2.xy;

				half opacity = clamp((tex2D(_MainTexture, scrollNoiseXY).a * _Opacity) , 0, 1);
				float2 scrollNoiseZW = _Time.y * _SpeedMainTexUVNoise.zw + i.dissolveUV.xy;

				float2 noiseUV = float2(scrollNoiseZW.x , (i.uv2.w + scrollNoiseZW.y));			
				half dissolveColor = tex2D(_Dissolve, noiseUV).r;
				
				half3 col = _AddColor.rgb + _Emission * remmapedEmission * i.color.rgb;

				return half4(col, i.color.a * opacity * dissolveColor);
			}
			ENDCG
		}
	}
}