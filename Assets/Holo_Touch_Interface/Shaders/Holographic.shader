Shader "HoloLens/Holographic"
{

Properties
{
    [KeywordEnum(Property, Camera)]
	_Method("DestructionMethod", Float) = 0
	_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
	_MainTex("Particle Texture", 2D) = "white" {}
    _StartDistance("Start Distance", Float) = 0.6
    _EndDistance("End Distance", Float) = 0.3
}

CGINCLUDE

#include "UnityCG.cginc"

#define PI 3.1415926535

sampler2D _MainTex;
fixed4 _MainTex_ST;
fixed4 _TintColor;
float _StartDistance;
float _EndDistance;
float _SmoothStepStart;
float _SmoothStepEnd;

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
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata_t v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
    fixed3 dist = length(_WorldSpaceCameraPos - worldPos);
    fixed proximity = clamp(1.0 - (_StartDistance - dist) / (_StartDistance - _EndDistance), 0.0, 1.0);

    fixed3 viewNormal = mul(UNITY_MATRIX_MV, v.normal);
    fixed intensity = abs(viewNormal.z);
    intensity = smoothstep(0.1, 1.0, intensity);

    o.vertex = UnityObjectToClipPos(v.vertex);
    o.color = v.color;
    o.color.a *= proximity * intensity;
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return i.color * _TintColor * tex2D(_MainTex, i.uv);
}

ENDCG

SubShader
{

Tags
{
    "RenderType" = "Transparent"
    "Queue" = "Geometry"
    "IgnoreProjector" = "True"
    "PreviewType" = "Plane"
}

Blend SrcAlpha One
ColorMask RGB
Cull Back
Lighting Off 
ZWrite Off

Pass
{
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 5.0
    ENDCG
}

}

}