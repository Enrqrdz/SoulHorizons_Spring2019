// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Sprite Color", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineThickness("Outline Thickness", Range(1.0,200.0)) = 4.0
	}

		SubShader
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					#include "UnityCG.cginc"

					sampler2D _MainTex;
					fixed4 _Color;

					struct appdata_t
					{
						float4 vertex   : POSITION;
						float4 color    : COLOR;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f
					{
						float4 pos : SV_POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
					};

					v2f vert(appdata_t IN)
					{
						v2f OUT;

						OUT.pos = UnityObjectToClipPos(IN.vertex);
						OUT.uv = IN.texcoord;

						OUT.color = IN.color *_Color;

						return OUT;
					}

					float4 _MainTex_TexelSize;
					fixed4 _OutlineColor;
					float _OutlineThickness;

					fixed4 frag(v2f IN) : COLOR
					{
						half4 color = (tex2D(_MainTex, IN.uv)) * IN.color;

						color.rgb *= color.a;


						half4 outlineColor = _OutlineColor;
						outlineColor.a *= ceil(color.a);
						outlineColor.rgb *= color.a;

						fixed northAlpha = tex2D(_MainTex, IN.uv + fixed2(0, _MainTex_TexelSize.y) * _OutlineThickness).a;
						fixed southAlpha = tex2D(_MainTex, IN.uv - fixed2(0, _MainTex_TexelSize.y) * _OutlineThickness).a;
						fixed westAlpha = tex2D(_MainTex, IN.uv - fixed2(_MainTex_TexelSize.x, 0) * _OutlineThickness).a;
						fixed eastAlpha = tex2D(_MainTex, IN.uv + fixed2(_MainTex_TexelSize.x , 0) * _OutlineThickness).a;

						fixed sumAlpha = northAlpha * southAlpha * westAlpha * eastAlpha;


						return lerp(outlineColor, color, ceil(sumAlpha));
					}

				ENDCG
			}
		}
}
