// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		_OutlineTex("Outline Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(0.5,10.0)) = 1.1
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					
					#include "UnityCG.cginc"
					#include "UnityUI.cginc"

					#pragma multi_compile __ UNITY_UI_CLIP_RECT
					#pragma multi_compile __ UNITY_UI_ALPHACLIP

					struct appdata
					{
						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f 
					{
						float4 pos : SV_POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
						float4 worldPosition : TEXCOORD1;
						UNITY_VERTEX_OUTPUT_STEREO
					};

					float _OutlineWidth;
					float4 _OutlineColor;
					sampler2D _OutlineTex;
					float4 _ClipRect;
					fixed4 _TextureSampleAdd;

					v2f vert(appdata IN) 
					{
						IN.vertex.xyz *= _OutlineWidth;

						v2f OUT;

						OUT.pos = UnityObjectToClipPos(IN.vertex);
						OUT.uv = IN.uv * _OutlineColor;

						return OUT;
					}

					fixed4 frag(v2f IN) : SV_Target
					{
						//half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
						float4 texColor = (tex2D(_OutlineTex, IN.uv) + _TextureSampleAdd * IN.color);

						return texColor * _OutlineColor;
					}

				ENDCG
			}

			Pass
			{
				CGPROGRAM
					#pragma vertex SpriteVert
					#pragma fragment SpriteFrag
					#pragma target 2.0
					#pragma multi_compile_instancing
					#pragma multi_compile _ PIXELSNAP_ON
					#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
					#include "UnitySprites.cginc"
				ENDCG
			}
		}
}
