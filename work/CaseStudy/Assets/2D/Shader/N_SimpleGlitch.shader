Shader "Unlit/N_SimpleGlitch"
{
    Properties{
        _MainTex("Select Texture", 2D) = "white" {}
        _GlitchIntensity("Glitch Intensity", Range(0,1)) = 0.1
        _BlockScale("Block Scale", Range(1,50)) = 10
        _NoiseSpeed("Noise Speed", Range(1,10)) = 10
    }
        SubShader{
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            LOD 100
            Pass {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float random(float2 seeds)
                {
                    return frac(sin(dot(seeds, float2(12.9898, 78.233))) * 43758.5453);
                }

                float blockNoise(float2 seeds)
                {
                    return random(floor(seeds));
                }

                float noiserandom(float2 seeds)
                {
                    return -1.0 + 2.0 * blockNoise(seeds);
                }

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _GlitchIntensity;
                float _BlockScale;
                float _NoiseSpeed;

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 uv = i.uv;
                    float4 color = tex2D(_MainTex, uv);


                    float noise = blockNoise(i.uv.y * _BlockScale);
                    noise += random(i.uv.x) * 0.3;
                    float2 randomvalue = noiserandom(float2(i.uv.y, _Time.y * _NoiseSpeed));
                    uv.x += randomvalue * sin(sin(_GlitchIntensity) * 0.2) * sin(-sin(noise) * 0.2) * frac(_Time.y); //.5 .2
                    //gv.y += randomvalue * sin(sin(_GlitchIntensity) * 2.0) * sin(-sin(noise) * 1.6) * frac(_Time.y); //.5 .2
                    color.r = tex2D(_MainTex, uv + float2(0.006, 0)).r;
                    color.g = tex2D(_MainTex, uv).g;
                    color.b = tex2D(_MainTex, uv - float2(0.008, 0)).b;
                    //color.a = 1.0f;

                    return color;
                }
                ENDCG
            }
        }
}
