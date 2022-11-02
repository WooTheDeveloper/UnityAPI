 Shader "Instanced/DrawMeshInstancedIndirect_IndirectArgsTest" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color",color) = (1,1,1,1)
    }
    SubShader {

        Pass {

            Tags {"LightMode"="ForwardBase"}

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #pragma target 4.5

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            struct MatrixData{
                float4x4 o2w;
                float4x4 w2o;
            };
            
            StructuredBuffer<MatrixData> MatrixDataBuffer;
            float4 _Color;
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float3 origPositionWS : TEXCOORD1;
            };

            v2f vert (appdata_full v, uint instanceID : SV_InstanceID)
            {
                MatrixData data = MatrixDataBuffer[instanceID];
                
                float4x4 w2o = data.w2o;
                float4x4 o2w = data.o2w;
                
                v2f o;
                o.origPositionWS = float3(o2w[0][3],o2w[2][3],o2w[1][3]);
                float3 worldPosition = mul(o2w,v.vertex);
                o.pos = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
                o.uv_MainTex = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(_Color);
            }

            ENDCG
        }
    }
}