
#include "UnityCG.cginc"
// vert、frag是未Replace前使用的shader
float4 vert (float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
fixed4 frag () : SV_Target { return fixed4(1,1,1,1); }
