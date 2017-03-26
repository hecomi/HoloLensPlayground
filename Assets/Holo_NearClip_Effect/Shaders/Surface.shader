Shader "HoloLens/NearClip/StandardSurface" 
{

Properties 
{
    _Color("Color", Color) = (1, 1, 1, 1)
    _MainTex("Albedo (RGB)", 2D) = "white" {}
    _Glossiness("Smoothness", Range(0, 1)) = 0.5
    _Metallic("Metallic", Range(0, 1)) = 0.0
    _StartDistance("Start Distance", Float) = 1.2
    _EndDistance("End Distance", Float) = 0.85
}

SubShader 
{
    Tags { "RenderType"="Opaque" }
    LOD 200
    
    CGPROGRAM
    #pragma surface surf Standard fullforwardshadows finalcolor:nearclip_effect
    #pragma target 3.0

    sampler2D _MainTex;

    struct Input 
    {
        float2 uv_MainTex;
        float3 worldPos;
    };

    half _Glossiness;
    half _Metallic;
    fixed4 _Color;
    float _StartDistance;
    float _EndDistance;

    void nearclip_effect(Input IN, SurfaceOutputStandard o, inout fixed4 color)
    {
        fixed3 dist = length(_WorldSpaceCameraPos - IN.worldPos);
        fixed t = (_StartDistance - dist) / (_StartDistance - _EndDistance);
        color *= clamp(1.0 - t, 0.0, 1.0);
    }

    void surf (Input IN, inout SurfaceOutputStandard o) 
    {
        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
        o.Albedo = c.rgb;
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
        o.Alpha = c.a;
    }
    ENDCG
}

FallBack "Diffuse"

}
