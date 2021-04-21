// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TopPartNode"
{
	Properties
	{
		_Oppa("Oppa", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float4 screenPos;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Oppa;


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
			o.Alpha = _Oppa;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;579;938;-1582.649;-70.52707;4.126131;False;False
Node;AmplifyShaderEditor.CommentaryNode;149;153.5273,747.8943;Inherit;False;466.0203;258.2872;ColorMove;2;134;132;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;43;137.3569,228.209;Inherit;False;970.2416;488.4174;ClampWithTime;7;34;32;33;38;41;35;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;167;2888.512,1299.533;Inherit;False;1659.173;1324.878;EmissionCode;12;166;141;102;155;154;163;101;153;150;162;46;168;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;107;1502.086,703.4727;Inherit;False;1125.038;482.7062;CanAttackToNode;4;106;115;108;105;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;31;1478.098,1799.529;Inherit;False;1137.533;534.9942;CanMoveToNode2;3;138;139;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;156;1487.34,2933.593;Inherit;False;1132.71;465.4442;CanAttackToNode4;3;161;158;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1;1501.265,240.1573;Inherit;False;876.1163;416.8486;SelectedNode;4;11;5;8;119;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;1482.319,2361.003;Inherit;False;1137.533;534.9942;CanMoveToNode3;4;143;144;145;164;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;1484.669,1243.193;Inherit;False;1137.533;534.9942;CanMoveToNode1;2;137;30;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;141;3718.71,1454.247;Inherit;False;Property;_SwitchEmission;SwitchEmission;3;0;Create;True;0;0;False;0;0;0;0;True;;KeywordEnum;6;Key0;Key1;Key2;Key3;Key4;Key5;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;3402.886,1877.624;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;2940.094,2409.083;Inherit;True;159;CanMoveToNodeVar4;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;2940.158,2008.036;Inherit;True;139;CanMoveToNodeVar2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;3392.976,1592.876;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;2954.755,1792.436;Inherit;True;11;SelectedNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;2944.595,2208.592;Inherit;True;164;CanMoveToNodeVar3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;3415.797,2126.724;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;2153.073,366.2988;Inherit;True;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-587.2651,684.983;Inherit;False;Property;_Oppa;Oppa;5;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;71;-488.9937,234.789;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;33;344.6169,366.62;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;533.5731,289.6231;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;2932.396,1356.142;Inherit;True;30;CanMoveToNodeVar1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;206.2791,474.9859;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;847.1038,338.8841;Inherit;False;ClampWithColorVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;190.8127,603.2568;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;2940.297,1577.557;Inherit;True;108;CanAttackToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;4286.496,1440.268;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;32;187.8892,357.911;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;2268.814,2004.021;Inherit;True;CanMoveToNodeVar2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;-524.3657,545.8796;Inherit;False;168;EmissionVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;132;169.5223,805.1859;Inherit;False;Constant;_Color1;Color 1;0;1;[HDR];Create;True;0;0;False;0;0,1,0.8569398,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;2084.054,1421.965;Inherit;True;CanMoveToNodeVar1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;115;1556.601,745.784;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;None;cc9ae09d15e94874381f733d6bec688c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;106;1612.323,937.0425;Inherit;False;Constant;_Color2;Color 2;0;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;119;1552.639,281.1393;Inherit;True;Property;_SelectedNodePNG;SelectedNodePNG;1;0;Create;True;0;0;False;0;-1;None;fb3e67538a281444c9af25adfb70c4d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;138;1645.812,2198.529;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;1595.581,2701.082;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;143;1516.981,2423.838;Inherit;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;-1;None;09de2539f4c20654b8ca667c469d4117;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;1556.477,480.6715;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;2,1.952941,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;161;1565.96,3242.732;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;38;640.3632,419.6709;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;2252.514,811.8441;Inherit;True;CanAttackToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;1955.128,3130.406;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;1951.132,828.9222;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;134;400.9959,806.6093;Inherit;False;ColorMove;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;1706.712,1432.235;Inherit;False;134;ColorMove;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;2237.768,3074.27;Inherit;True;CanMoveToNodeVar4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;164;2221.232,2543.224;Inherit;True;CanMoveToNodeVar3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;166;3988.697,1444.286;Inherit;False;Property;_IsEmissionOn;IsEmissionOn;4;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;1947.709,392.1111;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1928.626,2516.33;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;2067.158,2030.966;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-183.304,502.0342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TopPartNode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;141;1;101;0
WireConnection;141;0;150;0
WireConnection;141;2;153;0
WireConnection;141;3;154;0
WireConnection;141;4;155;0
WireConnection;141;5;163;0
WireConnection;155;0;46;0
WireConnection;155;1;150;0
WireConnection;155;2;153;0
WireConnection;154;0;153;0
WireConnection;154;1;102;0
WireConnection;163;0;162;0
WireConnection;163;1;150;0
WireConnection;11;0;8;0
WireConnection;33;0;32;0
WireConnection;34;0;33;0
WireConnection;41;0;38;0
WireConnection;168;0;166;0
WireConnection;139;0;129;0
WireConnection;30;0;137;0
WireConnection;38;0;34;0
WireConnection;38;1;48;0
WireConnection;38;2;35;0
WireConnection;108;0;105;0
WireConnection;158;0;115;2
WireConnection;158;1;161;0
WireConnection;105;0;115;3
WireConnection;105;1;106;0
WireConnection;134;0;132;0
WireConnection;159;0;158;0
WireConnection;164;0;145;0
WireConnection;166;0;141;0
WireConnection;8;0;119;1
WireConnection;8;1;5;0
WireConnection;145;0;143;2
WireConnection;145;1;144;0
WireConnection;129;0;119;2
WireConnection;129;1;138;0
WireConnection;0;0;71;0
WireConnection;0;9;87;0
ASEEND*/
//CHKSM=D5AC72D41C323A2F82DEE8308F19FCBD32C89504