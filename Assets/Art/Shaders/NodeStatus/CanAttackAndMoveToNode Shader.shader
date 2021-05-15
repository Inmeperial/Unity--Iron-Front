// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CanAttackAndMoveToNode Shader"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_ColorAttack("ColorAttack", Color) = (1,0,0,0)
		_ColorMove("ColorMove", Color) = (0,0.9367321,1,0)
		_RedLines("RedLines", Range( 0.5 , 1)) = 0.7580875
		_InnerRedLines("InnerRedLines", Range( 0.5 , 1)) = 0.7580875
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

		uniform float4 _ColorAttack;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _RedLines;
		uniform float _InnerRedLines;
		uniform float4 _ColorMove;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 color78 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 temp_output_79_0 = ( tex2D( _TextureSample2, uv_TextureSample2 ).g * color78 );
			float4 temp_cast_0 = (( ( step( i.uv_texcoord.y , _RedLines ) * step( ( 1.0 - _RedLines ) , i.uv_texcoord.y ) ) + ( step( i.uv_texcoord.x , _RedLines ) * step( ( 1.0 - _RedLines ) , i.uv_texcoord.x ) ) )).xxxx;
			float4 temp_output_48_0 = ( temp_output_79_0 - temp_cast_0 );
			float4 temp_output_73_0 = ( _ColorAttack * saturate( ( ( step( i.uv_texcoord.y , _InnerRedLines ) * step( ( 1.0 - _InnerRedLines ) , i.uv_texcoord.y ) ) + ( step( i.uv_texcoord.x , _InnerRedLines ) * step( ( 1.0 - _InnerRedLines ) , i.uv_texcoord.x ) ) ) ) );
			float4 temp_cast_1 = (( ( step( i.uv_texcoord.y , _RedLines ) * step( ( 1.0 - _RedLines ) , i.uv_texcoord.y ) ) + ( step( i.uv_texcoord.x , _RedLines ) * step( ( 1.0 - _RedLines ) , i.uv_texcoord.x ) ) )).xxxx;
			o.Emission = saturate( ( ( saturate( ( ( _ColorAttack * temp_output_79_0 ) - saturate( temp_output_48_0 ) ) ) + temp_output_73_0 ) + ( temp_output_48_0 * _ColorMove ) ) ).rgb;
			o.Alpha = ( temp_output_79_0 + temp_output_73_0 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;802;938;2388.917;-34.32163;1.960842;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-2828.869,131.1344;Inherit;False;Property;_RedLines;RedLines;7;0;Create;True;0;0;False;0;0.7580875;0.6;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;40;-2396.402,323.6865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-2604.851,-90.52869;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;38;-2535.151,-229.8781;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-2743.6,-644.0941;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;44;-2210.072,-17.87779;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;-2149.659,329.8524;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1803.977,1319.889;Inherit;False;Property;_InnerRedLines;InnerRedLines;8;0;Create;True;0;0;False;0;0.7580875;0.52;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;42;-2332.729,-325.2831;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;41;-2348.821,-571.4423;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;62;-1371.511,1510.185;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-1718.708,542.4045;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-2189.388,-1006.004;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;cc9ae09d15e94874381f733d6bec688c;cc9ae09d15e94874381f733d6bec688c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;64;-1510.26,956.6203;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2031.965,-455.1039;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1945.201,39.04819;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-1579.959,1095.969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;78;-2007.965,-801.3628;Inherit;False;Constant;_Color1;Color 1;5;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;65;-1185.181,1168.62;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-1307.838,861.2153;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;68;-1323.93,615.0563;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1640.364,-301.5999;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;66;-1124.767,1516.35;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1635.494,-956.9387;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-1625.048,-1243.178;Inherit;False;Property;_ColorAttack;ColorAttack;1;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-920.3088,1225.546;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1007.073,731.3945;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;-1339.047,-345.9006;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-615.472,884.8984;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1298.991,-1165.53;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;49;-1132.91,-485.2711;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;59;-525.5179,634.0009;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;51;-931.2122,-701.8311;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;57;-681.4553,-819.027;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;18;-1115.999,28.31261;Inherit;False;Property;_ColorMove;ColorMove;2;0;Create;True;0;0;False;0;0,0.9367321,1,0;0,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-486.2845,171.6371;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-845.6617,-65.26339;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-550.1226,-746.6683;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-255.0578,-435.2729;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;6;432.1781,-73.86044;Inherit;False;970.2416;488.4174;ClampWithTime;6;12;11;10;9;8;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;9;828.3955,-12.44631;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-93.74773,-79.8814;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-238.5548,-41.61361;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;507.9723,-443.0444;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;0.1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;568.5494,283.0042;Inherit;False;Property;_MaxBright;MaxBright;5;0;Create;True;0;0;False;0;0.9;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;1000.306,-266.2998;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;52;-165.9584,142.651;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;452.4572,-14.105;Inherit;False;Property;_TimeCicle;TimeCicle;6;1;[IntRange];Create;True;0;0;False;0;4;4;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;1176.412,-280.3862;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;503.4734,-248.3425;Inherit;False;Property;_SpeedVert;Speed Vert ;3;0;Create;True;0;0;False;0;0.27;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;573.3065,175.0526;Inherit;False;Property;_MinBright;MinBright;4;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;1031.827,139.2075;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;798.4471,-243.57;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;1471.641,-296.2113;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;28;1092.498,-568.7263;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;691.7366,83.00793;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;20;825.6343,-549.208;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;96.28101,-67.65692;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CanAttackAndMoveToNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;35;0
WireConnection;38;0;35;0
WireConnection;44;0;39;1
WireConnection;44;1;35;0
WireConnection;43;0;40;0
WireConnection;43;1;39;1
WireConnection;42;0;38;0
WireConnection;42;1;37;2
WireConnection;41;0;37;2
WireConnection;41;1;35;0
WireConnection;62;0;60;0
WireConnection;64;0;60;0
WireConnection;45;0;41;0
WireConnection;45;1;42;0
WireConnection;46;0;44;0
WireConnection;46;1;43;0
WireConnection;65;0;61;1
WireConnection;65;1;60;0
WireConnection;67;0;64;0
WireConnection;67;1;63;2
WireConnection;68;0;63;2
WireConnection;68;1;60;0
WireConnection;47;0;45;0
WireConnection;47;1;46;0
WireConnection;66;0;62;0
WireConnection;66;1;61;1
WireConnection;79;0;4;2
WireConnection;79;1;78;0
WireConnection;70;0;65;0
WireConnection;70;1;66;0
WireConnection;69;0;68;0
WireConnection;69;1;67;0
WireConnection;48;0;79;0
WireConnection;48;1;47;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;17;0;5;0
WireConnection;17;1;79;0
WireConnection;49;0;48;0
WireConnection;59;0;71;0
WireConnection;51;0;17;0
WireConnection;51;1;49;0
WireConnection;57;0;51;0
WireConnection;73;0;5;0
WireConnection;73;1;59;0
WireConnection;54;0;48;0
WireConnection;54;1;18;0
WireConnection;58;0;57;0
WireConnection;58;1;73;0
WireConnection;55;0;58;0
WireConnection;55;1;54;0
WireConnection;9;0;8;0
WireConnection;15;0;55;0
WireConnection;74;0;79;0
WireConnection;74;1;73;0
WireConnection;21;1;22;0
WireConnection;23;1;21;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;12;2;11;0
WireConnection;22;0;25;0
WireConnection;24;0;23;0
WireConnection;28;0;20;0
WireConnection;8;0;7;0
WireConnection;20;3;27;0
WireConnection;0;2;15;0
WireConnection;0;9;74;0
ASEEND*/
//CHKSM=B42384C2C6A7C16F097D0992A62144F3DC8A0933