Shader "Oculus/Unlit Transparent Color" {
Properties {
    _Color ("Main Color", Color) = (0,0,0,0)
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
    Fog {Mode Off}

    ZTest Always
    Blend SrcAlpha OneMinusSrcAlpha
    Color [_Color]

    Pass {}
}
}
