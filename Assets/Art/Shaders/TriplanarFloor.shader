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
			float4 lerpResult20 = lerp( tex2D( _Grass, uv_Grass ) , tex2D( _Terrain, uv_Terrain ) , float4( ( 1.0 - saturate( ase_worldPos ) ) , 0.0 ));
			o.Albedo = saturate( lerpResult20 ).rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV44 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode44 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV44, 5.0 ) );
			float clampResult50 = clamp( _SinTime.w , 0.1 , 0.7 );
			float4 ClampWithColorVar70 = ( clampResult50 * _ColorTile );
			float4 temp_output_45_0 = ( ( 1.0 - fresnelNode44 ) * ClampWithColorVar70 );
			#ifdef _ISFRESNELON_ON
				float4 staticSwitch97 = temp_output_45_0;
			#else
				float4 staticSwitch97 = float4( 0,0,0,0 );
			#endif
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float clampResult81 = clamp( ase_vertex3Pos.y , 0.0 , 0.5537913 );
			#ifdef _ISSELECTED_ON
				float4 staticSwitch100 = ( temp_output_45_0 + ( clampResult81 * _Color0 ) );
			#else
				float4 staticSwitch100 = staticSwitch97;
			#endif
			o.Emission = saturate( staticSwitch100 ).rgb;
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
629;73;799;666;227.9084;263.8036;1.826458;True;False
Node;AmplifyShaderEditor.RangedFloatNode;57;1843.286,-557.8342;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;0.1;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;1889.267,-436.6075;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;0.7;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;56;1871.29,-814.9021;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;50;2204.747,-603.3965;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;2239.814,-286.5209;Inherit;False;Property;_ColorTile;ColorTile;4;0;Create;True;0;0;False;0;0,0.9800038,1,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-377.0309,113.9845;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.7929211;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;2535.524,-376.5205;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-379.2699,10.39347;Inherit;False;Constant;_Float8;Float 8;5;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-177.8736,719.1243;Inherit;False;Constant;_Float4;Float 4;8;0;Create;True;0;0;False;0;0.5537913;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;76;-142.5686,474.0406;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;-184.6461,615.4206;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;44;-51.93264,-38.33724;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;2759.861,-379.7195;Inherit;False;ClampWithColorVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;78;-132.5221,872.5153;Inherit;False;Property;_Color0;Color 0;5;0;Create;True;0;0;False;0;0.8584906,0.8415912,0.1741278,0;0,0.9800038,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;27;-85.93944,-317.3994;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;81;178.9107,606.9757;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;96;194.4839,31.29343;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-17.98992,236.6593;Inherit;False;70;ClampWithColorVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;29;193.4117,-300.5327;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;385.5746,78.21577;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;755.0632,697.7393;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;853.8925,338.593;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;97;707.2811,77.68143;Inherit;False;Property;_IsFresnelOn;IsFresnelOn;6;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;17;136.9262,-567.8671;Inherit;True;Property;_Terrain;Terrain;0;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;125.3691,-821.6184;Inherit;True;Property;_Grass;Grass;2;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;43;390.3849,-306.7808;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;100;1006.778,174.5464;Inherit;False;Property;_IsSelected;IsSelected;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;644.0551,-461.7431;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;28;1311.871,-67.75497;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;35;2351.482,677.3082;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;32;2212.368,1013.787;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;31;1983.447,1067.68;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCGrayscale;38;2123.54,832.1819;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;99;1302.664,117.9262;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;34;1924.488,308.9281;Inherit;True;Property;_Alpha1;Alpha1;3;0;Create;True;0;0;False;0;-1;41c9f08c66bb48d4c8f7e06583c9b701;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;1925.166,542.4815;Inherit;True;Property;_Alpha2;Alpha2;1;0;Create;True;0;0;False;0;-1;939aa491bfac0da49bb09562b9498276;939aa491bfac0da49bb09562b9498276;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1583.322,108.9526;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;56;4
WireConnection;50;1;57;0
WireConnection;50;2;74;0
WireConnection;58;0;50;0
WireConnection;58;1;46;0
WireConnection;44;2;61;0
WireConnection;44;3;72;0
WireConnection;70;0;58;0
WireConnection;81;0;76;2
WireConnection;81;1;82;0
WireConnection;81;2;83;0
WireConnection;96;0;44;0
WireConnection;29;0;27;0
WireConnection;45;0;96;0
WireConnection;45;1;71;0
WireConnection;79;0;81;0
WireConnection;79;1;78;0
WireConnection;80;0;45;0
WireConnection;80;1;79;0
WireConnection;97;0;45;0
WireConnection;43;0;29;0
WireConnection;100;1;97;0
WireConnection;100;0;80;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;20;2;43;0
WireConnection;28;0;20;0
WireConnection;35;0;34;0
WireConnection;35;1;33;0
WireConnection;35;2;38;0
WireConnection;32;0;31;0
WireConnection;38;0;32;0
WireConnection;99;0;100;0
WireConnection;0;0;28;0
WireConnection;0;2;99;0
ASEEND*/
//CHKSM=BD7FF8718E65B1EF22847017121A9C762310A0C6