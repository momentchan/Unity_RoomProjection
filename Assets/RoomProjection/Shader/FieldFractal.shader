Shader "Hidden/FieldFractal"
{
	Properties
	{
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Assets/Packages/unity-gist/Cginc/Fbm.cginc"

			float _GlobalTime;

			float3 _NoiseParams0; // Frequency, Speed, Phase
			float3 _NoiseParams1; // Frequency, Speed, Phase
			float3 _NoiseParams2; // Frequency, Speed, Phase


			fixed4 frag(v2f_img i) : SV_Target
			{
				float2 seed = i.uv.xy;
				float t = _GlobalTime;

				float n0 = 0.5 + 0.5 * fbm4(seed.xy * _NoiseParams0.x, t * _NoiseParams0.y + _NoiseParams0.z);
				float n1 = 0.5 + 0.5 * fbm3(seed.xy * _NoiseParams1.x, t * _NoiseParams1.y + _NoiseParams1.z);
				float n2 = 0.5 + 0.5 * fbm2(seed.xy * _NoiseParams2.x, t * _NoiseParams2.y + _NoiseParams2.z);

				return float4(n0, n1, n2, 1.0);
			}
			ENDCG
		}
	}
}
