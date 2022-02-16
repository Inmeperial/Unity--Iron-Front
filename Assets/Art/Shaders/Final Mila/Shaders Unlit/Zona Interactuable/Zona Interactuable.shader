// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "p"
{
	Properties
	{
		_Float1("Float 1", Float) = 0
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
			half ASEVFace : VFACE;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
			float2 uv_texcoord;
		};

		uniform float _Float1;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color1 = IsGammaSpace() ? float4(4,0.6494153,0,0) : float4(21.11213,0.3793004,0,0);
			float4 color26 = IsGammaSpace() ? float4(4,0.5609062,0,0) : float4(21.11213,0.2748078,0,0);
			float4 switchResult25 = (((i.ASEVFace>0)?(color1):(color26)));
			float4 Color41 = switchResult25;
			o.Emission = Color41.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV17 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode17 = ( 0.62 + 1.0 * pow( 1.0 - fresnelNdotV17, 5.0 ) );
			float fresnelNdotV16 = dot( ase_worldNormal, -i.viewDir );
			float fresnelNode16 = ( 0.1 + 1.0 * pow( max( 1.0 - fresnelNdotV16 , 0.0001 ), 5.55 ) );
			float Fresnel45 = ( fresnelNode17 * fresnelNode16 );
			float mulTime32 = _Time.y * 0.54;
			float2 temp_cast_1 = (-0.21).xx;
			float2 temp_cast_2 = (2.25).xx;
			float2 uv_TexCoord28 = i.uv_texcoord * temp_cast_2;
			float2 temp_cast_3 = (uv_TexCoord28.y).xx;
			float2 panner30 = ( mulTime32 * temp_cast_1 + temp_cast_3);
			float simplePerlin2D33 = snoise( panner30*20.0 );
			simplePerlin2D33 = simplePerlin2D33*0.5 + 0.5;
			float Franjas48 = saturate( (0.0 + (step( simplePerlin2D33 , 0.24 ) - 0.0) * (0.74 - 0.0) / (2.26 - 0.0)) );
			float4 color36 = IsGammaSpace() ? float4(85.44498,85.44498,85.44498,0) : float4(17770.94,17770.94,17770.94,0);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float MascaraAltura51 = saturate( ( ( 1.0 - ase_vertex3Pos.z ) - _Float1 ) );
			o.Alpha = saturate( ( ( Fresnel45 * Franjas48 * color36 * MascaraAltura51 ) + float4( 0,0,0,0 ) ) ).r;
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
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
78;913;1807;626;2404.85;599.7831;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;49;-4478.68,-676.6838;Inherit;False;2027.938;833.5294;Franjas;12;27;28;30;33;35;38;39;48;34;29;32;31;Franjas;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;27;-4428.68,-626.6837;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0,2.25;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;31;-4270.375,-101.1545;Inherit;False;Constant;_Float4;Float 4;2;0;Create;True;0;0;False;0;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-4245.436,-596.4453;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-4224.313,-327.1075;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;-0.21;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-4101.35,-97.437;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-2410.337,-686.1657;Inherit;False;1250.034;548.2581;Fresnel;6;9;14;16;17;20;45;Fresnel;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;30;-3936.481,-567.2894;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3861.886,-345.0443;Inherit;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;53;-2421.403,-39.66334;Inherit;False;1166.876;609.4437;MascaraAltura;6;5;8;11;13;19;51;MascaraAltura;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;9;-2360.337,-395.7134;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;5;-2371.403,10.33665;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;33;-3624.955,-542.9825;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2123.362,311.7803;Inherit;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;0;0.97;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;14;-2127.236,-390.9077;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;35;-3383.976,-537.0116;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-2139.644,79.33393;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;17;-1942.334,-636.1657;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.62;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;16;-1944.836,-413.3878;Inherit;False;Standard;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.1;False;2;FLOAT;1;False;3;FLOAT;5.55;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-3153.991,-545.2297;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;2.26;False;3;FLOAT;0;False;4;FLOAT;0.74;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1932.512,241.8056;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;39;-2870.759,-546.4156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1618.494,-488.9726;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;-1681.22,246.2578;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-2693.742,-544.5447;Inherit;False;Franjas;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;42;-1102.851,-664.9796;Inherit;False;785.0771;430.8222;Color;4;26;1;25;41;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1500.527,249.1407;Inherit;False;MascaraAltura;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-1403.303,-486.1979;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-753.5068,525.4588;Inherit;False;51;MascaraAltura;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;-804.6825,351.6072;Inherit;False;Constant;_Color6;Color 6;0;1;[HDR];Create;True;0;0;False;0;85.44498,85.44498,85.44498,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;-1048.906,-441.1577;Inherit;False;Constant;_Color1;Color 1;0;1;[HDR];Create;True;0;0;False;0;4,0.5609062,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;50;-737.6115,155.5443;Inherit;False;48;Franjas;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-742.581,-35.87748;Inherit;False;45;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1052.851,-614.9796;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;4,0.6494153,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-474.7729,252.8723;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SwitchByFaceNode;25;-804.2324,-503.239;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-220.3411,252.4131;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-494.2955,-515.6024;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-35.61274,15.83229;Inherit;False;41;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;24;-16.69664,250.5374;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;171.7267,44.60433;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;p;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;27;2
WireConnection;32;0;31;0
WireConnection;30;0;28;2
WireConnection;30;2;29;0
WireConnection;30;1;32;0
WireConnection;33;0;30;0
WireConnection;33;1;34;0
WireConnection;14;0;9;0
WireConnection;35;0;33;0
WireConnection;11;0;5;3
WireConnection;16;4;14;0
WireConnection;38;0;35;0
WireConnection;13;0;11;0
WireConnection;13;1;8;0
WireConnection;39;0;38;0
WireConnection;20;0;17;0
WireConnection;20;1;16;0
WireConnection;19;0;13;0
WireConnection;48;0;39;0
WireConnection;51;0;19;0
WireConnection;45;0;20;0
WireConnection;21;0;47;0
WireConnection;21;1;50;0
WireConnection;21;2;36;0
WireConnection;21;3;52;0
WireConnection;25;0;1;0
WireConnection;25;1;26;0
WireConnection;23;0;21;0
WireConnection;41;0;25;0
WireConnection;24;0;23;0
WireConnection;0;2;43;0
WireConnection;0;9;24;0
ASEEND*/
//CHKSM=51D36B4825689E965135E7D7D59222C18CDBDAF1