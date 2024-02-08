// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlinePrePass"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"  
			fixed4 _OutLineColor;

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutLineColor;
			}


			#pragma vertex vert  
			#pragma fragment frag  
			ENDCG
		}
	}
}
