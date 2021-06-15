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
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
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
		uniform float _Opacity;

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
			float4 EmissionVar81 = saturate( ( ( saturate( ( ( _ColorAttack * temp_output_79_0 ) - saturate( temp_output_48_0 ) ) ) + temp_output_73_0 ) + ( temp_output_48_0 * _ColorMove ) ) );
			o.Emission = EmissionVar81.rgb;
			float4 OpacityVar80 = ( temp_output_79_0 + saturate( ( temp_output_73_0 - temp_output_79_0 ) ) );
			o.Alpha = ( OpacityVar80 * _Opacity ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
-1440;0;1440;849;1094.947;250.6256;1.196399;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-3199.66,-2487.111;Inherit;False;Property;_RedLines;RedLines;6;0;Create;True;0;0;False;0;0.7580875;0.52;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;40;-2767.193,-2294.558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;-2905.942,-2848.123;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-2174.768,-1298.356;Inherit;False;Property;_InnerRedLines;InnerRedLines;7;0;Create;True;0;0;False;0;0.7580875;0.52;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-3114.391,-3262.339;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-2975.642,-2708.774;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;44;-2580.863,-2636.123;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;64;-1881.052,-1661.625;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-1752.399,-1294.951;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-2089.5,-2075.841;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;62;-1655.385,-1094.688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;42;-2703.52,-2943.528;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;-2520.45,-2288.393;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;41;-2719.612,-3189.687;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2561.179,-3709.249;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;cc9ae09d15e94874381f733d6bec688c;8b180461484ec4a4896ef9c4b65b14b5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2315.992,-2579.197;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;68;-1694.722,-2003.189;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;65;-1469.055,-1436.253;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-1678.63,-1757.03;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;78;-2378.756,-3419.608;Inherit;False;Constant;_Color1;Color 1;5;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2402.756,-3073.349;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;66;-1408.641,-1088.523;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-1204.182,-1379.327;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2006.286,-3575.184;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2011.156,-2919.845;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1377.865,-1886.85;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;-1709.839,-2964.146;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-986.2642,-1733.346;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1995.84,-3861.423;Inherit;False;Property;_ColorAttack;ColorAttack;1;0;Create;True;0;0;False;0;1,0,0,0;0.8490566,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1669.783,-3783.775;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;59;-896.3098,-1984.244;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;49;-1503.702,-3103.516;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;51;-1302.004,-3320.076;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-857.0764,-2446.608;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;57;-1052.247,-3437.272;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;18;-1486.791,-2589.932;Inherit;False;Property;_ColorMove;ColorMove;2;0;Create;True;0;0;False;0;0,0.9367321,1,0;0.1906372,0.6274511,0.6274511,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;100;-480.2552,-2366.413;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;101;-214.5452,-2374.465;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1216.454,-2683.509;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-920.9146,-3364.913;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-625.8497,-3053.518;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-48.9574,-2669.39;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;247.5642,-2555.585;Inherit;False;OpacityVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;15;-352.4368,-2994.729;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-245.2476,53.67757;Inherit;False;80;OpacityVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-221.4388,-2885.535;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;6;432.1781,-73.86044;Inherit;False;970.2416;488.4174;ClampWithTime;6;12;11;10;9;8;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-308.3346,277.1315;Inherit;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;85;1181.829,-1289.191;Inherit;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;0.8490566,0.2646701,0.2202741,0;0.8490566,0.2646701,0.2202741,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;1535.122,-407.834;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;1213.829,-953.1911;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-59.33459,157.1315;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-1272.488,-3845.288;Inherit;False;testVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;20;493.8286,-761.1911;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;573.3065,175.0526;Inherit;False;Property;_MinBright;MinBright;3;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;452.4572,-14.105;Inherit;False;Property;_TimeCicle;TimeCicle;5;1;[IntRange];Create;True;0;0;False;0;4;4;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-170.5388,-46.59825;Inherit;False;81;EmissionVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;691.7366,83.00793;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;9;828.3955,-12.44631;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;829.8292,-1145.191;Inherit;False;94;testVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;86;120.0504,-777.2627;Inherit;False;Constant;_Float1;Float 1;9;0;Create;True;0;0;False;0;1.230911;1.802454;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;1031.827,139.2075;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;90;1026.229,-682.0513;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;1293.337,-304.7199;Inherit;False;Property;_Float0;Float 0;8;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;568.5494,283.0042;Inherit;False;Property;_MaxBright;MaxBright;4;0;Create;True;0;0;False;0;0.9;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;1485.829,-1161.191;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;89;800.0751,-538.9578;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;96.28101,-67.65692;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CanAttackAndMoveToNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;35;0
WireConnection;38;0;35;0
WireConnection;44;0;39;1
WireConnection;44;1;35;0
WireConnection;64;0;60;0
WireConnection;62;0;60;0
WireConnection;42;0;38;0
WireConnection;42;1;37;2
WireConnection;43;0;40;0
WireConnection;43;1;39;1
WireConnection;41;0;37;2
WireConnection;41;1;35;0
WireConnection;46;0;44;0
WireConnection;46;1;43;0
WireConnection;68;0;63;2
WireConnection;68;1;60;0
WireConnection;65;0;61;1
WireConnection;65;1;60;0
WireConnection;67;0;64;0
WireConnection;67;1;63;2
WireConnection;45;0;41;0
WireConnection;45;1;42;0
WireConnection;66;0;62;0
WireConnection;66;1;61;1
WireConnection;70;0;65;0
WireConnection;70;1;66;0
WireConnection;79;0;4;2
WireConnection;79;1;78;0
WireConnection;47;0;45;0
WireConnection;47;1;46;0
WireConnection;69;0;68;0
WireConnection;69;1;67;0
WireConnection;48;0;79;0
WireConnection;48;1;47;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;17;0;5;0
WireConnection;17;1;79;0
WireConnection;59;0;71;0
WireConnection;49;0;48;0
WireConnection;51;0;17;0
WireConnection;51;1;49;0
WireConnection;73;0;5;0
WireConnection;73;1;59;0
WireConnection;57;0;51;0
WireConnection;100;0;73;0
WireConnection;100;1;79;0
WireConnection;101;0;100;0
WireConnection;54;0;48;0
WireConnection;54;1;18;0
WireConnection;58;0;57;0
WireConnection;58;1;73;0
WireConnection;55;0;58;0
WireConnection;55;1;54;0
WireConnection;74;0;79;0
WireConnection;74;1;101;0
WireConnection;80;0;74;0
WireConnection;15;0;55;0
WireConnection;81;0;15;0
WireConnection;92;0;90;0
WireConnection;92;1;93;0
WireConnection;84;0;95;0
WireConnection;84;1;20;0
WireConnection;97;0;83;0
WireConnection;97;1;98;0
WireConnection;94;0;79;0
WireConnection;20;3;86;0
WireConnection;8;0;7;0
WireConnection;9;0;8;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;12;2;11;0
WireConnection;90;0;20;0
WireConnection;90;1;89;0
WireConnection;96;0;85;0
WireConnection;96;1;84;0
WireConnection;89;0;20;0
WireConnection;0;2;82;0
WireConnection;0;9;97;0
ASEEND*/
//CHKSM=12EAA7B872C2E7361C6A88BB06B4C2427DFF4DAF