// Cube.shader
// jave.lin 2019.08.29
Shader "Test/Cube" {
    CGINCLUDE
    #include "BeforeRenderReplaceCOM.cginc"
    ENDCG
    SubShader {
        Tags { "RenderType"="Opaque" "Color"="Red" "RenderWithShader"="Test" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback "Diffuse"
}

