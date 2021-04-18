// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TopPartNode"
{
	Properties
	{
		[Toggle(_ISMOVEAVAILABLE_ON)] _isMoveAvailable("isMoveAvailable", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ISMOVEAVAILABLE_ON
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
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )

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
			float fresnelNdotV25 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode25 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV25, 0.7 ) );
			float mulTime33 = _Time.y * (float)4;
			float temp_output_34_0 = sin( mulTime33 );
			float4 color37 = IsGammaSpace() ? float4(0,3.921569,4,0) : float4(0,20.21211,21.11213,0);
			float4 ClampWithColorVar41 = saturate( ( temp_output_34_0 * color37 ) );
			float4 CanMoveToNodeVar30 = saturate( ( ( 1.0 - fresnelNode25 ) * ClampWithColorVar41 ) );
			#ifdef _ISMOVEAVAILABLE_ON
				float4 staticSwitch82 = CanMoveToNodeVar30;
			#else
				float4 staticSwitch82 = float4( 0,0,0,0 );
			#endif
			o.Emission = staticSwitch82.rgb;
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
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
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
0;73;706;938;136.8353;-973.5258;1.665064;True;False
Node;AmplifyShaderEditor.CommentaryNode;43;318.3003,1771.048;Inherit;False;1252.094;722.976;ClampWithTime;11;42;41;40;39;38;34;37;35;33;32;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;32;339.5804,1957.426;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;509.1058,1903.975;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;716.3448,1870.855;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;37;880.8702,2269.581;Inherit;False;Constant;_ColorTile;ColorTile;0;1;[HDR];Create;True;0;0;False;0;0,3.921569,4,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;1287.481,2049.369;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;31;270.0421,1088.755;Inherit;False;1453.002;484.403;CanMoveToNode;8;30;29;27;25;28;26;24;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;24;353.129,1283.251;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0.7;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;1404.49,2139.445;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;350.89,1179.66;Inherit;False;Constant;_Float8;Float 8;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;25;742.4857,1162.881;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;1267.459,2347.2;Inherit;False;ClampWithColorVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;712.1695,1405.925;Inherit;False;41;ClampWithColorVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;27;988.9031,1151.211;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1191.616,1289.466;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;29;1444.589,1234.181;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;1437.14,1445.073;Inherit;False;CanMoveToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;74;-1273.445,114.1653;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;46;-895.937,602.4838;Inherit;False;30;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1;272.8647,137.291;Inherit;False;1479.932;757.176;SelectedNode;10;6;7;8;9;10;11;2;3;4;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;53;-891.9813,-379.9149;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;8811b88df877df34497837e713f37ff1;362603bd5608762448f5868471d4c6cb;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;56;271.428,-984.6953;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;6;697.2598,359.7248;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;373.5171,594.4913;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;0.8584906,0.8415912,0.1741278,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;7;998.4246,298.6577;Inherit;False;30;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;961.1624,608.6425;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-346.6716,-844.2674;Inherit;False;Constant;_fresnel_Power;fresnel_Power;9;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;10;1327.211,462.729;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;364.4432,2217.399;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;333.7021,368.1698;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1245.204,-961.6392;Inherit;False;Property;_ActiveInvisibility;Active Invisibility;2;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;71;-984.205,273.724;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-465.1121,1403.561;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-762.9711,1388.877;Inherit;False;Property;_Emission;Emission;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;1487.489,460.3867;Inherit;False;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-557.2277,1209.62;Inherit;False;Property;_Oppa;Oppa;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;1164.451,1879.97;Inherit;False;Constant;_TimeFloat;TimeFloat;0;0;Create;True;0;0;False;0;3;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-547.4396,-404.5863;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-332.1356,-952.0491;Inherit;False;Constant;_fresnel_Scale;fresnel_Scale;8;0;Create;True;0;0;False;0;1.540285;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;379.9095,2089.126;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;86;-852.2109,1083.455;Inherit;True;Property;_ToggleSwitch0;Toggle Switch0;4;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;38;856.0436,1973.48;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;70;-1161.474,-731.2208;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;66;-1584.146,-334.2541;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;82;-553.1768,571.716;Inherit;True;Property;_isMoveAvailable;isMoveAvailable;3;0;Create;True;0;0;False;0;0;0;0;True;_isMoveAvailable;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NegateNode;65;-1420.259,-597.7181;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;54;-1.748671,-951.0872;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RefractOpVec;67;-1187.803,-352.7987;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;375.7801,226.7896;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;9;1192.875,450.9577;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;64;-1650.752,-659.9507;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;340.4749,471.8739;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;0.5537913;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-183.304,502.0342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TopPartNode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;32;0
WireConnection;34;0;33;0
WireConnection;39;0;34;0
WireConnection;39;1;37;0
WireConnection;40;0;39;0
WireConnection;25;2;23;0
WireConnection;25;3;24;0
WireConnection;41;0;40;0
WireConnection;27;0;25;0
WireConnection;28;0;27;0
WireConnection;28;1;26;0
WireConnection;29;0;28;0
WireConnection;30;0;29;0
WireConnection;53;1;67;0
WireConnection;56;0;54;0
WireConnection;6;0;2;2
WireConnection;6;1;4;0
WireConnection;6;2;3;0
WireConnection;8;0;6;0
WireConnection;8;1;5;0
WireConnection;10;0;9;0
WireConnection;71;0;74;0
WireConnection;81;0;50;0
WireConnection;11;0;10;0
WireConnection;68;0;69;0
WireConnection;68;1;53;0
WireConnection;38;0;34;0
WireConnection;38;1;48;0
WireConnection;38;2;35;0
WireConnection;70;0;69;0
WireConnection;82;0;46;0
WireConnection;65;0;64;0
WireConnection;54;2;58;0
WireConnection;54;3;59;0
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;67;2;70;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;0;0;71;0
WireConnection;0;2;82;0
ASEEND*/
//CHKSM=359C5A485C4C1B9D25BAB8F416ABF3D07B5EE9C7