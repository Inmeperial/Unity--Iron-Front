// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CanAttackToNode Shader"
{
	Properties
	{
		[HDR]_Color2("Color 0", Color) = (1,0,0.068398,0)
		_MinBright2("MinBright", Range( 0 , 1)) = 0.2
		_MaxBright2("MaxBright", Range( 0 , 1)) = 0.9
		[IntRange]_TimeCicle2("TimeCicle", Range( 0 , 10)) = 4
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			half filler;
		};

		uniform float4 _Color2;
		uniform float _TimeCicle2;
		uniform float _MinBright2;
		uniform float _MaxBright2;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime18 = _Time.y * _TimeCicle2;
			float clampResult23 = clamp( sin( mulTime18 ) , _MinBright2 , _MaxBright2 );
			o.Emission = saturate( ( _Color2 * clampResult23 ) ).rgb;
			o.Alpha = saturate( clampResult23 );
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
				float3 worldPos : TEXCOORD1;
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
0;73;847;938;1952.977;633.1706;2.539537;True;False
Node;AmplifyShaderEditor.CommentaryNode;16;-1189.409,147.4657;Inherit;False;970.2416;488.4174;ClampWithTime;6;23;21;20;19;18;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1169.13,207.2212;Inherit;False;Property;_TimeCicle2;TimeCicle;3;1;[IntRange];Create;True;0;0;False;0;4;8;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;18;-929.8516,304.3343;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;19;-793.1926,208.8799;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1048.281,396.3788;Inherit;False;Property;_MinBright2;MinBright;1;0;Create;True;0;0;False;0;0.2;0.178;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1053.038,504.3302;Inherit;False;Property;_MaxBright2;MaxBright;2;0;Create;True;0;0;False;0;0.9;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;23;-589.7596,360.5338;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-592.4016,-289.1425;Inherit;False;Property;_Color2;Color 0;0;1;[HDR];Create;True;0;0;False;0;1,0,0.068398,0;0,1,0.9917969,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-314.7381,-118.3521;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;25;-49.06836,65.40969;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;26;-121.0329,262.5723;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;105.0602,-8.518398;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CanAttackToNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;17;0
WireConnection;19;0;18;0
WireConnection;23;0;19;0
WireConnection;23;1;20;0
WireConnection;23;2;21;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;25;0;24;0
WireConnection;26;0;23;0
WireConnection;0;2;25;0
WireConnection;0;9;26;0
ASEEND*/
//CHKSM=9710AA8FEB8CCB2E97A9CEF0D72E4F856067C1F3