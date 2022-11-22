// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MatrixEffect"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _mask1;
		uniform float T_X_1;
		uniform float T_Y_1;
		uniform float S_1;
		uniform sampler2D _numbers1;
		uniform sampler2D _mask2;
		uniform float T_X_2;
		uniform float T_Y_2;
		uniform float S_2;
		uniform sampler2D _numbers2;
		uniform float T_X_4;
		uniform float T_Y_4;
		uniform float S_4;
		uniform float T_X_3;
		uniform float T_Y_3;
		uniform float S_3;
		uniform float _Cutoff = 0.2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color9_g1 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float clampResult23_g1 = clamp( T_X_1 , 0.0 , 5.0 );
			float clampResult24_g1 = clamp( T_Y_1 , 0.0 , 5.0 );
			float2 appendResult20_g1 = (float2(clampResult23_g1 , clampResult24_g1));
			float clampResult19_g1 = clamp( S_1 , 0.0 , 5.0 );
			float mulTime2_g1 = _Time.y * clampResult19_g1;
			float2 appendResult4_g1 = (float2(0.0 , mulTime2_g1));
			float2 uv_TexCoord5_g1 = i.uv_texcoord * appendResult20_g1 + appendResult4_g1;
			float4 tex2DNode44_g1 = tex2D( _mask1, uv_TexCoord5_g1 );
			float3 appendResult10_g1 = (float3(tex2DNode44_g1.r , tex2DNode44_g1.g , tex2DNode44_g1.b));
			float2 uv_TexCoord6_g1 = i.uv_texcoord * appendResult20_g1;
			float temp_output_12_0_g1 = ( tex2DNode44_g1.a * tex2D( _numbers1, uv_TexCoord6_g1 ).a );
			float4 color13_g1 = IsGammaSpace() ? float4(0.008265947,0.2169811,0,0) : float4(0.0006397792,0.03864443,0,0);
			float clampResult54_g1 = clamp( T_X_2 , 0.0 , 5.0 );
			float clampResult62_g1 = clamp( T_Y_2 , 0.0 , 5.0 );
			float2 appendResult63_g1 = (float2(clampResult54_g1 , clampResult62_g1));
			float clampResult53_g1 = clamp( S_2 , 0.0 , 5.0 );
			float mulTime56_g1 = _Time.y * clampResult53_g1;
			float2 appendResult49_g1 = (float2(0.0 , mulTime56_g1));
			float2 uv_TexCoord51_g1 = i.uv_texcoord * appendResult63_g1 + appendResult49_g1;
			float4 tex2DNode57_g1 = tex2D( _mask2, uv_TexCoord51_g1 );
			float3 appendResult65_g1 = (float3(tex2DNode57_g1.r , tex2DNode57_g1.g , tex2DNode57_g1.b));
			float2 uv_TexCoord59_g1 = i.uv_texcoord * appendResult63_g1;
			float temp_output_58_0_g1 = ( tex2DNode57_g1.a * tex2D( _numbers2, uv_TexCoord59_g1 ).a );
			float4 color9_g2 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float clampResult23_g2 = clamp( T_X_4 , 0.0 , 5.0 );
			float clampResult24_g2 = clamp( T_Y_4 , 0.0 , 5.0 );
			float2 appendResult20_g2 = (float2(clampResult23_g2 , clampResult24_g2));
			float clampResult19_g2 = clamp( S_4 , 0.0 , 5.0 );
			float mulTime2_g2 = _Time.y * clampResult19_g2;
			float2 appendResult4_g2 = (float2(0.0 , mulTime2_g2));
			float2 uv_TexCoord5_g2 = i.uv_texcoord * appendResult20_g2 + appendResult4_g2;
			float4 tex2DNode44_g2 = tex2D( _mask1, uv_TexCoord5_g2 );
			float3 appendResult10_g2 = (float3(tex2DNode44_g2.r , tex2DNode44_g2.g , tex2DNode44_g2.b));
			float2 uv_TexCoord6_g2 = i.uv_texcoord * appendResult20_g2;
			float temp_output_12_0_g2 = ( tex2DNode44_g2.a * tex2D( _numbers1, uv_TexCoord6_g2 ).a );
			float4 color13_g2 = IsGammaSpace() ? float4(0.008265947,0.2169811,0,0) : float4(0.0006397792,0.03864443,0,0);
			float clampResult54_g2 = clamp( T_X_3 , 0.0 , 5.0 );
			float clampResult62_g2 = clamp( T_Y_3 , 0.0 , 5.0 );
			float2 appendResult63_g2 = (float2(clampResult54_g2 , clampResult62_g2));
			float clampResult53_g2 = clamp( S_3 , 0.0 , 5.0 );
			float mulTime56_g2 = _Time.y * clampResult53_g2;
			float2 appendResult49_g2 = (float2(0.0 , mulTime56_g2));
			float2 uv_TexCoord51_g2 = i.uv_texcoord * appendResult63_g2 + appendResult49_g2;
			float4 tex2DNode57_g2 = tex2D( _mask2, uv_TexCoord51_g2 );
			float3 appendResult65_g2 = (float3(tex2DNode57_g2.r , tex2DNode57_g2.g , tex2DNode57_g2.b));
			float2 uv_TexCoord59_g2 = i.uv_texcoord * appendResult63_g2;
			float temp_output_58_0_g2 = ( tex2DNode57_g2.a * tex2D( _numbers2, uv_TexCoord59_g2 ).a );
			float4 temp_output_65_0 = ( ( ( ( ( color9_g1 * float4( appendResult10_g1 , 0.0 ) ) * temp_output_12_0_g1 ) + ( temp_output_12_0_g1 * color13_g1 ) ) + ( ( ( color9_g1 * float4( appendResult65_g1 , 0.0 ) ) * temp_output_58_0_g1 ) + ( temp_output_58_0_g1 * color13_g1 ) ) ) + ( ( ( ( color9_g2 * float4( appendResult10_g2 , 0.0 ) ) * temp_output_12_0_g2 ) + ( temp_output_12_0_g2 * color13_g2 ) ) + ( ( ( color9_g2 * float4( appendResult65_g2 , 0.0 ) ) * temp_output_58_0_g2 ) + ( temp_output_58_0_g2 * color13_g2 ) ) ) );
			o.Emission = temp_output_65_0.rgb;
			o.Alpha = 1;
			clip( temp_output_65_0.r - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-1692;39;1264;936;3076.676;2138.976;2.455037;False;False
Node;AmplifyShaderEditor.RangedFloatNode;63;-1901.963,-905.1879;Inherit;False;Global;T_Y_3;T_Y_3;14;0;Create;True;0;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-1905.429,-779.7222;Inherit;False;Global;T_X_3;T_X_3;10;0;Create;True;0;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1966.178,-623.3835;Inherit;False;Global;S_3;S_3;10;0;Create;True;0;0;0;False;0;False;0;0.2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2019.175,-1282.012;Inherit;False;Global;S_1;S_1;10;0;Create;True;0;0;0;False;0;False;0;0.4;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1907.353,-410.0755;Inherit;False;Global;T_X_4;T_X_4;10;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1987.465,-1185.642;Inherit;False;Global;T_Y_2;T_Y_2;11;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2008.662,-1385.507;Inherit;False;Global;T_X_1;T_X_1;10;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-1907.353,-317.3584;Inherit;False;Global;S_4;S_4;12;0;Create;True;0;0;0;False;0;False;0;0.1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1883.923,-507.0646;Inherit;False;Global;T_Y_4;T_Y_4;11;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-1984.436,-1104.889;Inherit;False;Global;T_X_2;T_X_2;10;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1977.371,-1022.116;Inherit;False;Global;S_2;S_2;12;0;Create;True;0;0;0;False;0;False;0;0.3;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2018.757,-1474.336;Inherit;False;Global;T_Y_1;T_Y_1;11;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;89;-1627.56,-1387.528;Inherit;False;MatrixFunctionShader;1;;1;5864b4a10c5c77b48b56b1b395931df7;0;6;22;FLOAT;0;False;61;FLOAT;0;False;21;FLOAT;0;False;52;FLOAT;0;False;17;FLOAT;0;False;66;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;90;-1530.419,-619.7821;Inherit;False;MatrixFunctionShader;1;;2;5864b4a10c5c77b48b56b1b395931df7;0;6;22;FLOAT;0;False;61;FLOAT;0;False;21;FLOAT;0;False;52;FLOAT;0;False;17;FLOAT;0;False;66;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1179.791,-893.5413;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-775.0602,-873.549;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;MatrixEffect;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.2;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;89;22;53;0
WireConnection;89;61;57;0
WireConnection;89;21;54;0
WireConnection;89;52;56;0
WireConnection;89;17;55;0
WireConnection;89;66;58;0
WireConnection;90;22;76;0
WireConnection;90;61;63;0
WireConnection;90;21;77;0
WireConnection;90;52;64;0
WireConnection;90;17;78;0
WireConnection;90;66;62;0
WireConnection;65;0;89;0
WireConnection;65;1;90;0
WireConnection;0;2;65;0
WireConnection;0;10;65;0
ASEEND*/
//CHKSM=86E0E903549338C6DB220B37BA498B9EA613F44B