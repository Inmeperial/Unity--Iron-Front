// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CanAttackAndMoveToNode Shader"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
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
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color85 = IsGammaSpace() ? float4(0.8490566,0.2646701,0.2202741,0) : float4(0.6903409,0.05694805,0.03977691,0);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 color78 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 temp_output_79_0 = ( tex2D( _TextureSample2, uv_TextureSample2 ).g * color78 );
			float4 testVar94 = temp_output_79_0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV20 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode20 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV20, 1.230911 ) );
			float4 temp_output_84_0 = ( testVar94 * fresnelNode20 );
			o.Emission = ( color85 * temp_output_84_0 ).rgb;
			o.Alpha = temp_output_84_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;1020;938;1625.516;1152.091;1.970145;True;False
Node;AmplifyShaderEditor.ColorNode;78;-2378.756,-3419.608;Inherit;False;Constant;_Color1;Color 1;5;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-2560.179,-3624.249;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;cc9ae09d15e94874381f733d6bec688c;cc9ae09d15e94874381f733d6bec688c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2006.286,-3575.184;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-1555.122,-169.8324;Inherit;False;Constant;_Float1;Float 1;9;0;Create;True;0;0;False;0;1.230911;1.802454;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-1272.488,-3845.288;Inherit;False;testVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;20;-1181.344,-153.7608;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-845.3438,-537.7609;Inherit;False;94;testVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;85;-493.3437,-681.7609;Inherit;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;0.8490566,0.2646701,0.2202741,0;0.8490566,0.2646701,0.2202741,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-461.3436,-345.7608;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;6;432.1781,-73.86044;Inherit;False;970.2416;488.4174;ClampWithTime;6;12;11;10;9;8;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-3114.391,-3262.339;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;62;-1742.303,-1108.06;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1216.454,-2683.509;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-2174.768,-1298.356;Inherit;False;Property;_InnerRedLines;InnerRedLines;7;0;Create;True;0;0;False;0;0.7580875;0.52;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;66;-1495.559,-1101.895;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;89;-875.0977,68.47271;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;373.1929,518.3987;Inherit;False;81;EmissionVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-381.8359,302.7105;Inherit;False;Property;_Float0;Float 0;8;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;90;-648.9438,-74.62103;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;51;-1302.004,-3320.076;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-2975.642,-2708.774;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;41;-2719.612,-3189.687;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-140.0508,199.5964;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-857.0764,-2446.608;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;9;828.3955,-12.44631;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-920.9146,-3364.913;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-189.3437,-553.7609;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;568.5494,283.0042;Inherit;False;Property;_MaxBright;MaxBright;4;0;Create;True;0;0;False;0;0.9;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;64;-1881.052,-1661.625;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;-1709.839,-2964.146;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;68;-1694.722,-2003.189;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;691.7366,83.00793;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-2089.5,-2075.841;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2011.156,-2919.845;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;-2905.942,-2848.123;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-986.2642,-1733.346;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;573.3065,175.0526;Inherit;False;Property;_MinBright;MinBright;3;0;Create;True;0;0;False;0;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;-1052.247,-3437.272;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2402.756,-3073.349;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-1486.791,-2589.932;Inherit;False;Property;_ColorMove;ColorMove;2;0;Create;True;0;0;False;0;0,0.9367321,1,0;0,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;83;389.4842,608.6745;Inherit;False;80;OpacityVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;65;-1555.973,-1449.625;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;1031.827,139.2075;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1377.865,-1886.85;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;-316.4008,-2578.23;Inherit;False;OpacityVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2315.992,-2579.197;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-1678.63,-1757.03;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-3199.66,-2487.111;Inherit;False;Property;_RedLines;RedLines;6;0;Create;True;0;0;False;0;0.7580875;0.6;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-609.3467,-2659.859;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;44;-2580.863,-2636.123;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-1291.101,-1392.699;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-352.4368,-2994.729;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;43;-2520.45,-2288.393;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1995.84,-3861.423;Inherit;False;Property;_ColorAttack;ColorAttack;1;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-625.8497,-3053.518;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1669.783,-3783.775;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;452.4572,-14.105;Inherit;False;Property;_TimeCicle;TimeCicle;5;1;[IntRange];Create;True;0;0;False;0;4;4;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;49;-1503.702,-3103.516;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;59;-896.3098,-1984.244;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;42;-2703.52,-2943.528;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-1950.751,-1522.276;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-221.4388,-2885.535;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;40;-2767.193,-2294.558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;96.28101,-67.65692;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CanAttackAndMoveToNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;79;0;4;2
WireConnection;79;1;78;0
WireConnection;94;0;79;0
WireConnection;20;3;86;0
WireConnection;84;0;95;0
WireConnection;84;1;20;0
WireConnection;62;0;60;0
WireConnection;54;0;48;0
WireConnection;54;1;18;0
WireConnection;66;0;62;0
WireConnection;66;1;61;1
WireConnection;89;0;20;0
WireConnection;90;0;20;0
WireConnection;90;1;89;0
WireConnection;51;0;17;0
WireConnection;51;1;49;0
WireConnection;41;0;37;2
WireConnection;41;1;35;0
WireConnection;92;0;90;0
WireConnection;92;1;93;0
WireConnection;73;0;5;0
WireConnection;73;1;59;0
WireConnection;9;0;8;0
WireConnection;58;0;57;0
WireConnection;58;1;73;0
WireConnection;96;0;85;0
WireConnection;96;1;84;0
WireConnection;64;0;60;0
WireConnection;48;0;79;0
WireConnection;48;1;47;0
WireConnection;68;0;63;2
WireConnection;68;1;60;0
WireConnection;8;0;7;0
WireConnection;47;0;45;0
WireConnection;47;1;46;0
WireConnection;38;0;35;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;57;0;51;0
WireConnection;45;0;41;0
WireConnection;45;1;42;0
WireConnection;65;0;61;1
WireConnection;65;1;60;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;12;2;11;0
WireConnection;69;0;68;0
WireConnection;69;1;67;0
WireConnection;80;0;74;0
WireConnection;46;0;44;0
WireConnection;46;1;43;0
WireConnection;67;0;64;0
WireConnection;67;1;63;2
WireConnection;74;0;79;0
WireConnection;74;1;73;0
WireConnection;44;0;39;1
WireConnection;44;1;35;0
WireConnection;70;0;65;0
WireConnection;70;1;66;0
WireConnection;15;0;55;0
WireConnection;43;0;40;0
WireConnection;43;1;39;1
WireConnection;55;0;58;0
WireConnection;55;1;54;0
WireConnection;17;0;5;0
WireConnection;17;1;79;0
WireConnection;49;0;48;0
WireConnection;59;0;71;0
WireConnection;42;0;38;0
WireConnection;42;1;37;2
WireConnection;81;0;15;0
WireConnection;40;0;35;0
WireConnection;0;2;96;0
WireConnection;0;9;84;0
ASEEND*/
//CHKSM=A7BFFCB9C25CE6EF450C8EBD1E19B2F8DA952B7D