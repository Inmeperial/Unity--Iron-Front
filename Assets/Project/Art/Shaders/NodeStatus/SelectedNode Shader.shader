// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SelectedNode Shader"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		[HDR]_ColorSelectedNode("ColorSelectedNode", Color) = (1,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorSelectedNode;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 color17 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 temp_output_16_0 = saturate( ( _ColorSelectedNode * ( tex2D( _TextureSample2, uv_TextureSample2 ).r * color17 ) ) );
			o.Emission = temp_output_16_0.rgb;
			o.Alpha = temp_output_16_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;802;938;1584.791;853.6332;2.245051;True;False
Node;AmplifyShaderEditor.SamplerNode;5;-1033.959,13.64192;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;fb3e67538a281444c9af25adfb70c4d2;fb3e67538a281444c9af25adfb70c4d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-1022.785,280.2313;Inherit;False;Constant;_Color2;Color 1;5;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-728.6213,159.4928;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-963.4203,-198.6396;Inherit;False;Property;_ColorSelectedNode;ColorSelectedNode;1;1;[HDR];Create;True;0;0;False;0;1,0,0,0;2,1.780392,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;1;371.22,-65.71007;Inherit;False;970.2416;488.4174;ClampWithTime;6;12;11;10;9;3;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-502.806,-23.43942;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;-276.1586,-44.2908;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;391.5,-5.954594;Inherit;False;Property;_TimeCicle;TimeCicle;4;1;[IntRange];Create;True;0;0;False;0;4;8;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;3;630.7778,91.15837;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;507.5909,291.1539;Inherit;False;Property;_MaxBright;MaxBright;3;0;Create;True;0;0;False;0;0.9;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;512.349,183.2028;Inherit;False;Property;_MinBright;MinBright;2;0;Create;True;0;0;False;0;0.2;0.178;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;1153.282,-385.0674;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;1386.19,-181.0742;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;11;767.4371,-4.2959;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;970.8701,147.3578;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SelectedNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;5;1
WireConnection;18;1;17;0
WireConnection;8;0;6;0
WireConnection;8;1;18;0
WireConnection;16;0;8;0
WireConnection;3;0;2;0
WireConnection;14;1;12;0
WireConnection;15;0;12;0
WireConnection;11;0;3;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;12;2;9;0
WireConnection;0;2;16;0
WireConnection;0;9;16;0
ASEEND*/
//CHKSM=FFCCBEC3C7BF97DF9C0C85BF812E840A8F2A697D