Shader "HoloLens/SpatialMapping/Occlusion"
{

Properties
{
    _Mask("Mask", Int) = 1
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
    UNITY_SETUP_INSTANCE_ID(v);
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.vertex = UnityObjectToClipPos(v.vertex);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return 0;
}

ENDCG

SubShader
{

Tags 
{ 
    "RenderType"="Opaque" 
    "Queue"="Geometry-1"
}

Pass
{
    Name "OCCLUSION"

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