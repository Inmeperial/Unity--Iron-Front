// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CoverForMecha Shader"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_ColorShieldInner("ColorShieldInner", Color) = (1,1,1,0)
		_ColorShieldOut("ColorShieldOut", Color) = (0,0,0,0)
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_halfShield("halfShield", Int) = 5
		_fullShield("fullShield", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorShieldInner;
		uniform int _halfShield;
		uniform int _fullShield;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _ColorShieldOut;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_53_0 = ( 0.5 * _halfShield );
			float ifLocalVar45 = 0;
			if( temp_output_53_0 <= _fullShield )
				ifLocalVar45 = (float)_fullShield;
			else
				ifLocalVar45 = temp_output_53_0;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 color5 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 temp_output_4_0 = ( tex2D( _TextureSample0, uv_TextureSample0 ).a * color5 );
			float4 temp_output_9_0 = ( step( i.uv_texcoord.y , ifLocalVar45 ) * temp_output_4_0 );
			o.Emission = saturate( ( ( _ColorShieldInner * temp_output_9_0 ) + ( ( 1.0 - temp_output_9_0 ) * _ColorShieldOut ) ) ).rgb;
			o.Alpha = ( temp_output_4_0 * _Opacity ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
-1756;43;1449;883;2198.051;1617.666;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;54;-1688.842,-1047.55;Inherit;False;Constant;_Float6;Float 0;7;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;41;-1682.799,-945.3852;Inherit;False;Property;_halfShield;halfShield;4;0;Create;True;0;0;False;0;5;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1498.373,-1001.895;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;43;-1657.651,-742.6088;Inherit;False;Property;_fullShield;fullShield;6;0;Create;True;0;0;False;0;0;1;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1280.745,-253.5429;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;beaaf2baa0ba22b4fb4cb47297dd9033;beaaf2baa0ba22b4fb4cb47297dd9033;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1259.285,-1198.623;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;45;-1311.599,-836.385;Inherit;True;False;5;0;FLOAT;0;False;1;INT;0;False;2;FLOAT;0;False;3;INT;0;False;4;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1199.494,-44.44584;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,0;0.9056604,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-954.6093,-105.3094;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;8;-1013.532,-930.4155;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-735.5894,-827.3821;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;31;-608.512,-522.9067;Inherit;False;Property;_ColorShieldOut;ColorShieldOut;2;0;Create;True;0;0;False;0;0,0,0,0;0.360048,0.8773585,0.8274467,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;34;-476.3596,-717.5881;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;10;-686.0004,-1181.755;Inherit;False;Property;_ColorShieldInner;ColorShieldInner;1;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-357.2381,-574.2972;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-380.4537,-1005.422;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1065.192,178.4523;Inherit;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-147.2675,-726.7458;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;46;-1787.845,-1681.148;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-761.3885,-62.93153;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-2192.625,-1553.731;Inherit;False;Constant;_Float4;Float 0;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;49;-2201.045,-1740.148;Inherit;False;Property;_halfShield2;halfShield;5;0;Create;True;0;0;False;0;3;1;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;50;-2194.045,-1434.148;Inherit;False;Property;_fullShield2;fullShield;7;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-2004.045,-1510.148;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;126.9346,-520.719;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1998.045,-1777.148;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2203.514,-1840.803;Inherit;False;Constant;_Float5;Float 0;7;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;301.9207,-226.2475;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CoverForMecha Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;53;0;54;0
WireConnection;53;1;41;0
WireConnection;45;0;53;0
WireConnection;45;1;43;0
WireConnection;45;2;53;0
WireConnection;45;3;43;0
WireConnection;45;4;43;0
WireConnection;4;0;1;4
WireConnection;4;1;5;0
WireConnection;8;0;7;2
WireConnection;8;1;45;0
WireConnection;9;0;8;0
WireConnection;9;1;4;0
WireConnection;34;0;9;0
WireConnection;35;0;34;0
WireConnection;35;1;31;0
WireConnection;20;0;10;0
WireConnection;20;1;9;0
WireConnection;30;0;20;0
WireConnection;30;1;35;0
WireConnection;46;0;51;0
WireConnection;46;1;52;0
WireConnection;46;2;48;0
WireConnection;46;3;47;0
WireConnection;46;4;47;0
WireConnection;3;0;4;0
WireConnection;3;1;2;0
WireConnection;52;0;47;0
WireConnection;52;1;50;0
WireConnection;26;0;30;0
WireConnection;51;0;48;0
WireConnection;51;1;49;0
WireConnection;0;2;26;0
WireConnection;0;9;3;0
ASEEND*/
//CHKSM=77040EA048FD4F2AC0BD58760BBBEF4DBEA075B8