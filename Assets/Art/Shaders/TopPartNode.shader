// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TopPartNode"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[KeywordEnum(Key0,Key1,Key2)] _Keyword0("Keyword 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD0_KEY0 _KEYWORD0_KEY1 _KEYWORD0_KEY2
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 screenColor71 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_screenPosNorm.xy);
			o.Albedo = screenColor71.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV93 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode93 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV93, 7.814607 ) );
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 color98 = IsGammaSpace() ? float4(4,0,0,0) : float4(21.11213,0,0,0);
			float4 CanAttackToNodeVar90 = ( ( 1.0 - fresnelNode93 ) * tex2D( _TextureSample2, uv_TextureSample2 ).b * color98 );
			float fresnelNdotV25 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode25 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV25, 0.7 ) );
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 color37 = IsGammaSpace() ? float4(0,2.94902,2.996078,0) : float4(0,10.7967,11.17936,0);
			float4 CanMoveToNodeVar30 = ( ( 1.0 - fresnelNode25 ) * tex2D( _TextureSample1, uv_TextureSample1 ).g * color37 );
			#if defined(_KEYWORD0_KEY0)
				float4 staticSwitch115 = ( CanAttackToNodeVar90 + CanMoveToNodeVar30 );
			#elif defined(_KEYWORD0_KEY1)
				float4 staticSwitch115 = CanAttackToNodeVar90;
			#elif defined(_KEYWORD0_KEY2)
				float4 staticSwitch115 = CanMoveToNodeVar30;
			#else
				float4 staticSwitch115 = ( CanAttackToNodeVar90 + CanMoveToNodeVar30 );
			#endif
			o.Emission = staticSwitch115.rgb;
			o.Alpha = 1;
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
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
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;73;778;938;-186.952;-1307.937;2.319556;False;False
Node;AmplifyShaderEditor.CommentaryNode;99;265.367,2094.603;Inherit;False;1581.547;921.5889;CanAttackToNode;11;90;98;91;92;97;96;95;94;93;102;104;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;31;245.446,1042.637;Inherit;False;1568.891;819.5419;CanMoveToNode;9;30;28;27;25;23;24;26;88;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;91;353.3902,2300.548;Inherit;False;Constant;_Float6;Float 6;5;0;Create;True;0;0;False;0;7.814607;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;351.1513,2196.957;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;328.5329,1237.133;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0.7;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;326.2939,1133.542;Inherit;False;Constant;_Float8;Float 8;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;25;717.8901,1116.763;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;93;742.7474,2180.178;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;98;822.2954,2655.661;Inherit;False;Constant;_Color1;Color 1;0;1;[HDR];Create;True;0;0;False;0;4,0,0,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;96;806.1945,2431.315;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;False;0;-1;3ffdc12720b4db94c8cb554ad735d399;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;94;989.1646,2168.508;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;964.3073,1105.093;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;37;918.4844,1641.654;Inherit;False;Constant;_ColorTile;ColorTile;0;1;[HDR];Create;True;0;0;False;0;0,2.94902,2.996078,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;88;500.7602,1453.261;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;3ffdc12720b4db94c8cb554ad735d399;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;1279.577,2422.651;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1254.719,1365.26;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;1552.84,2425.346;Inherit;False;CanAttackToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;1550.25,1522.537;Inherit;False;CanMoveToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-1502.49,688.9902;Inherit;True;90;CanAttackToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-1466.884,1074.19;Inherit;True;30;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;74;-1273.445,114.1653;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-1004.432,597.829;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1;272.8647,137.291;Inherit;False;1479.932;757.176;SelectedNode;10;6;7;8;9;10;11;2;3;4;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;43;1884.317,147.9393;Inherit;False;1252.094;722.976;ClampWithTime;9;42;41;40;38;34;35;33;32;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;35;1930.461,594.2894;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;6;697.2598,359.7248;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;38;2422.063,350.3706;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;10;1327.211,462.729;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-357.74,948.8931;Inherit;False;Property;_Oppa;Oppa;2;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;961.1624,608.6425;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;333.7021,368.1698;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;2970.509,516.3356;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;2730.47,256.8607;Inherit;False;Constant;_TimeFloat;TimeFloat;0;0;Create;True;0;0;False;0;3;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;373.5171,594.4913;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;0.8584906,0.8415912,0.1741278,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RefractOpVec;67;-1187.803,-352.7987;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;64;-1650.752,-659.9507;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;7;998.4246,298.6577;Inherit;False;30;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;718.8657,1346.694;Inherit;False;41;ClampVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;102;460.6534,2532.584;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;2833.478,724.0905;Inherit;False;ClampVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;1945.927,466.0168;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1245.204,-961.6392;Inherit;False;Property;_ActiveInvisibility;Active Invisibility;1;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;2282.365,247.746;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;1192.875,450.9577;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;104;573.1924,2735.779;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;54;-1.748671,-951.0872;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;71;-984.205,273.724;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;56;271.428,-984.6953;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;1487.489,460.3867;Inherit;False;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;66;-1584.146,-334.2541;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;340.4749,471.8739;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;0.5537913;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;968.7583,2859.434;Inherit;False;41;ClampVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;70;-1161.474,-731.2208;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-547.4396,-404.5863;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;375.7801,226.7896;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;32;1905.598,334.3167;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.NegateNode;65;-1420.259,-597.7181;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-332.1356,-952.0491;Inherit;False;Constant;_fresnel_Scale;fresnel_Scale;8;0;Create;True;0;0;False;0;1.540285;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;2075.125,280.8658;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;115;-690.9984,833.4317;Inherit;False;Property;_Keyword0;Keyword 0;5;0;Create;True;0;0;False;0;0;0;0;True;;KeywordEnum;3;Key0;Key1;Key2;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-346.6716,-844.2674;Inherit;False;Constant;_fresnel_Power;fresnel_Power;9;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-891.9813,-379.9149;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;8811b88df877df34497837e713f37ff1;362603bd5608762448f5868471d4c6cb;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-183.304,502.0342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TopPartNode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;2;23;0
WireConnection;25;3;24;0
WireConnection;93;2;92;0
WireConnection;93;3;91;0
WireConnection;94;0;93;0
WireConnection;27;0;25;0
WireConnection;95;0;94;0
WireConnection;95;1;96;3
WireConnection;95;2;98;0
WireConnection;28;0;27;0
WireConnection;28;1;88;2
WireConnection;28;2;37;0
WireConnection;90;0;95;0
WireConnection;30;0;28;0
WireConnection;101;0;100;0
WireConnection;101;1;46;0
WireConnection;6;0;2;2
WireConnection;6;1;4;0
WireConnection;6;2;3;0
WireConnection;38;0;34;0
WireConnection;38;1;48;0
WireConnection;38;2;35;0
WireConnection;10;0;9;0
WireConnection;8;0;6;0
WireConnection;8;1;5;0
WireConnection;40;0;34;0
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;67;2;70;0
WireConnection;41;0;40;0
WireConnection;34;0;33;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;54;2;58;0
WireConnection;54;3;59;0
WireConnection;71;0;74;0
WireConnection;56;0;54;0
WireConnection;11;0;10;0
WireConnection;70;0;69;0
WireConnection;68;0;69;0
WireConnection;68;1;53;0
WireConnection;65;0;64;0
WireConnection;33;0;32;0
WireConnection;115;1;101;0
WireConnection;115;0;100;0
WireConnection;115;2;46;0
WireConnection;53;1;67;0
WireConnection;0;0;71;0
WireConnection;0;2;115;0
ASEEND*/
//CHKSM=F60BD1A9794FD232D901249F4EDE5FCAF59F0642