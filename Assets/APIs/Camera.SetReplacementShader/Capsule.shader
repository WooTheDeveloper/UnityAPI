// Capsule.shader
// jave.lin 2019.08.29
Shader "Test/Capsule" {
    CGINCLUDE
    #include "BeforeRenderReplaceCOM.cginc"
    ENDCG
    SubShader {
        Tags { "RenderType"="Opaque" "Color"="Blue" "RenderWithShader"="Test" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback "Diffuse"
}

