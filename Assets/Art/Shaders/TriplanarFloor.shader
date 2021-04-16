// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplanar"
{
	Properties
	{
		_Terrain("Terrain", 2D) = "white" {}
		_Grass("Grass", 2D) = "white" {}
		_ColorTile("ColorTile", Color) = (0,0.9800038,1,0)
		_Color0("Color 0", Color) = (0.8584906,0.8415912,0.1741278,0)
		[Toggle(_ISFRESNELON_ON)] _IsFresnelOn("IsFresnelOn", Float) = 0
		[Toggle(_ISSELECTED_ON)] _IsSelected("IsSelected", Float) = 0
		[Toggle(_ACTIVATESMOOT_ON)] _ActivateSmoot("ActivateSmoot", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ACTIVATESMOOT_ON
		#pragma shader_feature_local _ISSELECTED_ON
		#pragma shader_feature_local _ISFRESNELON_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Grass;
		uniform float4 _Grass_ST;
		uniform sampler2D _Terrain;
		uniform float4 _Terrain_ST;
		uniform float4 _ColorTile;
		uniform float4 _Color0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Grass = i.uv_texcoord * _Grass_ST.xy + _Grass_ST.zw;
			float2 uv_Terrain = i.uv_texcoord * _Terrain_ST.xy + _Terrain_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 temp_cast_0 = (-0.56).xxx;
			float3 temp_cast_1 = (2.0).xxx;
			float3 smoothstepResult112 = smoothstep( temp_cast_0 , temp_cast_1 , ase_worldPos);
			#ifdef _ACTIVATESMOOT_ON
				float3 staticSwitch126 = ( 1.0 - smoothstepResult112 );
			#else
				float3 staticSwitch126 = ( 1.0 - saturate( ase_worldPos ) );
			#endif
			float4 lerpResult20 = lerp( tex2D( _Grass, uv_Grass ) , tex2D( _Terrain, uv_Terrain ) , float4( staticSwitch126 , 0.0 ));
			float4 TriPlanar117 = saturate( lerpResult20 );
			o.Albedo = TriPlanar117.rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV44 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode44 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV44, 0.7929211 ) );
			float mulTime121 = _Time.y * (float)3;
			float clampResult50 = clamp( sin( mulTime121 ) , 0.1 , 0.7 );
			float4 ClampWithColorVar70 = saturate( ( clampResult50 * _ColorTile ) );
			float4 CanMoveToNodeVar109 = saturate( ( ( 1.0 - fresnelNode44 ) * ClampWithColorVar70 ) );
			#ifdef _ISFRESNELON_ON
				float4 staticSwitch97 = CanMoveToNodeVar109;
			#else
				float4 staticSwitch97 = float4( 0,0,0,0 );
			#endif
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float clampResult81 = clamp( ase_vertex3Pos.y , 0.0 , 0.5537913 );
			float4 SelectedNodeVar107 = saturate( ( CanMoveToNodeVar109 + ( clampResult81 * _Color0 ) ) );
			#ifdef _ISSELECTED_ON
				float4 staticSwitch100 = SelectedNodeVar107;
			#else
				float4 staticSwitch100 = staticSwitch97;
			#endif
			o.Emission = staticSwitch100.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
-964;73;685;486;-2299.987;-1640.846;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;102;2095.685,-955.5158;Inherit;False;1252.094;722.976;ClampWithTime;11;70;46;58;50;57;74;120;121;122;123;127;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;127;2172.771,-781.4229;Inherit;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;3;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleTimeNode;121;2251.889,-887.4729;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;122;2440.809,-876.3397;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;2184.485,-559.2554;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;2138.505,-680.4822;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.1;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;2525.032,-439.6795;Inherit;False;Property;_ColorTile;ColorTile;4;0;Create;True;0;0;False;0;0,0.9800038,1,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;50;2499.965,-726.0444;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;2780.176,-679.7437;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;105;1998.935,-127.7943;Inherit;False;1453.002;484.403;CanMoveToNode;8;109;45;96;71;44;72;61;99;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;61;2025.043,-19.62069;Inherit;False;Constant;_Float8;Float 8;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;2027.282,83.97035;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7929211;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;120;2930.06,-660.3479;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;44;2352.379,-68.35141;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;3087.259,-669.5693;Inherit;False;ClampWithColorVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;2386.322,206.6451;Inherit;False;70;ClampWithColorVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;96;2598.796,1.279277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;2803.582,23.72532;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;101;2042.928,535.0864;Inherit;False;1479.932;757.176;SelectedNode;10;107;119;80;79;111;81;78;76;83;82;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;116;2043.708,1369.1;Inherit;False;1295.604;1025.629;TriPlanar;13;112;28;114;20;17;18;43;29;27;117;124;125;126;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;76;2138.566,611.4753;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;2103.261,856.5595;Inherit;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;0.5537913;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;3026.195,26.38875;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;125;2092.116,2166.402;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;2096.488,752.8555;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;2072.542,2005.452;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;114;2084.807,2278.177;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;False;0;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;2276.294,1910.949;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;78;2136.303,979.1769;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;0.8584906,0.8415912,0.1741278,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;112;2470.315,2036.179;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;3180.07,29.06874;Inherit;False;CanMoveToNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;81;2460.045,744.4105;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;2761.209,683.3433;Inherit;False;109;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2723.947,993.3281;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;43;2472.298,1853.133;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;124;2669.386,1925.519;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;126;2735.632,1731.833;Inherit;False;Property;_ActivateSmoot;ActivateSmoot;9;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;17;2147.854,1674.108;Inherit;True;Property;_Terrain;Terrain;0;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;80;2955.659,835.6434;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;2120.227,1426.785;Inherit;True;Property;_Grass;Grass;2;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;20;2626.417,1462.45;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;119;3089.995,847.4147;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;3250.273,845.0724;Inherit;False;SelectedNodeVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;28;2890.592,1482.81;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;487.8328,196.754;Inherit;False;109;CanMoveToNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;3053.112,1462.686;Inherit;False;TriPlanar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;97;784.1664,171.4008;Inherit;False;Property;_IsFresnelOn;IsFresnelOn;6;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;790.5827,387.777;Inherit;False;107;SelectedNodeVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;100;1135.628,165.1497;Inherit;False;Property;_IsSelected;IsSelected;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;1355.431,77.11185;Inherit;False;117;TriPlanar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;34;3775.824,-723.141;Inherit;True;Property;_Alpha1;Alpha1;3;0;Create;True;0;0;False;0;-1;41c9f08c66bb48d4c8f7e06583c9b701;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;32;4063.704,-18.28214;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;31;3834.783,35.61093;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;33;3776.502,-489.5875;Inherit;True;Property;_Alpha2;Alpha2;1;0;Create;True;0;0;False;0;-1;939aa491bfac0da49bb09562b9498276;939aa491bfac0da49bb09562b9498276;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;123;2638.206,-898.8562;Inherit;False;Property;_TimeFloat;TimeFloat;8;0;Create;True;0;0;False;0;3;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;4202.818,-354.7608;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;38;3974.876,-199.8871;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1583.322,108.9526;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;121;0;127;0
WireConnection;122;0;121;0
WireConnection;50;0;122;0
WireConnection;50;1;57;0
WireConnection;50;2;74;0
WireConnection;58;0;50;0
WireConnection;58;1;46;0
WireConnection;120;0;58;0
WireConnection;44;2;61;0
WireConnection;44;3;72;0
WireConnection;70;0;120;0
WireConnection;96;0;44;0
WireConnection;45;0;96;0
WireConnection;45;1;71;0
WireConnection;99;0;45;0
WireConnection;29;0;27;0
WireConnection;112;0;27;0
WireConnection;112;1;125;0
WireConnection;112;2;114;0
WireConnection;109;0;99;0
WireConnection;81;0;76;2
WireConnection;81;1;82;0
WireConnection;81;2;83;0
WireConnection;79;0;81;0
WireConnection;79;1;78;0
WireConnection;43;0;29;0
WireConnection;124;0;112;0
WireConnection;126;1;43;0
WireConnection;126;0;124;0
WireConnection;80;0;111;0
WireConnection;80;1;79;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;20;2;126;0
WireConnection;119;0;80;0
WireConnection;107;0;119;0
WireConnection;28;0;20;0
WireConnection;117;0;28;0
WireConnection;97;0;110;0
WireConnection;100;1;97;0
WireConnection;100;0;108;0
WireConnection;32;0;31;0
WireConnection;35;0;34;0
WireConnection;35;1;33;0
WireConnection;35;2;38;0
WireConnection;38;0;32;0
WireConnection;0;0;118;0
WireConnection;0;2;100;0
ASEEND*/
//CHKSM=E6D9F511ED75B8520F646E4D37EEB844D86DBE84