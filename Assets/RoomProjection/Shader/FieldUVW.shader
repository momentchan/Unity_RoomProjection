Shader "Hidden/FieldUVW"
{
    Properties
    {
        _SrcBlend("SrcBlend", Int) = 5 // SrcAlpha
        _DstBlend("DstBlend", Int) = 10 // OneMinusSrcAlpha
        _ZWrite("ZWrite", Int) = 0 // On
        _ZTest("ZTest", Int) = 4 // LEqual
        _Cull("Cull", Int) = 0 // Off
        _ZBias("ZBias", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Blend[_SrcBlend][_DstBlend]
        ZWrite[_ZWrite]
        ZTest[_ZTest]
        Cull[_Cull]
        Offset[_ZBias],[_ZBias]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.uv.xyz, 1);
            }
            ENDCG
        }
    }
}
