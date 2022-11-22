// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TestShader"
{
	Properties
	{
		_TextureAlbedo("TextureAlbedo", 2D) = "white" {}
		_MaskAlbedo("MaskAlbedo", 2D) = "white" {}
		_ColorAlbedo("ColorAlbedo", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorAlbedo;
		uniform sampler2D _TextureAlbedo;
		uniform float4 _TextureAlbedo_ST;
		uniform sampler2D _MaskAlbedo;
		uniform float4 _MaskAlbedo_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureAlbedo = i.uv_texcoord * _TextureAlbedo_ST.xy + _TextureAlbedo_ST.zw;
			float2 uv_MaskAlbedo = i.uv_texcoord * _MaskAlbedo_ST.xy + _MaskAlbedo_ST.zw;
			float4 temp_output_3_0 = ( tex2D( _TextureAlbedo, uv_TextureAlbedo ) - tex2D( _MaskAlbedo, uv_MaskAlbedo ) );
			o.Albedo = ( _ColorAlbedo * ( 1.0 - temp_output_3_0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;589;938;1025.731;701.2856;2.095569;False;False
Node;AmplifyShaderEditor.SamplerNode;1;-1590.547,-45.8025;Inherit;True;Property;_TextureAlbedo;TextureAlbedo;0;0;Create;True;0;0;False;0;-1;13e29d1fd4647e641b4ed0970b17cdb5;13e29d1fd4647e641b4ed0970b17cdb5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1576.201,345.3237;Inherit;True;Property;_MaskAlbedo;MaskAlbedo;1;0;Create;True;0;0;False;0;-1;ec5eaadec970eca499296de8d7d148b1;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;3;-1124.393,227.8703;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-904.6479,-311.8556;Inherit;False;Property;_ColorAlbedo;ColorAlbedo;2;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;4;-795.0154,40.21576;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;8;-751.359,316.6215;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-418.9844,-24.74796;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;5;-330.2596,-229.8276;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-335.5536,234.4899;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TestShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;4;0;3;0
WireConnection;8;0;3;0
WireConnection;7;0;6;0
WireConnection;7;1;4;0
WireConnection;9;1;8;0
WireConnection;0;0;7;0
ASEEND*/
//CHKSM=BCC325ABF788B94007D694CE81AEFD71D1731B8F