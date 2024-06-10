Shader "Unlit/M_Noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 10.0
        _NoiseStrength ("Noise Strength", Float) = 0.1
        _ColorNoiseStrength ("Color Noise Strength", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _NoiseScale;
            float _NoiseStrength;
            float _ColorNoiseStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // �m�C�Y�𐶐�
                float noise = rand(i.uv * _NoiseScale + _Time.y);

                // UV���W�����E�ɕψ�
                float2 uvOffset = float2(noise * _NoiseStrength, 0);

                // �ψʌ��UV���W�Ńe�N�X�`���̐F���擾
                fixed4 col = tex2D(_MainTex, i.uv + uvOffset);

                // �F�Ƀm�C�Y��K�p
                col.rgb += noise * _ColorNoiseStrength;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
