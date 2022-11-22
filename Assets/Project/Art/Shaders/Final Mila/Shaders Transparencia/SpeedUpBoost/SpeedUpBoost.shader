// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpeedUpBoost"
{
	Properties
	{
		_Offset("Offset", Range( 0 , 10)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TilingFlechas("Tiling Flechas", Vector) = (0,0,0,0)
		_Speed("Speed", Float) = 8
		_TilingMascaraHorizontal("Tiling Mascara Horizontal", Float) = 20
		_Horizontal1("Horizontal1", Float) = 8.6
		_TilingMascaraVertical("Tiling Mascara Vertical", Float) = 20
		_Vertical1("Vertical1", Float) = 8.6
		_Vertical2("Vertical2", Float) = 15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float _Speed;
		uniform float2 _TilingFlechas;
		uniform float _Offset;
		uniform float _TilingMascaraVertical;
		uniform float _Vertical1;
		uniform float _Vertical2;
		uniform float _TilingMascaraHorizontal;
		uniform float _Horizontal1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color4 = IsGammaSpace() ? float4(4,2.447059,0,0) : float4(21.11213,7.161727,0,0);
			float Tiempo26 = ( _Time.y * _Speed );
			float2 temp_cast_0 = (_Offset).xx;
			float2 uv_TexCoord2 = i.uv_texcoord * _TilingFlechas + temp_cast_0;
			float2 panner73 = ( Tiempo26 * float2( 0,-1 ) + uv_TexCoord2);
			o.Emission = ( color4 * ( 1.0 - tex2D( _TextureSample0, panner73 ) ) ).rgb;
			float2 temp_cast_2 = (_TilingMascaraVertical).xx;
			float2 uv_TexCoord49 = i.uv_texcoord * temp_cast_2;
			float2 temp_cast_3 = (_TilingMascaraVertical).xx;
			float2 uv_TexCoord48 = i.uv_texcoord * temp_cast_3;
			float MascaraVertical75 = step( ( saturate( sin( ( uv_TexCoord49.x * _Vertical1 ) ) ) + saturate( sin( ( uv_TexCoord48.x * _Vertical2 ) ) ) ) , 0.0 );
			float2 temp_cast_4 = (_TilingMascaraHorizontal).xx;
			float2 uv_TexCoord36 = i.uv_texcoord * temp_cast_4;
			float MacaraHorizontal74 = step( ( saturate( sin( ( uv_TexCoord36.y * _Horizontal1 ) ) ) + 0.0 ) , 0.0 );
			o.Alpha = saturate( ( ( MascaraVertical75 - MacaraHorizontal74 ) / 0.0 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
59;405;1807;702;6044.438;309.7199;4.675293;True;True
Node;AmplifyShaderEditor.RangedFloatNode;63;-3153.46,2395.386;Inherit;False;Property;_TilingMascaraVertical;Tiling Mascara Vertical;7;0;Create;True;0;0;False;0;20;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-3297.242,1251.522;Inherit;False;Property;_TilingMascaraHorizontal;Tiling Mascara Horizontal;4;0;Create;True;0;0;False;0;20;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2904.745,1164.148;Inherit;False;Property;_Horizontal1;Horizontal1;5;0;Create;True;0;0;False;0;8.6;4.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-2884.941,2023.442;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-2833.749,2283.135;Inherit;False;Property;_Vertical1;Vertical1;8;0;Create;True;0;0;False;0;8.6;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2965.76,901.5916;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-2818.766,2771.701;Inherit;False;Property;_Vertical2;Vertical2;9;0;Create;True;0;0;False;0;15;1.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-2878.675,2513.969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-2628.091,2145.977;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2682.281,1066.193;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-2617.589,2645.131;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;22;-3175.036,-670.178;Inherit;False;682.7109;531.5023;Tiempo;4;26;25;24;23;Tiempo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;54;-2421.769,2644.202;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;-2473.127,1063.642;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;55;-2419.033,2143.67;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;-2257.913,2142.968;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;38;-2299.369,1062.273;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;56;-2264.811,2642.383;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-3125.036,-620.178;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3146.58,-396.676;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;False;0;8;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-2066.145,1326.615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2937.752,-526.014;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2012.097,2418.062;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1704.426,-309.2385;Inherit;False;Property;_Offset;Offset;0;0;Create;True;0;0;False;0;0;4.111765;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-1806.935,2419.477;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-1696.793,-573.6818;Inherit;False;Property;_TilingFlechas;Tiling Flechas;2;0;Create;True;0;0;False;0;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-2735.326,-538.0441;Inherit;False;Tiempo;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;68;-1849.746,1329.061;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;-1631.66,1321.181;Inherit;False;MacaraHorizontal;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;-1593.465,2421.696;Inherit;False;MascaraVertical;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1407.024,-366.0956;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;70;-1319.419,-106.5574;Inherit;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;0,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;97;-1337.978,166.753;Inherit;False;26;Tiempo;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;-904.1063,281.2763;Inherit;False;74;MacaraHorizontal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;73;-1067.973,-177.1479;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-907.5179,473.1574;Inherit;False;75;MascaraVertical;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;98;-634.5563,353.5704;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-805.522,-199.7065;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;ed52d5bf2ba2dc94cb50dc990e8d64c4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-429.4629,-267.4067;Inherit;False;Constant;_Color0;Color 0;1;1;[HDR];Create;True;0;0;False;0;4,2.447059,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;101;-263.3091,241.4026;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-381.5769,-83.23029;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-2950.445,1385.291;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-2895.152,1642.125;Inherit;False;Property;_Horizontal2;Horizontal2;6;0;Create;True;0;0;False;0;15;10.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;39;-2459.177,1565.241;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;60;11.74739,231.1624;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;43;-2273.443,1566.88;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2668.872,1561.339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-154.1186,-117.3443;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;195.0205,18.92517;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SpeedUpBoost;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;49;0;63;0
WireConnection;36;0;62;0
WireConnection;48;0;63;0
WireConnection;52;0;49;1
WireConnection;52;1;51;0
WireConnection;37;0;36;2
WireConnection;37;1;35;0
WireConnection;53;0;48;1
WireConnection;53;1;50;0
WireConnection;54;0;53;0
WireConnection;34;0;37;0
WireConnection;55;0;52;0
WireConnection;57;0;55;0
WireConnection;38;0;34;0
WireConnection;56;0;54;0
WireConnection;44;0;38;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;58;0;57;0
WireConnection;58;1;56;0
WireConnection;67;0;58;0
WireConnection;26;0;25;0
WireConnection;68;0;44;0
WireConnection;74;0;68;0
WireConnection;75;0;67;0
WireConnection;2;0;11;0
WireConnection;2;1;9;0
WireConnection;73;0;2;0
WireConnection;73;2;70;0
WireConnection;73;1;97;0
WireConnection;98;0;77;0
WireConnection;98;1;76;0
WireConnection;1;1;73;0
WireConnection;101;0;98;0
WireConnection;8;0;1;0
WireConnection;42;0;62;0
WireConnection;39;0;41;0
WireConnection;60;0;101;0
WireConnection;43;0;39;0
WireConnection;41;0;42;2
WireConnection;41;1;40;0
WireConnection;3;0;4;0
WireConnection;3;1;8;0
WireConnection;0;2;3;0
WireConnection;0;9;60;0
ASEEND*/
//CHKSM=483249CD9348396AE88CA9164603112D4BA2B198