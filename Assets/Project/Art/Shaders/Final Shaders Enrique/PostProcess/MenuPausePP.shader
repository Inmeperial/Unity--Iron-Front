// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MenuPausePP"
{
	Properties
	{
		_lerpPower("lerpPower", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Cull Off
		ZWrite Off
		ZTest Always
		
		Pass
		{
			CGPROGRAM

			

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

		
			struct ASEAttributesDefault
			{
				float3 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
			};

			struct ASEVaryingsDefault
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _lerpPower;


			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV (v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex (o.texcoord, 1.0);

				v.texcoord = o.texcoordStereo;
				float4 ase_ppsScreenPosVertexNorm = float4(o.texcoordStereo,0,1);

				

				return o;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.texcoordStereo,0,1);

				float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode22 = tex2D( _MainTex, uv_MainTex );
				float4 ditherCustomScreenPos25 = tex2DNode22;
				float2 clipScreen25 = ditherCustomScreenPos25.xy * _ScreenParams.xy;
				float dither25 = Dither4x4Bayer( fmod(clipScreen25.x, 4), fmod(clipScreen25.y, 4) );
				dither25 = step( dither25, 0.1 );
				float grayscale24 = Luminance(tex2DNode22.rgb);
				float4 temp_cast_2 = (( dither25 + grayscale24 )).xxxx;
				float clampResult34 = clamp( _lerpPower , 0.0 , 1.0 );
				float4 lerpResult27 = lerp( tex2DNode22 , temp_cast_2 , (( clampResult34 > 0.0 ) ? 0.7 :  0.0 ));
				

				float4 color = lerpResult27;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
-1920;73;587;936;474.392;738.2944;1;False;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;20;-548.677,-197.8693;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-215.0999,-403.0856;Inherit;False;Constant;_ditherPower;ditherPower;1;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-343.9387,-203.4964;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-385.9807,112.8902;Inherit;False;Property;_lerpPower;lerpPower;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;24;38.72717,-85.76666;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;25;35.67929,-432.5305;Inherit;False;0;True;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-35.0863,364.5663;Inherit;False;Constant;_lerpTotal_0;lerpTotal_0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;34;-69.11698,134.6253;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-43.08631,282.9168;Inherit;False;Constant;_lerpTotal_1;lerpTotal_1;1;0;Create;True;0;0;False;0;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;266.6201,-143.2443;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;35;219.2714,202.0709;Inherit;False;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;13;942.5324,64.21539;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;27;490.178,-188.7306;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;6;1201.135,-318.8378;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DitheringNode;9;1767.849,-235.9298;Inherit;False;0;True;3;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;2346.777,-311.2892;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareGreaterEqual;15;1511.791,-93.02832;Inherit;False;4;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;8;1758.295,-383.1624;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;14;1170.315,-36.13333;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;2016.432,-264.2127;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;1405.874,-324.4648;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;720.5314,-193.9815;Float;False;True;-1;2;ASEMaterialInspector;0;2;MenuPausePP;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;22;0;20;0
WireConnection;24;0;22;0
WireConnection;25;0;28;0
WireConnection;25;2;22;0
WireConnection;34;0;33;0
WireConnection;26;0;25;0
WireConnection;26;1;24;0
WireConnection;35;0;34;0
WireConnection;35;2;31;0
WireConnection;35;3;30;0
WireConnection;27;0;22;0
WireConnection;27;1;26;0
WireConnection;27;2;35;0
WireConnection;9;0;15;0
WireConnection;9;2;7;0
WireConnection;11;0;7;0
WireConnection;11;1;10;0
WireConnection;11;2;13;4
WireConnection;15;0;13;4
WireConnection;15;2;14;0
WireConnection;8;0;7;0
WireConnection;14;0;13;4
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;7;0;6;0
WireConnection;0;0;27;0
ASEEND*/
//CHKSM=FB53BE632C0DB19E8B54D392021563AFB0117F67