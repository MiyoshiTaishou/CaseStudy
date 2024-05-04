Shader "Custom/NoiseDissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1.0
        _DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.0
        _DissolveSpeed ("Dissolve Speed", Float) = 1.0
        _Threshold ("Threshold", Range(0.0, 2.0)) = 0.5 // より緩やかな閾値
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        
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
            sampler2D _NoiseTex;
            float _NoiseScale;
            float _DissolveAmount;
            float _DissolveSpeed;
            float _Threshold;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _NoiseScale;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 noiseColor = tex2D(_NoiseTex, i.uv);
                
                // ノイズの明度を計算
                float dissolveFactor = dot(noiseColor.rgb, float3(0.2, 0.7, 0.1));
                
                float dissolveAmount = _DissolveAmount;
                float threshold = _Threshold;
                
                if (dissolveFactor < threshold)
                {
                    discard;
                }
                
                return texColor;
            }
            ENDCG
        }
    }
}
