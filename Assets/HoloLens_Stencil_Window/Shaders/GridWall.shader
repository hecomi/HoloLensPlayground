Shader "HoloLens/GridWall"
{

Properties
{
    _Color("Color", Color) = (1, 1, 1, 1)
    _LineScale("LineScale", Range(0, 100)) = 5
    _LinesPerMeter("LinesPerMeter", Float) = 25
    _Mask("Mask", Int) = 1
}

SubShader
{

Tags 
{ 
    "RenderType"="Opaque" 
    "Queue"="Geometry-1"
}

CGINCLUDE

#include "UnityCG.cginc"

struct v2f
{
    float4 vertex : SV_POSITION;
    float4 worldPos: TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

fixed _LineScale;
fixed _LinesPerMeter;
fixed4 _Color;

v2f vert(appdata_base v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    return o;
}

fixed4 frag_occlusion(v2f i) : SV_Target
{
    return 0;
}

fixed4 frag_grid(v2f i) : SV_Target
{
    float4 p = fmod(abs(i.worldPos) * _LinesPerMeter, 1.0);
    fixed3 c1 = pow(p.xyz, 100.0 / _LineScale);
    fixed3 c2 = pow(1.0 - p.xyz, 100.0 / _LineScale);
    return clamp((c1.x + c2.x) + (c1.y + c2.y), 0.0, 1.0) * _Color;
}

ENDCG

Pass
{
    ColorMask 0
    ZWrite On
    ZTest LEqual

    Stencil 
    {
        Ref [_Mask]
        Comp NotEqual
    }

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag_occlusion
    #pragma target 5.0
    #pragma only_renderers d3d11
    ENDCG
}

Pass
{
    ZWrite Off
    ZTest LEqual
    Blend SrcAlpha OneMinusSrcAlpha 

    Stencil 
    {
        Ref [_Mask]
        Comp NotEqual
    }

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag_grid
    #pragma target 5.0
    #pragma only_renderers d3d11
    ENDCG
}

}

}