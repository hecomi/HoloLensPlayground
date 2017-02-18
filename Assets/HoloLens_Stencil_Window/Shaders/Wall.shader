Shader "HoloLens/Wall"
{

Properties
{
    _Mask("Mask", Int) = 1
}

SubShader
{

Tags 
{ 
    "RenderType" = "Opaque" 
    "Queue" = "Geometry-1"
}

CGINCLUDE

#include "UnityCG.cginc"

struct v2f
{
    float4 vertex : SV_POSITION;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata_base v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.vertex = UnityObjectToClipPos(v.vertex);
    return o;
}

fixed4 frag(v2f i) : COLOR
{
    return 0;
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
    #pragma fragment frag
    #pragma target 5.0
    #pragma only_renderers d3d11
    ENDCG
}

}

}