// Upgrade NOTE: replaced '_WorldSpaceCameraPos.w' with 'unity_Scale.w'

Shader "Reflective/Bumped Specular Refractive" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Base Texture", 2D) = "white" {}
		_HeightTex ("Bump Texture", 2D) = "bump" {}
		_FoamTex ("Foam Texture", 2D) = "white" {}
		_CubeTex ("_CubeTex", CUBE) = "white" {}
		
		_Refractivity ("_Refractivity", Range (0.1, 100.0)) = 1.0
		
		_Ambient ("_Ambient", Range (0.0, 1.0)) = 0.8
		
		_Shininess ("_Shininess", Range (0.1, 60.0)) = 1.0
		_SpecColor ("Spec Color", Color) = (0.5,0.5,0.5,0.5)
		
		_Displacement ("_Displacement", Range (0.0, 2.0)) = 1.0
		_DisplacementTiling ("_DisplacementTiling", Range (0.1, 4.0)) = 1.0
		
		_InvFade ("_InvFade", Range (0.05, 5.0)) = 1.0
		_InvFadeFoam ("_InvFadeFoam", Range (0.05, 5.0)) = 1.0
		
		_FresnelPower ("_FresnelPower", Range (0.1, 10.0)) = 2.0
		_Reflectivity ("_Reflectivity", Range (0.0, 1.0)) = 0.8
		
		_ColorTextureOverlay ("_ColorTextureOverlay", Range (0.0, 1.0)) = 0.75
		
		_WorldLightDir("_WorldLightDir", Vector) = (0,0,0,1)
		
		_Speed("_Speed", Range (0.0, 10.0)) = 0.8
		
	}
	
	Category 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGB
		Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		
		SubShader 
		{			
			GrabPass { }
			Pass 
			{
				CGPROGRAM

				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragmentoption ARB_fog_exp2
	
				#include "UnityCG.cginc"
	
				sampler2D _MainTex;
				sampler2D _HeightTex;
				sampler2D _FoamTex;
				samplerCUBE _CubeTex;
				
				sampler2D _CameraDepthTexture;
				sampler2D _GrabTexture;
				
				float4 _TintColor;
				float4 _SpecColor;
				
				float4 _GrabTexture_TexelSize;
				
				float _Displacement;
				float _DisplacementTiling;
							
				float _InvFade;
				float _InvFadeFoam;
				
				float _FresnelPower;
				
				float _Shininess;
				float _Ambient;
				
				float _Refractivity;
				float _Reflectivity;
				
				float _ColorTextureOverlay;
				
				float4 _WorldLightDir;
				
				float _Speed;
				
	
				struct v2f {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float4 projPos : TEXCOORD2;
					float3 viewDirWorld : TEXCOORD3;
					float3 TtoW0 : TEXCOORD4;
					float3 TtoW1 : TEXCOORD5;
					float3 TtoW2 : TEXCOORD6;
					float2 texcoord2 : TEXCOORD7;
				};
				
				float4 _MainTex_ST;
				float4 _HeightTex_ST;
				float4 _FoamTex_ST;
	
				v2f vert (appdata_full v)
				{
					v2f o;
					
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.texcoord1 = TRANSFORM_TEX(v.texcoord1,_FoamTex);			
					o.texcoord2 = TRANSFORM_TEX(v.texcoord,_HeightTex);
					
					o.color = half4(v.color.rgb,1.0); //v.color;
					
					o.projPos = ComputeScreenPos (mul(UNITY_MATRIX_MVP, v.vertex));				
					
					v.vertex.xyz += v.normal.xyz * sin((length(v.vertex.zy + v.color.rgb-0.5) + _Time.w * _Speed )*_DisplacementTiling) * _Displacement * 1.5 * v.color.a;
					
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);		
					o.projPos = ComputeScreenPos (mul(UNITY_MATRIX_MVP, v.vertex));
					
					o.viewDirWorld = -WorldSpaceViewDir(v.vertex);
					
					TANGENT_SPACE_ROTATION;
					o.TtoW0 = mul(rotation, _Object2World[0].xyz * unity_Scale.w);
					o.TtoW1 = mul(rotation, _Object2World[1].xyz * unity_Scale.w);
					o.TtoW2 = mul(rotation, _Object2World[2].xyz * unity_Scale.w);				
					
					return o;
				}
	
				half4 frag (v2f i) : COLOR
				{
					i.color.rgb = half3(1,1,1);
					
					// calc fade factor
					float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
					float partZ = i.projPos.z;
					half depthDiff = (sceneZ-partZ);
					half2 fade2 = saturate (half2(_InvFade, _InvFadeFoam) * depthDiff);
					i.color.a *= fade2.x;
					
					// calculate world normal
					half3 normal = ((UnpackNormal(tex2D(_HeightTex,i.texcoord))));
					half3 worldNormal;
					worldNormal.x = dot(i.TtoW0, normal.xyz);
					worldNormal.y = dot(i.TtoW1, normal.xyz);
					worldNormal.z = dot(i.TtoW2, normal.xyz);		

					worldNormal = normalize(worldNormal);
					i.viewDirWorld = normalize(i.viewDirWorld);
					
					// foam
					float4 foam = tex2D(_FoamTex, i.texcoord1);
					float4 color = tex2D(_MainTex, i.texcoord);
					color = lerp(half4(1,1,1,1),color,_ColorTextureOverlay);					
					
					// REFRACTION
					float2 offset = normal * _GrabTexture_TexelSize.xy;
					float4 refractionUv = i.projPos;
					refractionUv.xy += offset * i.projPos.z * _Refractivity * fade2.y;	
		
					float3 refrColor = tex2Dproj(_GrabTexture, refractionUv);
					
					// REFLECTION
					float3 reflectVector = normalize(reflect(i.viewDirWorld,worldNormal));
					half4 reflColor = texCUBE(_CubeTex, reflectVector);
					reflColor = lerp(0.75,reflColor, saturate(fade2.y+(_Reflectivity-1.0)));
					
					// FRESNEL CALCS
					float fcbias = 0.20373;
					float facing = 0.8 - max(dot(-i.viewDirWorld,worldNormal), 0.0);
					float refl2Refr = max(fcbias + (1.0-fcbias) * pow(facing, _FresnelPower), 0);				
					
					color.rgb *= (lerp(refrColor,reflColor, refl2Refr)); 
	
					color = lerp(foam, color, fade2.y);
					
					// light
					color.rgb = color.rgb * max(_Ambient, dot(_WorldLightDir.xyz,worldNormal));
					color.rgb += _SpecColor.rgb * max(0.0, pow(dot(_WorldLightDir.xyz,reflectVector),_Shininess));
												
					return i.color * _TintColor * color;
				}
				ENDCG 
			} // pass
		} // subshader	

		SubShader 
		{			
			// NO GRAB PASS () HERE

			Pass 
			{
				CGPROGRAM

				//#pragma target 3.0
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragmentoption ARB_fog_exp2
	
				#include "UnityCG.cginc"
	
				sampler2D _MainTex;
				sampler2D _HeightTex;
				samplerCUBE _CubeTex;
				
				float4 _TintColor;
				float4 _SpecColor;
								
				float _Displacement;
				float _DisplacementTiling;
							
				float _InvFade;
				float _InvFadeFoam;
				
				float _FresnelPower;
				
				float _Shininess;
				float _Ambient;
				
				float _Refractivity;
				float _Reflectivity;
				
				float _ColorTextureOverlay;
				
				float4 _WorldLightDir;
				
				float _Speed;
				
	
				struct v2f {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float2 texcoord1 : TEXCOORD1;
					float4 projPos : TEXCOORD2;
					float3 viewDirWorld : TEXCOORD3;
					float3 TtoW0 : TEXCOORD4;
					float3 TtoW1 : TEXCOORD5;
					float3 TtoW2 : TEXCOORD6;
					float2 texcoord2 : TEXCOORD7;
				};
				
				float4 _MainTex_ST;
				float4 _HeightTex_ST;
	
				v2f vert (appdata_full v)
				{
					v2f o;
					
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					//o.texcoord1 = TRANSFORM_TEX(v.texcoord1,_FoamTex);			
					o.texcoord2 = TRANSFORM_TEX(v.texcoord,_HeightTex);
					
					o.color = half4(v.color.rgb,1.0); //v.color;
					
					o.projPos = ComputeScreenPos (mul(UNITY_MATRIX_MVP, v.vertex));				
					
					v.vertex.xyz += v.normal.xyz * sin((length(v.vertex.zy + v.color.rgb-0.5) + _Time.w * _Speed )*_DisplacementTiling) * _Displacement * 1.5 * v.color.a;
					
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);		
					o.projPos = ComputeScreenPos (mul(UNITY_MATRIX_MVP, v.vertex));
					
					o.viewDirWorld = -WorldSpaceViewDir(v.vertex);
					
					TANGENT_SPACE_ROTATION;
					o.TtoW0 = mul(rotation, _Object2World[0].xyz * unity_Scale.w);
					o.TtoW1 = mul(rotation, _Object2World[1].xyz * unity_Scale.w);
					o.TtoW2 = mul(rotation, _Object2World[2].xyz * unity_Scale.w);				
					
					return o;
				}
	
				half4 frag (v2f i) : COLOR
				{
					i.color.rgb = half3(1,1,1);
					
					// calculate world normal
					half3 normal = ((UnpackNormal(tex2D(_HeightTex,i.texcoord))));
					half3 worldNormal;

					worldNormal.x = dot(i.TtoW0, normal.xyz);
					worldNormal.y = dot(i.TtoW1, normal.xyz);
					worldNormal.z = dot(i.TtoW2, normal.xyz);		

					worldNormal = normalize(worldNormal);
					i.viewDirWorld = normalize(i.viewDirWorld);
					
					// color
					float4 color = tex2D(_MainTex, i.texcoord);
					color = lerp(half4(1,1,1,1),color,_ColorTextureOverlay);					
					
					// REFLECTION
					float3 reflectVector = normalize(reflect(i.viewDirWorld,worldNormal));
					half4 reflColor = texCUBE(_CubeTex, reflectVector);
					reflColor = lerp(0.75,reflColor, saturate((_Reflectivity-1.0)));
					
					// FRESNEL CALCS
					float fcbias = 0.20373;
					float facing = 0.8 - max(dot(-i.viewDirWorld,worldNormal), 0.0);
					float refl2Refr = max(fcbias + (1.0-fcbias) * pow(facing, _FresnelPower), 0);				
					
					color.rgba *= (lerp(half4(1,1,1, 0.0), half4(reflColor.rgb,1.0), refl2Refr)); 
					 
					// light
					color.rgb = color.rgb * max(_Ambient, dot(_WorldLightDir.xyz,worldNormal));
					color.rgb += _SpecColor.rgb * max(0.0, pow(dot(_WorldLightDir.xyz,reflectVector),_Shininess));
												
					return i.color * _TintColor * color;
				}
				ENDCG 
			} // pass
		} // subshader



	} // category
} // shader
