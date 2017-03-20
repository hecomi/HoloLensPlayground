Shader "HoloLens/NonDestructionAdditive"
{

Properties
{
    [KeywordEnum(Property, Camera)]
	_Method("DestructionMethod", Float) = 0
	_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
	_MainTex("Particle Texture", 2D) = "white" {}
	_InvFade("Soft Particles Factor", Range(0.01, 3.0)) = 1.0
    _StartDistance("Start Distance", Float) = 0.6
    _EndDistance("End Distance", Float) = 0.3
}

CGINCLUDE

#include "UnityCG.cginc"

#define PI 3.1415926535

sampler2D _MainTex;
fixed4 _MainTex_ST;
fixed4 _TintColor;
sampler2D_float _CameraDepthTexture;
fixed _InvFade;
fixed _StartDistance;
fixed _EndDistance;

struct appdata_t 
{
    float4 vertex : POSITION;
    float4 normal : NORMAL;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f 
{
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_FOG_COORDS(1)
#ifdef SOFTPARTICLES_ON
        float4 projPos : TEXCOORD2;
#endif
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata_t v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    fixed4 worldPos = mul(unity_ObjectToWorld, v.vertex);
    fixed3 dist = length(_WorldSpaceCameraPos - worldPos);
    fixed alpha = clamp(1.0 - (_StartDistance - dist) / (_StartDistance - _EndDistance), 0.0, 1.0);

    o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef SOFTPARTICLES_ON
    o.projPos = ComputeScreenPos(o.vertex);
    COMPUTE_EYEDEPTH(o.projPos.z);
#endif

    o.color = v.color;
    o.color.a *= alpha;
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    UNITY_TRANSFER_FOG(o, o.vertex);

    return o;
}

fixed4 frag(v2f i) : SV_Target
{
#ifdef SOFTPARTICLES_ON
    float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
    float partZ = i.projPos.z;
    float fade = saturate(_InvFade * (sceneZ - partZ));
    i.color.a *= fade;
#endif

    fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
    UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0, 0, 0, 0));
    return col;
}

ENDCG

SubShader
{

Tags
{
    "RenderType" = "Transparent"
    "Queue" = "Transparent"
    "IgnoreProjector"="True"
    "PreviewType"="Plane"
}

Blend SrcAlpha One
ColorMask RGB
Cull Off Lighting Off ZWrite Off

Pass
{
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 5.0
    #pragma multi_compile_particles
    #pragma multi_compile_fog
    ENDCG
}

}

}