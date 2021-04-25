// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TopPartNode"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_SelectedNodePNG("SelectedNodePNG", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		[KeywordEnum(Key0,Key1,Key2,Key3,Key4,Key5)] _SwitchEmission("SwitchEmission", Float) = 0
		[Toggle(_ISEMISSIONON_ON)] _IsEmissionOn("IsEmissionOn", Float) = 0
		[Toggle(_ADDEFFECTTOEMISSION_ON)] _AddEffectToEmission("AddEffectToEmission", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ADDEFFECTTOEMISSION_ON
		#pragma shader_feature_local _ISEMISSIONON_ON
		#pragma shader_feature_local _SWITCHEMISSION_KEY0 _SWITCHEMISSION_KEY1 _SWITCHEMISSION_KEY2 _SWITCHEMISSION_KEY3 _SWITCHEMISSION_KEY4 _SWITCHEMISSION_KEY5
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _SelectedNodePNG;
		uniform float4 _SelectedNodePNG_ST;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor71 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
			o.Albedo = screenColor71.rgb;
			float4 color132 = IsGammaSpace() ? float4(0,1,0.8569398,0) : float4(0,1,0.7048764,0);
			float4 ColorMove134 = color132;
			float4 CanMoveToNodeVar130 = ColorMove134;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode115 = tex2D( _TextureSample2, uv_TextureSample2 );
			float4 color106 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float4 CanAttackToNodeVar108 = ( tex2DNode115.b * color106 );
			float2 uv_SelectedNodePNG = i.uv_texcoord * _SelectedNodePNG_ST.xy + _SelectedNodePNG_ST.zw;
			float4 tex2DNode119 = tex2D( _SelectedNodePNG, uv_SelectedNodePNG );
			float4 color5 = IsGammaSpace() ? float4(2,1.952941,0,0) : float4(4.594794,4.360297,0,0);
			float4 SelectedNodeVar11 = ( tex2DNode119.r * color5 );
			float4 CanMoveToNodeVar2139 = ( tex2DNode119.g * ColorMove134 );
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			float4 CanMoveToNodeVar3164 = ( tex2D( _TextureSample4, uv_TextureSample4 ).g * ColorMove134 );
			float4 CanMoveToNodeVar4159 = ( tex2DNode115.g * ColorMove134 );
			#if defined(_SWITCHEMISSION_KEY0)
				float4 staticSwitch141 = CanMoveToNodeVar130;
			#elif defined(_SWITCHEMISSION_KEY1)
				float4 staticSwitch141 = CanAttackToNodeVar108;
			#elif defined(_SWITCHEMISSION_KEY2)
				float4 staticSwitch141 = SelectedNodeVar11;
			#elif defined(_SWITCHEMISSION_KEY3)
				float4 staticSwitch141 = ( SelectedNodeVar11 + CanMoveToNodeVar2139 );
			#elif defined(_SWITCHEMISSION_KEY4)
				float4 staticSwitch141 = ( CanMoveToNodeVar3164 + CanAttackToNodeVar108 + SelectedNodeVar11 );
			#elif defined(_SWITCHEMISSION_KEY5)
				float4 staticSwitch141 = ( CanMoveToNodeVar4159 + CanAttackToNodeVar108 );
			#else
				float4 staticSwitch141 = CanMoveToNodeVar130;
			#endif
			#ifdef _ISEMISSIONON_ON
				float4 staticSwitch166 = staticSwitch141;
			#else
				float4 staticSwitch166 = float4( 0,0,0,0 );
			#endif
			float mulTime33 = _Time.y * (float)4;
			float clampResult38 = clamp( sin( mulTime33 ) , 0.0 , 0.7 );
			float ClampWithTimeVar41 = clampResult38;
			#ifdef _ADDEFFECTTOEMISSION_ON
				float4 staticSwitch171 = ( staticSwitch166 * ClampWithTimeVar41 );
			#else
				float4 staticSwitch171 = staticSwitch166;
			#endif
			float4 EmissionVar168 = staticSwitch171;
			o.Emission = EmissionVar168.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;556;938;-3536.651;-827.3914;1.75254;True;False
Node;AmplifyShaderEditor.CommentaryNode;149;153.5273,747.8943;Inherit;False;466.0203;258.2872;ColorMove;2;134;132;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;132;169.5223,805.1859;Inherit;False;Constant;_Color1;Color 1;0;1;[HDR];Create;True;0;0;False;0;0,1,0.8569398,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;400.9959,806.6093;Inherit;False;ColorMove;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;107;1502.086,703.4727;Inherit;False;1125.038;482.7062;CanAttackToNode;4;106;115;108;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;31;1478.098,1799.529;Inherit;False;1137.533;534.9942;CanMoveToNode2;3;138;139;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;156;1487.34,2933.593;Inherit;False;1132.71;465.4442;CanAttackToNode4;3;161;158;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1;1501.265,240.1573;Inherit;False;876.1163;416.8486;SelectedNode;4;11;5;8;119;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;1482.319,2361.003;Inherit;False;1137.533;534.9942;CanMoveToNode3;4;143;144;145;164;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;1595.581,2701.082;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;43;137.3569,228.209;Inherit;False;970.2416;488.4174;ClampWithTime;7;34;32;33;38;41;35;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;5;1556.477,480.6715;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;2,1.952941,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;143;1516.981,2423.838;Inherit;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;-1;None;09de2539f4c20654b8ca667c469d4117;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;161;1565.96,3242.732;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;119;1552.639,281.1393;Inherit;True;Property;_SelectedNodePNG;SelectedNodePNG;1;0;Create;True;0;0;False;0;-1;None;fb3e67538a281444c9af25adfb70c4d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;106;1612.323,937.0425;Inherit;False;Constant;_Color2;Color 2;0;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;115;1556.601,745.784;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;None;cc9ae09d15e94874381f733d6bec688c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;138;1645.812,2198.529;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;2067.158,2030.966;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;1951.132,828.9222;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1955.128,3130.406;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;1947.709,392.1111;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;120;1484.669,1243.193;Inherit;False;1137.533;534.9942;CanMoveToNode1;2;137;30;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1928.626,2516.33;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;32;187.8892,357.911;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;344.6169,366.62;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;164;2221.232,2543.224;Inherit;True;CanMoveToNodeVar3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;1706.712,1432.235;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;2153.073,366.2988;Inherit;True;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;167;2888.512,1299.533;Inherit;False;1659.173;1324.878;EmissionCode;15;166;141;102;155;154;163;101;153;150;162;46;168;171;172;173;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;2252.514,811.8441;Inherit;True;CanAttackToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;2268.814,2004.021;Inherit;True;CanMoveToNodeVar2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;2237.768,3074.27;Inherit;True;CanMoveToNodeVar4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;2084.054,1421.965;Inherit;True;CanMoveToNodeVar1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;206.2791,474.9859;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;533.5731,289.6231;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;2940.158,2008.036;Inherit;True;139;CanMoveToNodeVar2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;2954.755,1792.436;Inherit;True;11;SelectedNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;2944.595,2208.592;Inherit;True;164;CanMoveToNodeVar3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;190.8127,603.2568;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;2940.094,2409.083;Inherit;True;159;CanMoveToNodeVar4;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;2940.297,1577.557;Inherit;True;108;CanAttackToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;3415.797,2126.724;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;3402.886,1877.624;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;3392.976,1592.876;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;2932.396,1356.142;Inherit;True;30;CanMoveToNodeVar1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;38;743.9337,481.111;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;141;3718.71,1454.247;Inherit;False;Property;_SwitchEmission;SwitchEmission;3;0;Create;True;0;0;False;0;0;0;2;True;;KeywordEnum;6;Key0;Key1;Key2;Key3;Key4;Key5;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;847.1038,338.8841;Inherit;False;ClampWithTimeVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;166;3988.697,1444.286;Inherit;False;Property;_IsEmissionOn;IsEmissionOn;4;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;3782.154,2143.148;Inherit;True;41;ClampWithTimeVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;4007.121,1949.432;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;171;4090.853,1683.459;Inherit;False;Property;_AddEffectToEmission;AddEffectToEmission;6;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;4344.25,1921.444;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-587.2651,684.983;Inherit;False;Property;_Oppa;Oppa;5;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;-524.3657,545.8796;Inherit;False;168;EmissionVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;71;-488.9937,234.789;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-183.304,502.0342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TopPartNode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;134;0;132;0
WireConnection;129;0;119;2
WireConnection;129;1;138;0
WireConnection;105;0;115;3
WireConnection;105;1;106;0
WireConnection;158;0;115;2
WireConnection;158;1;161;0
WireConnection;8;0;119;1
WireConnection;8;1;5;0
WireConnection;145;0;143;2
WireConnection;145;1;144;0
WireConnection;33;0;32;0
WireConnection;164;0;145;0
WireConnection;11;0;8;0
WireConnection;108;0;105;0
WireConnection;139;0;129;0
WireConnection;159;0;158;0
WireConnection;30;0;137;0
WireConnection;34;0;33;0
WireConnection;163;0;162;0
WireConnection;163;1;150;0
WireConnection;155;0;46;0
WireConnection;155;1;150;0
WireConnection;155;2;153;0
WireConnection;154;0;153;0
WireConnection;154;1;102;0
WireConnection;38;0;34;0
WireConnection;38;1;48;0
WireConnection;38;2;35;0
WireConnection;141;1;101;0
WireConnection;141;0;150;0
WireConnection;141;2;153;0
WireConnection;141;3;154;0
WireConnection;141;4;155;0
WireConnection;141;5;163;0
WireConnection;41;0;38;0
WireConnection;166;0;141;0
WireConnection;173;0;166;0
WireConnection;173;1;172;0
WireConnection;171;1;166;0
WireConnection;171;0;173;0
WireConnection;168;0;171;0
WireConnection;0;0;71;0
WireConnection;0;2;169;0
ASEEND*/
//CHKSM=A4FE9A198DEA6564D8ECB3066CEF7BBCF6A3433F