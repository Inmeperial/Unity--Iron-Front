// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MasterShader"
{
	Properties
	{
		_TextureAlbedo("TextureAlbedo", 2D) = "white" {}
		_TextureNormal("TextureNormal", 2D) = "bump" {}
		_TextureEmission("TextureEmission", 2D) = "white" {}
		_MaskAlbedo("MaskAlbedo", 2D) = "white" {}
		[Toggle]_isWeapon("isWeapon", Float) = 0
		_ColorAlbedo("ColorAlbedo", Color) = (1,1,1,0)
		[Toggle]_isOutLineOn("isOutLineOn", Float) = 0
		[Toggle]_IsEmissionON("IsEmissionON", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = (( _isOutLineOn )?( 0.05 ):( (float)0 ));
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float4 color34 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			o.Emission = color34.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#pragma shader_feature_local _SwitchTexture_Key0 _SwitchTexture_Key1 _SwitchTexture_Key2
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _TextureNormal;
		uniform float4 _TextureNormal_ST;
		uniform float _isWeapon;
		uniform float4 _ColorAlbedo;
		uniform sampler2D _TextureAlbedo;
		uniform float4 _TextureAlbedo_ST;
		uniform sampler2D _MaskAlbedo;
		uniform float4 _MaskAlbedo_ST;
		uniform float _IsEmissionON;
		uniform sampler2D _TextureEmission;
		uniform float4 _TextureEmission_ST;
		uniform float _isOutLineOn;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 OutLineVar40 = 0;
			v.vertex.xyz += OutLineVar40;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureNormal = i.uv_texcoord * _TextureNormal_ST.xy + _TextureNormal_ST.zw;
			float3 NormalVar38 = UnpackScaleNormal( tex2D( _TextureNormal, uv_TextureNormal ), 2.28 );
			o.Normal = NormalVar38;
			float2 uv_TextureAlbedo = i.uv_texcoord * _TextureAlbedo_ST.xy + _TextureAlbedo_ST.zw;
			float4 tex2DNode9 = tex2D( _TextureAlbedo, uv_TextureAlbedo );
			float2 uv_MaskAlbedo = i.uv_texcoord * _MaskAlbedo_ST.xy + _MaskAlbedo_ST.zw;
			float4 color22 = IsGammaSpace() ? float4(0.7830189,0.304542,0.1514329,0) : float4(0.5754442,0.07550805,0.01993717,0);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV6 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode6 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV6, 1.0 ) );
			float4 color7 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float mulTime3 = _Time.y * (float)10;
			float clampResult10 = clamp( sin( mulTime3 ) , 0.0 , 0.4 );
			#if defined(_SwitchTexture_Key0)
				float4 staticSwitch20 = (( _isWeapon )?( tex2DNode9 ):( ( ( _ColorAlbedo * tex2DNode9 ) + ( saturate( ( tex2DNode9 - tex2D( _MaskAlbedo, uv_MaskAlbedo ) ) ) * color22 ) ) ));
			#elif defined(_SwitchTexture_Key1)
				float4 staticSwitch20 = ( (( _isWeapon )?( tex2DNode9 ):( ( ( _ColorAlbedo * tex2DNode9 ) + ( saturate( ( tex2DNode9 - tex2D( _MaskAlbedo, uv_MaskAlbedo ) ) ) * color22 ) ) )) + ( fresnelNode6 * color7 ) );
			#elif defined(_SwitchTexture_Key2)
				float4 staticSwitch20 = ( (( _isWeapon )?( tex2DNode9 ):( ( ( _ColorAlbedo * tex2DNode9 ) + ( saturate( ( tex2DNode9 - tex2D( _MaskAlbedo, uv_MaskAlbedo ) ) ) * color22 ) ) )) + clampResult10 );
			#else
				float4 staticSwitch20 = (( _isWeapon )?( tex2DNode9 ):( ( ( _ColorAlbedo * tex2DNode9 ) + ( saturate( ( tex2DNode9 - tex2D( _MaskAlbedo, uv_MaskAlbedo ) ) ) * color22 ) ) ));
			#endif
			float4 AlbedoVar36 = staticSwitch20;
			o.Albedo = AlbedoVar36.rgb;
			float2 uv_TextureEmission = i.uv_texcoord * _TextureEmission_ST.xy + _TextureEmission_ST.zw;
			float4 color58 = IsGammaSpace() ? float4(3.435294,3.291407,1.456852,0) : float4(15.105,13.748,2.288305,0);
			float4 EmissionVar46 = (( _IsEmissionON )?( ( tex2D( _TextureEmission, uv_TextureEmission ) * 2.0 * color58 ) ):( float4( 0,0,0,0 ) ));
			o.Emission = EmissionVar46.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
0;0;1920;1029;2725.521;1501.599;1;False;False
Node;AmplifyShaderEditor.SamplerNode;9;-2951.29,-2811.906;Inherit;True;Property;_TextureAlbedo;TextureAlbedo;0;0;Create;True;0;0;False;0;-1;13e29d1fd4647e641b4ed0970b17cdb5;13e29d1fd4647e641b4ed0970b17cdb5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;44;-2926.263,-2491.571;Inherit;True;Property;_MaskAlbedo;MaskAlbedo;3;0;Create;True;0;0;False;0;-1;ec5eaadec970eca499296de8d7d148b1;ec5eaadec970eca499296de8d7d148b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;50;-2287.177,-2633.656;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;57;-2344.357,-3338.751;Inherit;False;Property;_ColorAlbedo;ColorAlbedo;5;0;Create;True;0;0;False;0;1,1,1,0;1,0,0.09110308,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-2134.574,-2211.35;Inherit;False;Constant;_ColorMask;ColorMask;4;0;Create;True;0;0;False;0;0.7830189,0.304542,0.1514329,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;51;-2022.445,-2569.103;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;1;-2286.376,-912.3623;Inherit;False;Constant;_Int1;Int 1;1;0;Create;True;0;0;False;0;10;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2421.867,-1275.932;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1837.472,-2270.926;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;3;-2105.686,-910.5546;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1796.691,-3237.299;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-1970.467,-1208.464;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-2132.113,-791.687;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;0;0;0;0.6;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;55;-2281.958,-1941.726;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2142.113,-660.6871;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;0.4;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1592.502,-2727.48;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;6;-2100.844,-1378.198;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;8;-1898.968,-908.6364;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;10;-1746.114,-901.6869;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1777.467,-1332.464;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;58;-1520.668,709.8113;Inherit;False;Constant;_Color1;Color 1;8;1;[HDR];Create;True;0;0;False;0;3.435294,3.291407,1.456852,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-907.9919,1026.602;Inherit;False;Constant;_FloatOutLine;FloatOutLine;7;0;Create;True;0;0;False;0;0.05;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-1796.674,247.3251;Inherit;True;Property;_TextureEmission;TextureEmission;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;42;-843.9919,898.6016;Inherit;False;Constant;_Int0;Int 0;8;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;30;-1577.142,-1725.667;Inherit;False;Property;_isWeapon;isWeapon;4;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1635.145,563.7399;Inherit;False;Constant;_num;num;8;0;Create;True;0;0;False;0;2;3.847574;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-597.4146,684.8699;Inherit;False;Constant;_ColorOutLine;ColorOutLine;7;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1248,-1040;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1213.36,407.9516;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1218.772,-106.2708;Inherit;False;Constant;_NormalScale;NormalScale;0;0;Create;True;0;0;False;0;2.28;2.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1551.288,-1347.365;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;31;-587.9919,946.6016;Inherit;False;Property;_isOutLineOn;isOutLineOn;6;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;45;-920.7961,325.4742;Inherit;False;Property;_IsEmissionON;IsEmissionON;7;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;20;-1033.791,-1465.78;Inherit;True;Property;_SwitchTexture;SwitchTexture;1;0;Create;True;0;0;False;0;0;0;0;False;;KeywordEnum;3;_Key0;_Key1;_Key2;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;32;-293.8374,785.1951;Inherit;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;17;-1008.059,-155.8618;Inherit;True;Property;_TextureNormal;TextureNormal;1;0;Create;True;0;0;False;0;-1;None;a1eab61512be39f43a87676bb8e919a6;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-633.2109,326.9568;Inherit;False;EmissionVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-678.6194,-1470.584;Inherit;False;AlbedoVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-16,784;Inherit;False;OutLineVar;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-662.9821,-163.6297;Inherit;False;NormalVar;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;289.6029,74.03841;Inherit;False;36;AlbedoVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;278.706,187.7546;Inherit;False;38;NormalVar;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;510.8272,509.3028;Inherit;False;40;OutLineVar;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;291.0741,312.3468;Inherit;False;46;EmissionVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;747.0731,196.4648;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;MasterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;1,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;9;0
WireConnection;50;1;44;0
WireConnection;51;0;50;0
WireConnection;23;0;51;0
WireConnection;23;1;22;0
WireConnection;3;0;1;0
WireConnection;56;0;57;0
WireConnection;56;1;9;0
WireConnection;55;0;9;0
WireConnection;52;0;56;0
WireConnection;52;1;23;0
WireConnection;6;3;2;0
WireConnection;8;0;3;0
WireConnection;10;0;8;0
WireConnection;10;1;4;0
WireConnection;10;2;5;0
WireConnection;11;0;6;0
WireConnection;11;1;7;0
WireConnection;30;0;52;0
WireConnection;30;1;55;0
WireConnection;12;0;30;0
WireConnection;12;1;10;0
WireConnection;49;0;16;0
WireConnection;49;1;48;0
WireConnection;49;2;58;0
WireConnection;13;0;30;0
WireConnection;13;1;11;0
WireConnection;31;0;42;0
WireConnection;31;1;35;0
WireConnection;45;1;49;0
WireConnection;20;1;30;0
WireConnection;20;0;13;0
WireConnection;20;2;12;0
WireConnection;32;0;34;0
WireConnection;32;1;31;0
WireConnection;17;5;26;0
WireConnection;46;0;45;0
WireConnection;36;0;20;0
WireConnection;40;0;32;0
WireConnection;38;0;17;0
WireConnection;0;0;37;0
WireConnection;0;1;39;0
WireConnection;0;2;47;0
WireConnection;0;11;41;0
ASEEND*/
//CHKSM=3326ADB74A08BB59264100AC5A13C1054C03C3FF