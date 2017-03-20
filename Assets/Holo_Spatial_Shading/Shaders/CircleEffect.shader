Shader "HoloLens/SpatialMapping/CircleEffect"
{

Properties
{
    _Color("Color", Color) = (1, 1, 1, 1)
    _Intensity("Intensity", Range(0, 10)) = 2
    _Height("Height", Range(0, 1)) = 0.1
    _DecayDist("Decay Distance", Float) = 2
    _LinesPerMeter("Lines per Meter", Float) = 25
    _Mask("Mask", Int) = 1
}

CGINCLUDE

#include "UnityCG.cginc"

struct v2f
{
    float4 vertex : SV_POSITION;
    fixed3 worldPos : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

fixed _LinesPerMeter;
fixed _Intensity;
fixed _Height;
fixed4 _Color;
fixed3 _Center;
fixed _Radius;
fixed _Width;
fixed _DecayDist;

v2f vert(appdata_base v)
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed3 worldIndex;
    modf(i.worldPos.xyz * _LinesPerMeter, worldIndex);
    fixed3 centerOfBoxel = worldIndex / _LinesPerMeter;
    fixed dist = length(_Center - centerOfBoxel);
    fixed diff = exp(-abs(_Radius - dist) / _Width);

    fixed3 rgb = diff * _Color.rgb;
    fixed a = (rgb.r + rgb.b + rgb.g) * _Color.a * exp(-dist / _DecayDist) * _Intensity;

    return fixed4(rgb, a);
}

ENDCG

SubShader
{

Tags 
{ 
    "RenderType"="Opaque" 
    "Queue"="Geometry-1"
}

UsePass "HoloLens/SpatialMapping/Occlusion/OCCLUSION"

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
    #pragma fragment frag
    #pragma target 5.0
    #pragma only_renderers d3d11
    ENDCG
}

}

}