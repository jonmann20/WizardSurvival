Shader "RAIN/AspectRingShader"
{
    SubShader
    {
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest Off
		ZWrite Off
		Cull Front
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			float _colorAlpha;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return float4(_colorSolid.rgb, _colorAlpha);
			}
				
			ENDCG
		}

		Blend One Zero
		ZTest LEqual
		Cull Front
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
	
			float4 _colorSolid;
			
			struct vert_out
			{
				float4 position : POSITION;
			};
			
			vert_out vert(appdata_base v)
			{
				vert_out tOut;
				tOut.position = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return tOut;
			}
	
			float4 frag(vert_out f) : COLOR
			{
				return _colorSolid;
			}
				
			ENDCG
		}
    }
    FallBack "Diffuse"
}
