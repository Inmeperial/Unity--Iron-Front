// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TopPartNode"
{
	Properties
	{
		_Float0("Float 0", Range( 0 , 1)) = 0.1
		_Float1("Float 1", Range( 0 , 1)) = 0.1
		_Float3("Float 3", Range( 0.5 , 1)) = 0.7580875
		_Float4("Float 4", Range( 0.5 , 1)) = 0.7580875
		[Enum(One,0,Two,1,Tree,2)]_Remap("Remap", Range( 0 , 2)) = 0.5556372
		_Color3("Color 3", Color) = (1,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color3;
		uniform float _Float1;
		uniform float _Float0;
		uniform float _Float3;
		uniform float _Float4;
		uniform float _Remap;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_204_0 = saturate( ( step( i.uv_texcoord.y , _Float1 ) + step( ( 1.0 - i.uv_texcoord.y ) , _Float1 ) + step( i.uv_texcoord.x , _Float0 ) + step( ( 1.0 - i.uv_texcoord.x ) , _Float0 ) ) );
			float temp_output_203_0 = saturate( ( ( step( i.uv_texcoord.y , _Float3 ) * step( ( 1.0 - _Float3 ) , i.uv_texcoord.y ) ) + ( step( i.uv_texcoord.x , _Float4 ) * step( ( 1.0 - _Float4 ) , i.uv_texcoord.x ) ) ) );
			float temp_output_217_0 = ( _Remap / 2.0 );
			float lerpResult207 = lerp( temp_output_204_0 , ( temp_output_203_0 * temp_output_204_0 ) , (0.0 + (temp_output_217_0 - 0.0) * (1.0 - 0.0) / (0.5 - 0.0)));
			float lerpResult208 = lerp( lerpResult207 , saturate( ( temp_output_204_0 - temp_output_203_0 ) ) , (0.0 + (temp_output_217_0 - 0.5) * (1.0 - 0.0) / (1.0 - 0.5)));
			float temp_output_216_0 = saturate( lerpResult208 );
			o.Emission = ( _Color3 * temp_output_216_0 ).rgb;
			o.Alpha = temp_output_216_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;847;938;191.2155;901.6084;4.12842;False;False
Node;AmplifyShaderEditor.RangedFloatNode;186;-3053.043,-1719.531;Inherit;False;Property;_Float3;Float 3;8;0;Create;True;0;0;False;0;0.7580875;-0.5;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-2914.295,-1165.967;Inherit;False;Property;_Float4;Float 4;9;0;Create;True;0;0;False;0;0.7580875;-0.5;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;174;-3005.358,67.81932;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;198;-2679.382,-914.1718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;195;-2887.831,-1328.387;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;185;-3026.58,-1881.952;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;190;-2818.131,-1467.736;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;180;-2829.676,-509.1721;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;183;-2754.484,-44.89565;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;193;-2615.709,-1563.141;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;187;-2631.8,-1809.3;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;177;-2792.594,494.2141;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;197;-2493.052,-1255.736;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-2868.355,-327.3865;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;False;0;0.1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;199;-2476.961,-1009.577;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-2928.395,269.5424;Inherit;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;178;-2576.114,421.5019;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-2228.182,-1198.81;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;182;-2516.073,-175.4273;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;181;-2512.493,-407.2667;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;175;-2572.534,189.6625;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-2314.945,-1692.962;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-1653.718,314.548;Inherit;False;Constant;_Float6;Float 6;12;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-1867.198,134.9446;Inherit;False;Property;_Remap;Remap;10;1;[Enum];Create;True;3;One;0;Two;1;Tree;2;0;False;0;0.5556372;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;201;-1972.76,-1543.645;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;184;-2194.381,-60.34804;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;217;-1457.636,146.3753;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;203;-1692.766,-1268.526;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;204;-1936.815,-489.9502;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;210;-1447.901,-113.0147;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;-1629.784,-551.2686;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;202;-1480.021,-981.2898;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;211;-1155.196,40.5646;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;207;-1219.898,-355.5862;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;213;-1162.462,-770.2114;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;208;-900.9203,-263.5034;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;1;1501.265,240.1573;Inherit;False;876.1163;416.8486;SelectedNode;4;11;5;8;119;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;216;-611.0532,-208.1486;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;31;1478.098,1799.529;Inherit;False;1137.533;534.9942;CanMoveToNode2;3;138;139;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;215;-570.6692,-488.1465;Inherit;False;Property;_Color3;Color 3;11;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;43;137.3569,228.209;Inherit;False;970.2416;488.4174;ClampWithTime;7;34;32;33;38;41;35;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;167;2888.512,1299.533;Inherit;False;1659.173;1324.878;EmissionCode;14;166;141;102;155;154;163;101;153;150;162;46;168;171;172;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;156;1487.34,2933.593;Inherit;False;1132.71;465.4442;CanAttackToNode4;3;161;158;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;1482.319,2361.003;Inherit;False;1137.533;534.9942;CanMoveToNode3;4;143;144;145;164;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;149;153.5273,747.8943;Inherit;False;466.0203;258.2872;ColorMove;2;134;132;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;1484.669,1243.193;Inherit;False;1137.533;534.9942;CanMoveToNode1;2;137;30;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;107;1502.086,703.4727;Inherit;False;1125.038;482.7062;CanAttackToNode;4;106;115;108;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;3392.976,1592.876;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;166;4053.697,1294.785;Inherit;False;Property;_IsEmissionOn;IsEmissionOn;4;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;3402.886,1877.624;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;2940.094,2409.083;Inherit;True;159;CanMoveToNodeVar4;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;2940.158,2008.036;Inherit;True;139;CanMoveToNodeVar2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;1595.581,2701.082;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;4677.877,1347.489;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;3415.797,2126.724;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;141;3718.71,1454.247;Inherit;True;Property;_SwitchEmission;SwitchEmission;3;0;Create;True;0;0;False;0;0;0;0;True;;KeywordEnum;6;Key0;Key1;Key2;Key3;Key4;Key5;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;1645.812,2198.529;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;206.2791,474.9859;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;2067.158,2030.966;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1928.626,2516.33;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;2153.073,366.2988;Inherit;True;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;1556.477,480.6715;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;2,1.952941,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;143;1516.981,2423.838;Inherit;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;-1;09de2539f4c20654b8ca667c469d4117;09de2539f4c20654b8ca667c469d4117;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;153;2954.755,1792.436;Inherit;True;11;SelectedNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;2252.514,811.8441;Inherit;True;CanAttackToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;1951.132,828.9222;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;1565.96,3242.732;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;2944.595,2208.592;Inherit;True;164;CanMoveToNodeVar3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;34;533.5731,289.6231;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;2940.297,1577.557;Inherit;True;108;CanAttackToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;115;1556.601,745.784;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;cc9ae09d15e94874381f733d6bec688c;cc9ae09d15e94874381f733d6bec688c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;101;2932.396,1356.142;Inherit;True;30;CanMoveToNodeVar1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;171;4360.51,1393.722;Inherit;False;Property;_AddEffectToEmission;AddEffectToEmission;5;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;2237.768,3074.27;Inherit;True;CanMoveToNodeVar4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;2268.814,2004.021;Inherit;True;CanMoveToNodeVar2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;164;2221.232,2543.224;Inherit;True;CanMoveToNodeVar3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-401.0551,-200.0716;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1955.128,3130.406;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;400.9959,806.6093;Inherit;False;ColorMove;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;119;1552.639,281.1393;Inherit;True;Property;_SelectedNodePNG;SelectedNodePNG;1;0;Create;True;0;0;False;0;-1;fb3e67538a281444c9af25adfb70c4d2;fb3e67538a281444c9af25adfb70c4d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;4344.25,1915.628;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;132;169.5223,805.1859;Inherit;False;Constant;_Color1;Color 1;0;1;[HDR];Create;True;0;0;False;0;0,1,0.8569398,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;172;4081.665,1548.087;Inherit;True;41;ClampWithTimeVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;32;187.8892,357.911;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;847.1038,338.8841;Inherit;False;ClampWithTimeVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;190.8127,603.2568;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;38;743.9337,481.111;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;344.6169,366.62;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;1947.709,392.1111;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;1706.712,1432.235;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;2084.054,1421.965;Inherit;True;CanMoveToNodeVar1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;106;1612.323,937.0425;Inherit;False;Constant;_Color2;Color 2;0;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-180.6117,502.0342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;TopPartNode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;198;0;196;0
WireConnection;190;0;186;0
WireConnection;183;0;180;2
WireConnection;193;0;190;0
WireConnection;193;1;185;2
WireConnection;187;0;185;2
WireConnection;187;1;186;0
WireConnection;177;0;174;1
WireConnection;197;0;195;1
WireConnection;197;1;196;0
WireConnection;199;0;198;0
WireConnection;199;1;195;1
WireConnection;178;0;177;0
WireConnection;178;1;176;0
WireConnection;200;0;197;0
WireConnection;200;1;199;0
WireConnection;182;0;183;0
WireConnection;182;1;179;0
WireConnection;181;0;180;2
WireConnection;181;1;179;0
WireConnection;175;0;174;1
WireConnection;175;1;176;0
WireConnection;194;0;187;0
WireConnection;194;1;193;0
WireConnection;201;0;194;0
WireConnection;201;1;200;0
WireConnection;184;0;181;0
WireConnection;184;1;182;0
WireConnection;184;2;175;0
WireConnection;184;3;178;0
WireConnection;217;0;209;0
WireConnection;217;1;218;0
WireConnection;203;0;201;0
WireConnection;204;0;184;0
WireConnection;210;0;217;0
WireConnection;206;0;203;0
WireConnection;206;1;204;0
WireConnection;202;0;204;0
WireConnection;202;1;203;0
WireConnection;211;0;217;0
WireConnection;207;0;204;0
WireConnection;207;1;206;0
WireConnection;207;2;210;0
WireConnection;213;0;202;0
WireConnection;208;0;207;0
WireConnection;208;1;213;0
WireConnection;208;2;211;0
WireConnection;216;0;208;0
WireConnection;154;0;153;0
WireConnection;154;1;102;0
WireConnection;166;0;141;0
WireConnection;155;0;46;0
WireConnection;155;1;150;0
WireConnection;155;2;153;0
WireConnection;173;0;166;0
WireConnection;173;1;172;0
WireConnection;163;0;162;0
WireConnection;163;1;150;0
WireConnection;141;1;101;0
WireConnection;141;0;150;0
WireConnection;141;2;153;0
WireConnection;141;3;154;0
WireConnection;141;4;155;0
WireConnection;141;5;163;0
WireConnection;129;0;119;2
WireConnection;129;1;138;0
WireConnection;145;0;143;2
WireConnection;145;1;144;0
WireConnection;11;0;8;0
WireConnection;108;0;105;0
WireConnection;105;0;115;3
WireConnection;105;1;106;0
WireConnection;34;0;33;0
WireConnection;171;1;166;0
WireConnection;171;0;173;0
WireConnection;159;0;158;0
WireConnection;139;0;129;0
WireConnection;164;0;145;0
WireConnection;214;0;215;0
WireConnection;214;1;216;0
WireConnection;158;0;115;2
WireConnection;158;1;161;0
WireConnection;134;0;132;0
WireConnection;168;0;141;0
WireConnection;41;0;38;0
WireConnection;38;0;34;0
WireConnection;38;1;48;0
WireConnection;38;2;35;0
WireConnection;33;0;32;0
WireConnection;8;0;119;1
WireConnection;8;1;5;0
WireConnection;30;0;137;0
WireConnection;0;2;214;0
WireConnection;0;9;216;0
ASEEND*/
//CHKSM=926EC194C87819010A74CC016EAC270B60004A21