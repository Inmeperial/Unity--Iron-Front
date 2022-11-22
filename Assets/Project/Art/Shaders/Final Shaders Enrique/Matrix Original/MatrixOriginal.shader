// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MatrixOriginal"
{
	Properties
	{
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D GL_textMask;
		uniform sampler2D GL_numbersText;
		uniform float4 GL_colorWord;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color13 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float2 _tillingVector = float2(1,0.2);
			float2 appendResult3 = (float2(0.0 , _Time.y));
			float2 uv_TexCoord11 = i.uv_texcoord * _tillingVector + appendResult3;
			float4 tex2DNode12 = tex2D( GL_textMask, uv_TexCoord11 );
			float3 appendResult14 = (float3(tex2DNode12.r , tex2DNode12.g , tex2DNode12.b));
			float2 uv_TexCoord10 = i.uv_texcoord * _tillingVector;
			float temp_output_8_0 = ( tex2DNode12.a * tex2D( GL_numbersText, uv_TexCoord10 ).a );
			o.Albedo = ( ( ( color13 * float4( saturate( appendResult14 ) , 0.0 ) ) * temp_output_8_0 ) + ( temp_output_8_0 * GL_colorWord ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-809;32;656;936;1008.017;386.927;1.091241;True;False
Node;AmplifyShaderEditor.CommentaryNode;2;-2888.774,-250.4101;Inherit;False;2394.605;906.7421;;17;4;7;17;5;16;8;13;14;9;10;12;11;3;18;6;15;24;Original;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2838.774,152.8886;Inherit;False;Constant;_timeSpeed;timeSpeed;3;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2484.731,156.4525;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;-2298.898,142.8316;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;18;-2462.342,312.8695;Inherit;False;Constant;_tillingVector;tillingVector;8;0;Create;True;0;0;0;False;0;False;1,0.2;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2124.691,104.2635;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-1806.622,79.15713;Inherit;True;Global;GL_textMask;GL_textMask;1;0;Create;True;0;0;0;False;0;False;-1;c685fcb362349e449aa1ca3f5acf6d68;c685fcb362349e449aa1ca3f5acf6d68;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2079.506,366.2744;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1456.429,47.69656;Inherit;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;13;-1410.412,-200.41;Inherit;False;Constant;_colorFirstWord;colorFirstWord;9;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;24;-1259.656,19.86163;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;9;-1799.787,338.1605;Inherit;True;Global;GL_numbersText;GL_numbersText;0;0;Create;True;0;0;0;False;0;False;-1;122b302712b8fc9438eda93cc55d4997;122b302712b8fc9438eda93cc55d4997;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1124.423,-59.43401;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1271.125,347.6766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1121.184,452.8436;Inherit;False;Global;GL_colorWord;GL_colorWord;0;0;Create;True;0;0;0;False;0;False;0.03083839,0.9339623,0.1677198,0;0.05980776,0.6037736,0.1722864,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-906.0934,327.4945;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-857.7715,90.00645;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-630.35,113.2905;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-264.051,67.06967;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;MatrixOriginal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;15;0
WireConnection;3;1;6;0
WireConnection;11;0;18;0
WireConnection;11;1;3;0
WireConnection;12;1;11;0
WireConnection;10;0;18;0
WireConnection;14;0;12;1
WireConnection;14;1;12;2
WireConnection;14;2;12;3
WireConnection;24;0;14;0
WireConnection;9;1;10;0
WireConnection;16;0;13;0
WireConnection;16;1;24;0
WireConnection;8;0;12;4
WireConnection;8;1;9;4
WireConnection;7;0;8;0
WireConnection;7;1;5;0
WireConnection;17;0;16;0
WireConnection;17;1;8;0
WireConnection;4;0;17;0
WireConnection;4;1;7;0
WireConnection;0;0;4;0
ASEEND*/
//CHKSM=11C64384F92E3C8EEAA856127BAE2B3DE2FA6407