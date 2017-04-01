Shader "HoloLens/UI/SpriteWithOcclusion"
{

Properties
{
    [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
    _Color("Tint", Color) = (1, 1, 1, 1)
    [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    _OcclusionColor("OcclusionColor", Color) = (1, 1, 1, 1)
    _FadeStartDistance("Start Distance", Float) = 1.2
    _FadeEndDistance("End Distance", Float) = 0.85
}

SubShader
{

Tags
{ 
    "Queue"="Transparent" 
    "IgnoreProjector"="True" 
    "RenderType"="Transparent" 
    "PreviewType"="Plane"
    "CanUseSpriteAtlas"="True"
}

CGINCLUDE

#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float2 texcoord  : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

float _FadeStartDistance;
float _FadeEndDistance;
fixed4 _Color;
fixed4 _OcclusionColor;

v2f vert(appdata_t v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.texcoord = v.texcoord;
    o.color = v.color * _Color;
#ifdef PIXELSNAP_ON
    o.vertex = UnityPixelSnap (o.vertex);
#endif

    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
    float3 dist = length(_WorldSpaceCameraPos - worldPos);
    fixed t = (_FadeStartDistance - dist) / (_FadeStartDistance - _FadeEndDistance);
    o.color.a *= clamp(1.0 - t, 0.0, 1.0);

    return o;
}

sampler2D _MainTex;
sampler2D _AlphaTex;

fixed4 SampleSpriteTexture(float2 uv)
{
    fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
    color.a = tex2D (_AlphaTex, uv).r;
#endif

    return color;
}

fixed4 frag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
    c.rgb *= c.a;
    return c;
}

fixed4 occlusion_frag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color * _OcclusionColor;
    c.rgb *= c.a;
    return ;
}

ENDCG

Pass
{
    Cull Off
    Lighting Off
    ZWrite Off
    ZTest LEqual
    Blend One OneMinusSrcAlpha

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 2.0
    #pragma multi_compile _ PIXELSNAP_ON
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    ENDCG
}

Pass
{
    Cull Off
    Lighting Off
    ZWrite Off
    ZTest Greater
    Blend One OneMinusSrcAlpha

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment occlusion_frag
    #pragma target 2.0
    #pragma multi_compile _ PIXELSNAP_ON
    #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
    #include "UnityCG.cginc"
    ENDCG
}

}

}
