﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Image Effects/RadialBlurFilter" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SampleDist ("SampleDist", float) = 0.2
		_SampleStrength ("SampleStrength", float) = 3.0
		_SamplePosX ("SamplePosX", float) = 0.5
		_SamplePosY ("SamplePosY", float) = 0.5
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }

			CGPROGRAM
			// Upgrade NOTE: excluded shader from Xbox360 and OpenGL ES 2.0 because it uses unsized arrays 
			#pragma exclude_renderers xbox360 gles
			#pragma exclude_renderers xbox360
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			uniform float _SampleDist;
			uniform float _SampleStrength;
			uniform float _SamplePosX;
			uniform float _SamplePosY;

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float2 texCo = i.uv;

				float2 spos = float2(_SamplePosX, _SamplePosY);
				float2 dir = spos - texCo;
				float dist = length(dir);

				dir /= dist;

				float4 color = tex2D(_MainTex, texCo);
				float4 sum = color;
				float samples[10] = { -0.08,-0.05,-0.03,-0.02,-0.01,0.01,0.02,0.03,0.05,0.08 };


				for (int i = 0; i < 10; ++i)
				{
					sum += tex2D(_MainTex, texCo + dir * samples[i] * _SampleDist);
				}

				sum /= 11.0;
				float t = saturate(dist * _SampleStrength);
				return lerp(color, sum, t);
			}
			ENDCG
		}
	}
	Fallback off
}