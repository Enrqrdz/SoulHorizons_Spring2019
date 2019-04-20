// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Deprecated" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Outline("_Outline", Range(0,0.1)) = 0
		_OutlineColor("Color", Color) = (1, 1, 1, 1)
	}
		SubShader{
			Pass {
				Tags { "RenderType" = "Opaque" }
				Cull Front

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f 
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				float _Outline;
				float4 _OutlineColor;

				float4 vert(appdata_base v) : SV_POSITION {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
					normal.x *= UNITY_MATRIX_P[0][0];
					normal.y *= UNITY_MATRIX_P[1][1];
					o.vertex.xy += normal.xy * _Outline;
					return o.vertex;
				}

				half4 frag(v2f i) : COLOR {
					return _OutlineColor;
				}

				ENDCG
			}

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord)) * IN.color;

				//color += half4(1, 1, 1, 0);

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif


				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif



				return color;
			}

			ENDCG
		}
			FallBack "Diffuse"

}

