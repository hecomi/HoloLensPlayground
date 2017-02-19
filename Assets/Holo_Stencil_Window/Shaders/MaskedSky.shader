Shader "HoloLens/MaskedSky"
{

Properties
{
    _Mask("Mask", Int) = 1
    _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
    [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
    [NoScaleOffset] _Cube ("Cubemap (HDR)", Cube) = "grey" {}
}

SubShader
{

Tags 
{ 
    "RenderType" = "Opaque" 
    "Queue" = "Geometry-1"
    "PreviewType" = "Skybox"
}

CGINCLUDE

#include "UnityCG.cginc"

samplerCUBE _Cube;
half4 _Cube_HDR;
half4 _Tint;
half _Exposure;

struct v2f 
{
    float4 vertex : SV_POSITION;
    float3 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert(appdata_base v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.texcoord = v.vertex.xyz;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    half4 tex = texCUBE(_Cube, i.texcoord);
    half3 c = DecodeHDR(tex, _Cube_HDR);
    c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
    c *= _Exposure;
    return half4(c, 1);
}

ENDCG

Pass
{ 
    Cull Front
    ZWrite On

    Stencil 
    {
        Ref [_Mask]
        Comp Equal
    }

    CGPROGRAM
    #pragma fragmentoption ARB_precision_hint_fastest
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 5.0
    #pragma only_renderers d3d11
    ENDCG
}

}

}