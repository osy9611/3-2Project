Shader "Custom/GrassShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_AlphaCutout("_AlphaCutout", Range(0, 1.01)) = 0.5
		//라이트 맵을 위한 것
		[Header(LightMap)]
		[Toggle]_USELIGHTMAP("UseLightMap", float) = 0
		[Toggle]_USELIGHTMAPCOLOR("UseLightMapColor", float) = 0
		[HDR]_LightMapColor("LightMapColor", color) = (0, 0, 0, 0)

		[Header(VertexAnimation)]
		[Toggle]_USEVERTEXANIMTION("_UseVertexAnim", float) = 0	//애니메이션을 사용할것이지 물어보는 것
		_MoveAmount("MoveAmount", float) = 0.2					//움직임의 양
		_MoveSpeed("MoveSpeed", float) = 2						//움직이는 속도
		_MoveTilling("MoveTilling", float) = 2					
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			cull off	//모든 면을 그린다
			Lighting OFF	//라이트는 끔

			Pass
			{
				Lighting OFF	//라이팅을 끔

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			//라이트맵 사용
			#pragma shader_feature _USELIGHTMAP_OFF _USELIGHTMAP_ON
			#pragma shader_feature _USELIGHTMAPCOLOR_OFF _USELIGHTMAPCOLOR_ON
			#pragma shader_feature _USEVERTEXANIMTION_OFF _USEVERTEXANIMTION_ON

			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct appdata_lightmap {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv1 : TEXCOORD3;

				//월드 포지션
				float4 worldSpacePos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _AlphaCutout;
			half3 _LightMapColor;

			//vertexAnimationData
			half _MoveAmount;
			half _MoveSpeed;
			half _MoveTilling;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				//VertexAnimation
				#if _USEVERTEXANIMTION_ON
				o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex); //월드 포지션
				o.vertex.x += (sin((o.worldSpacePos.z * _MoveTilling) + _Time.y * _MoveSpeed) * _MoveAmount) * o.uv.y;
				#endif

				//라이트 맵
				#if _USELIGHTMAP_ON
				// Use `unity_LightmapST` NOT `unity_Lightmap_ST`
				o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			clip(col.a - _AlphaCutout);

			//라이트맵 컬러 적용
			#if _USELIGHTMAP_ON
			col.rgb *= (DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1))
				#if _USELIGHTMAPCOLOR_ON //라이트맵에 컬러 사용
				+ _LightMapColor
				#endif
			);
			#endif

			return col;
		}
		ENDCG
	}
		}
}
