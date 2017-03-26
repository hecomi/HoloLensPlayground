Shader "HoloLens/NearClip/DestructionAdditiveGS"
{

Properties
{
    [KeywordEnum(Property, Camera)]
    _Method("DestructionMethod", Float) = 0
    _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
    _MainTex("Particle Texture", 2D) = "white" {}
    _InvFade("Soft Particles Factor", Range(0.01, 3.0)) = 1.0
    _Destruction("Destruction Factor", Range(0.0, 1.0)) = 0.0
    _PositionFactor("Position Factor", Range(0.0, 1.0)) = 0.2
    _RotationFactor("Rotation Factor", Range(0.0, 1.0)) = 1.0
    _ScaleFactor("Scale Factor", Range(0.0, 1.0)) = 1.0
    _AlphaFactor("Alpha Factor", Range(0.0, 1.0)) = 1.0
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
fixed _Destruction;
fixed _PositionFactor;
fixed _RotationFactor;
fixed _ScaleFactor;
fixed _AlphaFactor;
fixed _StartDistance;
fixed _EndDistance;

struct appdata_t 
{
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct g2f
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

inline float rand(float2 seed)
{
    return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float3 rotate(float3 p, float3 rotation)
{
    float3 a = normalize(rotation);
    float angle = length(rotation);
    if (abs(angle) < 0.001) return p;
    float s = sin(angle);
    float c = cos(angle);
    float r = 1.0 - c;
    float3x3 m = float3x3(
        a.x * a.x * r + c,
        a.y * a.x * r + a.z * s,
        a.z * a.x * r - a.y * s,
        a.x * a.y * r - a.z * s,
        a.y * a.y * r + c,
        a.z * a.y * r + a.x * s,
        a.x * a.z * r + a.y * s,
        a.y * a.z * r - a.x * s,
        a.z * a.z * r + c
    );
    return mul(m, p);
}

appdata_t vert(appdata_t v)
{
    return v;
}

[maxvertexcount(3)]
void geom(triangle appdata_t input[3], inout TriangleStream<g2f> stream)
{
    float3 center = (input[0].vertex + input[1].vertex + input[2].vertex).xyz / 3;

    float3 vec1 = input[1].vertex - input[0].vertex;
    float3 vec2 = input[2].vertex - input[0].vertex;
    float3 normal = normalize(cross(vec1, vec2));

#ifdef _METHOD_PROPERTY
    fixed destruction = _Destruction;
#else
    float4 worldPos = mul(unity_ObjectToWorld, float4(center, 1.0));
    float3 dist = length(_WorldSpaceCameraPos - worldPos);
    fixed destruction = clamp((_StartDistance - dist) / (_StartDistance - _EndDistance), 0.0, 1.0);
#endif

    fixed r = 2 * (rand(center.xy) - 0.5);
    fixed3 r3 = fixed3(r, r, r);

    [unroll]
    for (int i = 0; i < 3; ++i)
    {
        appdata_t v = input[i];

        g2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

        /*
#ifdef _METHOD_PROPERTY
        fixed destruction = _Destruction;
#else
        float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
        float3 dist = length(_WorldSpaceCameraPos - worldPos);
        fixed destruction = clamp((_StartDistance - dist) / (_StartDistance - _EndDistance), 0.0, 1.0);
#endif*/

        // Scale
        v.vertex.xyz = (v.vertex.xyz - center) * (1.0 - destruction * _ScaleFactor) + center;
        // Rotation
        v.vertex.xyz = rotate(v.vertex.xyz - center, r3 * destruction * _RotationFactor) + center;
        // Move
        v.vertex.xyz += normal * destruction * _PositionFactor * r3;

        o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef SOFTPARTICLES_ON
        o.projPos = ComputeScreenPos(o.vertex);
        COMPUTE_EYEDEPTH(o.projPos.z);
#endif

        o.color = v.color;
        o.color.a *= 1.0 - destruction * _AlphaFactor;
        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
        UNITY_TRANSFER_FOG(o, o.vertex);

        stream.Append(o);
    }
    stream.RestartStrip();
}

fixed4 frag(g2f i) : SV_Target
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
    "IgnoreProjector" = "True"
    "PreviewType" = "Plane"
}

Blend SrcAlpha One
ColorMask RGB
Cull Off Lighting Off ZWrite Off

Pass
{
    CGPROGRAM
    #pragma vertex vert
    #pragma geometry geom
    #pragma fragment frag
    #pragma target 5.0
    #pragma multi_compile_particles
    #pragma multi_compile_fog
    #pragma multi_compile _METHOD_PROPERTY _METHOD_CAMERA
    ENDCG
}

}

}