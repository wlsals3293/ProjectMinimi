Shader "Custom/CustomLight"
{
    Properties
    {
        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #pragma surface surf navii fullforwardshadows
        float4 Lightingnavii(SurfaceOutput s, float3 lightDir, float atten) {
            float NdotL = dot(s.Normal, lightDir) * 0.5 + 0.5;

            if (NdotL > 0.5) {
                NdotL = 0.8;
            }
            else {
                NdotL = 0.5;
            }
            float4 final;
            final.rgb = s.Albedo * NdotL;
            final.a = s.Alpha;

            return final;
        }
        
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        

        
        

        void surf (Input IN, inout SurfaceOutput o)
        {
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
