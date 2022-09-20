// jave.lin 2019.08.29
// 用于替换Cube, Sphere, Capsule的shader
Shader "Test/ReplacementShaderUnlitColor" {
    Properties {
    }
    CGINCLUDE
    #include "UnityCG.cginc"
    // SetReplacementShader的shader
    float4 vert (float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
    fixed4 frag_Red () : SV_Target { return fixed4(1,0,0,1); }
    fixed4 frag_Green () : SV_Target { return fixed4(0,1,0,1); }
    fixed4 frag_Blue () : SV_Target { return fixed4(0,0,1,1); }
    // RenderWithShader的shader
    //与vert函数一样void vert_renderWithShader(float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
    fixed4 frag_renderWithShader () : SV_Target { return fixed4(1,1,0,1); }
    ENDCG
    SubShader {
        Tags { "Color"="Red" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Red
            ENDCG
        }
    }
    SubShader {
        Tags { "Color"="Green" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Green
            ENDCG
        }
    }
    SubShader {
        Tags { "Color"="Blue" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_Blue
            ENDCG
        }
    }
    SubShader {
        Tags { "RenderWithShader"="Test" }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_renderWithShader
            ENDCG
        }
    }
    Fallback "Diffuse"
}

