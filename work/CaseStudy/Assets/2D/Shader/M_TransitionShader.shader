Shader "Unlit/M_TransitionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // 臒l��ݒ肷�鏊
        _Val("Val", Range(-1.0, 1.0)) = 1.0

        //�F�����߂�
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // �A���t�@�u�����h�ݒ�H
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
            float4 _MainTex_ST;
            // �A���t�@�l�p�̕ϐ�
            float _Val;

             float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float output = saturate(col.x - _Val);

                // 臒l�����ύX
                return fixed4(_Color.xyz, output);
            }
            ENDCG
        }
    }
}
