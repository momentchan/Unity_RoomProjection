Shader "Hidden/Combine"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Int) = 5 // SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DstBlend", Int) = 10 // OneMinusSrcAlpha
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float _Theta;
	ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Blend[_SrcBlend][_DstBlend]

		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float2 rotate(float2 uv, float t) {
				float s, c;
				sincos(t, s, c);
				return float2(
					uv.x * c + uv.y * (-s),
					uv.x * s + uv.y * (c)
					);
			}
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float2 uv = v.uv;

				static const float2 offset = float2(0.5, 0.5);
				uv -= offset;
				uv = rotate(uv, _Theta);
				uv += offset;
				o.uv = uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
