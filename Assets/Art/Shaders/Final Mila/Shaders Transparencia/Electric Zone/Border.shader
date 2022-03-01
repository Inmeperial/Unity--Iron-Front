// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Border"
{
	Properties
	{
		_DepthDistance("DepthDistance", Range( 0 , 5)) = 0
		_FallOff("FallOff", Range( -4 , 3)) = 0
		_DepthIntensity("DepthIntensity", Range( -2 , 2)) = 0
		_Scale1("Scale1", Range( 0 , 5)) = 0
		_Power1("Power1", Range( 0 , 10)) = 0
		_Scale2("Scale2", Range( 0 , 5)) = 0
		_Power("Power", Range( 0 , 10)) = 0
		_ElectricityIzq("ElectricityIzq", 2D) = "white" {}
		_ElectricityDer("ElectricityDer", 2D) = "white" {}
		_Vector0("Vector 0", Vector) = (0.15,-0.55,0,0)
		_offSetElectriciy("offSetElectriciy", Vector) = (0.15,-0.55,0,0)
		_TextureSample10("Texture Sample 10", 2D) = "white" {}
		_TextureSample11("Texture Sample 11", 2D) = "white" {}
		_ElectricSpeed("ElectricSpeed", Range( 0 , 5)) = 1
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample9("Texture Sample 9", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_TextureSample8("Texture Sample 8", 2D) = "white" {}
		_Tiling("Tiling", Range( 0 , 10)) = 0
		_RumbleSphere("RumbleSphere", Range( 0 , 100)) = 0
		_SpeedSphere("SpeedSphere", Range( 0 , 10)) = 1
		_SphereSize("SphereSize", Range( 0 , 40)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
			float4 screenPosition18;
			float2 uv_texcoord;
		};

		uniform float _SpeedSphere;
		uniform float _RumbleSphere;
		uniform float _SphereSize;
		uniform float _Scale1;
		uniform float _Power1;
		uniform float _Scale2;
		uniform float _Power;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _DepthIntensity;
		uniform float _FallOff;
		uniform sampler2D _ElectricityIzq;
		uniform sampler2D _TextureSample3;
		uniform float _ElectricSpeed;
		uniform float _Tiling;
		uniform sampler2D _ElectricityDer;
		uniform sampler2D _TextureSample2;
		uniform float2 _offSetElectriciy;
		uniform sampler2D _TextureSample11;
		uniform sampler2D _TextureSample8;
		uniform sampler2D _TextureSample10;
		uniform float2 _Vector0;
		uniform sampler2D _TextureSample9;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_vertexNormal = v.normal.xyz;
			float simplePerlin2D177 = snoise( ( ase_vertex3Pos + ( _Time.y * _SpeedSphere ) ).xy*_RumbleSphere );
			simplePerlin2D177 = simplePerlin2D177*0.5 + 0.5;
			float3 OndulacionesEsfera189 = ( ( ase_vertex3Pos + ( ase_vertexNormal * simplePerlin2D177 ) ) / _SphereSize );
			v.vertex.xyz += OndulacionesEsfera189;
			float3 vertexPos18 = ase_vertex3Pos;
			float4 ase_screenPos18 = ComputeScreenPos( UnityObjectToClipPos( vertexPos18 ) );
			o.screenPosition18 = ase_screenPos18;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV5 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode5 = ( 0.0 + _Scale1 * pow( 1.0 - fresnelNdotV5, _Power1 ) );
			float fresnelNdotV6 = dot( ase_worldNormal, -i.viewDir );
			float fresnelNode6 = ( 0.0 + _Scale2 * pow( 1.0 - fresnelNdotV6, _Power ) );
			float Fresnel8 = ( fresnelNode5 * fresnelNode6 );
			float4 color53 = IsGammaSpace() ? float4(0.7329842,1.507853,2,0) : float4(0.496405,2.468254,4.594794,0);
			float4 ase_screenPos18 = i.screenPosition18;
			float4 ase_screenPosNorm18 = ase_screenPos18 / ase_screenPos18.w;
			ase_screenPosNorm18.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm18.z : ase_screenPosNorm18.z * 0.5 + 0.5;
			float screenDepth18 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm18.xy ));
			float distanceDepth18 = saturate( abs( ( screenDepth18 - LinearEyeDepth( ase_screenPosNorm18.z ) ) / ( _DepthDistance ) ) );
			float DepthFade25 = saturate( pow( ( ( 1.0 - distanceDepth18 ) * _DepthIntensity ) , _FallOff ) );
			float4 color62 = IsGammaSpace() ? float4(0.776431,1.597229,2.118547,0) : float4(0.564647,2.801611,5.215354,0);
			float mulTime73 = _Time.y * 0.5;
			float ElectricitySpeed88 = _ElectricSpeed;
			float2 temp_cast_1 = (ElectricitySpeed88).xx;
			float Tiling147 = _Tiling;
			float2 temp_cast_2 = (Tiling147).xx;
			float2 uv_TexCoord74 = i.uv_texcoord * temp_cast_2;
			float2 panner76 = ( mulTime73 * temp_cast_1 + uv_TexCoord74);
			float4 lerpResult117 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample3, panner76 ) , 0.05);
			float mulTime80 = _Time.y * -0.5;
			float2 temp_cast_5 = (ElectricitySpeed88).xx;
			float2 temp_cast_6 = (Tiling147).xx;
			float2 uv_TexCoord79 = i.uv_texcoord * temp_cast_6 + _offSetElectriciy;
			float2 panner81 = ( mulTime80 * temp_cast_5 + uv_TexCoord79);
			float4 lerpResult115 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample2, panner81 ) , 0.05);
			float4 Electricity84 = ( tex2D( _ElectricityIzq, lerpResult117.rg ) + tex2D( _ElectricityDer, lerpResult115.rg ) );
			float4 color108 = IsGammaSpace() ? float4(0.776431,1.597229,2.118547,0) : float4(0.564647,2.801611,5.215354,0);
			float mulTime223 = _Time.y * 0.2;
			float2 temp_cast_8 = (ElectricitySpeed88).xx;
			float2 panner225 = ( mulTime223 * temp_cast_8 + i.uv_texcoord);
			float4 lerpResult234 = lerp( tex2D( _TextureSample11, panner225 ) , tex2D( _TextureSample8, panner225 ) , 0.05);
			float mulTime222 = _Time.y * -0.2;
			float2 temp_cast_9 = (ElectricitySpeed88).xx;
			float2 uv_TexCoord219 = i.uv_texcoord + _Vector0;
			float2 panner226 = ( mulTime222 * temp_cast_9 + uv_TexCoord219);
			float4 lerpResult233 = lerp( tex2D( _TextureSample10, panner226 ) , tex2D( _TextureSample9, panner226 ) , 0.05);
			float4 ElectricityMovement238 = ( lerpResult234 + lerpResult233 );
			float4 color242 = IsGammaSpace() ? float4(0.776431,1.597229,2.118547,0) : float4(0.564647,2.801611,5.215354,0);
			float4 Color63 = saturate( ( ( Fresnel8 * color53 ) + ( DepthFade25 * color62 ) + ( Electricity84 * color108 ) + ( ElectricityMovement238 * color242 * 2.0 ) ) );
			o.Emission = Color63.rgb;
			float4 Opacidad65 = saturate( ( Fresnel8 + DepthFade25 + Electricity84 ) );
			o.Alpha = Opacidad65.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float2 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xyzw = customInputData.screenPosition18;
				o.customPack2.xy = customInputData.uv_texcoord;
				o.customPack2.xy = v.texcoord;
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
				surfIN.screenPosition18 = IN.customPack1.xyzw;
				surfIN.uv_texcoord = IN.customPack2.xy;
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
50;324;1807;728;7529.688;2011.306;7.163241;True;True
Node;AmplifyShaderEditor.CommentaryNode;251;-4636.379,1733.439;Inherit;False;569.8892;309.4485;Tiling Exlectricidad;2;147;146;Tiling Exlectricidad;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;250;-4597.501,1379.671;Inherit;False;594.7349;313.011;Velocidad Electricidad;2;87;88;Velocidad Electricidad;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-4586.379,1783.439;Inherit;False;Property;_Tiling;Tiling;20;0;Create;True;0;0;False;0;0;5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;252;-3761.377,980.787;Inherit;False;2764.663;1694.344;Oscilacion Electricidad;25;149;74;73;89;76;119;118;129;117;115;78;77;82;84;171;148;83;80;79;90;81;130;116;114;139;Oscilacion Electricidad;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;147;-4309.49,1784.887;Inherit;False;Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-4547.501,1434.682;Inherit;False;Property;_ElectricSpeed;ElectricSpeed;15;0;Create;True;0;0;False;0;1;1.19;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;254;-6854.698,1011.884;Inherit;False;2138.528;1563.563;Movimiento Electricidad;19;217;222;220;224;223;221;219;226;225;231;230;240;239;228;229;234;233;237;238;Movimiento Electricidad;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;249;-3826.266,293.5219;Inherit;False;1772.881;616.1865;Depth Fade;10;16;17;18;21;19;23;20;22;24;25;Depth Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;83;-3696.515,2079.753;Inherit;False;Property;_offSetElectriciy;offSetElectriciy;10;0;Create;True;0;0;False;0;0.15,-0.55;9.3,7.94;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;148;-3711.377,1877.317;Inherit;False;147;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;88;-4274.766,1429.671;Inherit;False;ElectricitySpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;149;-3285.891,1096.582;Inherit;False;147;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;16;-3776.266,343.5219;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;74;-3073.994,1078.557;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-3761.07,550.1875;Inherit;False;Property;_DepthDistance;DepthDistance;0;0;Create;True;0;0;False;0;0;0.2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-3413.377,2230.711;Inherit;False;88;ElectricitySpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;80;-3365.177,2422.131;Inherit;False;1;0;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-3427.344,1973.559;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;217;-6804.698,1980.069;Inherit;False;Property;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;0.15,-0.55;9.3,7.94;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;73;-3018.728,1530.089;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-3070.151,1338.378;Inherit;False;88;ElectricitySpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;222;-6473.36,2322.446;Inherit;False;1;0;FLOAT;-0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;81;-3110.27,2011.521;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;18;-3484.504,431.6586;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;224;-6562.43,1061.884;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;248;-3834.912,-868.7057;Inherit;False;1587.81;1012.482;Fresnel;10;3;59;4;57;56;60;6;5;7;8;Fresnel;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;221;-6558.586,1321.705;Inherit;False;88;ElectricitySpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;219;-6535.527,1873.874;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;220;-6563.298,2129.213;Inherit;False;88;ElectricitySpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;76;-2788.921,1328.483;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;223;-6507.164,1513.421;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;118;-2516.799,1482.709;Inherit;True;Property;_TextureSample3;Texture Sample 3;18;0;Create;True;0;0;False;0;-1;None;559f8ee5680d0db4bbe72474a21422fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-3305.253,641.7084;Inherit;False;Property;_DepthIntensity;DepthIntensity;2;0;Create;True;0;0;False;0;0;1.4;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-3230.253,432.7087;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-2759.825,2182.652;Inherit;True;Property;_TextureSample2;Texture Sample 2;16;0;Create;True;0;0;False;0;-1;None;559f8ee5680d0db4bbe72474a21422fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;119;-2396.517,1674.886;Inherit;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-2665.399,2378.414;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;3;-3784.912,-407.2457;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;130;-2722.616,1723.169;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;226;-6218.453,1911.837;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;225;-6277.357,1311.81;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;129;-2471.669,1030.787;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;230;-5884.953,1658.218;Inherit;False;Constant;_Float5;Float 5;11;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;-5773.582,2278.729;Inherit;False;Constant;_Float4;Float 4;11;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-3351.903,-818.7057;Inherit;False;Property;_Scale1;Scale1;3;0;Create;True;0;0;False;0;0;0.49;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-3343.854,-604.4206;Inherit;False;Property;_Power1;Power1;4;0;Create;True;0;0;False;0;0;4.36;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;240;-6006.136,1281.018;Inherit;True;Property;_TextureSample11;Texture Sample 11;14;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;228;-6005.235,1466.039;Inherit;True;Property;_TextureSample8;Texture Sample 8;19;0;Create;True;0;0;False;0;-1;None;559f8ee5680d0db4bbe72474a21422fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;231;-5868.007,2082.967;Inherit;True;Property;_TextureSample9;Texture Sample 9;17;0;Create;True;0;0;False;0;-1;None;559f8ee5680d0db4bbe72474a21422fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;239;-5877.623,1887.134;Inherit;True;Property;_TextureSample10;Texture Sample 10;12;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;115;-2376.531,2035.451;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2986.253,438.7087;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3020.253,651.7084;Inherit;False;Property;_FallOff;FallOff;1;0;Create;True;0;0;False;0;0;1.33;-4;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;117;-2150.758,1462.962;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-3358.732,-114.2234;Inherit;False;Property;_Power;Power;6;0;Create;True;0;0;False;0;0;4.03;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;253;-3756.722,2791.545;Inherit;False;2101.006;796.97;Moviemiento Ondulatorio en OffSet;14;176;173;172;175;174;186;182;177;181;184;188;183;187;189;Moviemiento Ondulatorio en OffSet;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-3355.225,-331.6818;Inherit;False;Property;_Scale2;Scale2;5;0;Create;True;0;0;False;0;0;1.08;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;4;-3555.912,-406.2457;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;233;-5550.839,2057.649;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-3706.722,3330.515;Inherit;False;Property;_SpeedSphere;SpeedSphere;22;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;173;-3645.526,3115.421;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;77;-1878.357,1447.777;Inherit;True;Property;_ElectricityIzq;ElectricityIzq;7;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;5;-3016.09,-686.362;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.21;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;6;-3017.789,-425.0199;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-1873.792,1636.279;Inherit;True;Property;_ElectricityDer;ElectricityDer;8;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;22;-2718.385,438.0326;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;234;-5639.194,1446.292;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;24;-2466.385,441.0326;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2697.684,-517.0364;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;-3417.792,3248.853;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;172;-3431.4,3058.543;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;237;-5289.075,1751.258;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1528.619,1531.736;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;238;-5000.17,1745.301;Inherit;False;ElectricityMovement;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;174;-3174.668,3093.332;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-2490.102,-513.9381;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-2296.385,438.0326;Inherit;False;DepthFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-1239.714,1525.779;Inherit;False;Electricity;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;246;-1903.693,-1617.561;Inherit;False;1369.672;1792.421;Sumatoria de efectos en emision;16;62;244;242;86;53;14;27;108;243;241;61;52;107;13;10;63;Sumatoria De Efectos;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;186;-3209.02,3305.993;Inherit;False;Property;_RumbleSphere;RumbleSphere;21;0;Create;True;0;0;False;0;0;2;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;247;-1890.724,222.8964;Inherit;False;902.1983;668.5983;Mascara De Opacidad;6;85;15;26;12;11;65;Mascara De Opacidad;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;62;-1848.135,-1001.174;Inherit;False;Constant;_Color1;Color 1;3;1;[HDR];Create;True;0;0;False;0;0.776431,1.597229,2.118547,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;27;-1850.433,-1188.688;Inherit;False;25;DepthFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-1839.593,-832.803;Inherit;False;84;Electricity;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;108;-1826.038,-647.9384;Inherit;False;Constant;_Color2;Color 2;3;1;[HDR];Create;True;0;0;False;0;0.776431,1.597229,2.118547,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;243;-1850.281,-461.7798;Inherit;False;238;ElectricityMovement;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-1793.962,-83.14023;Inherit;False;Constant;_Float1;Float 1;24;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;182;-2907.024,2923.164;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;177;-2901.518,3126.962;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;242;-1853.693,-246.7941;Inherit;False;Constant;_Color3;Color 3;3;1;[HDR];Create;True;0;0;False;0;0.776431,1.597229,2.118547,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;-1849.318,-1364.092;Inherit;False;Constant;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;0.7329842,1.507853,2,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;14;-1838.071,-1567.561;Inherit;False;8;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1840.724,272.8964;Inherit;False;8;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1517.391,-1456.352;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-1839.075,460.775;Inherit;False;25;DepthFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;241;-1558.515,-482.2545;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1515.339,-1167.351;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1828.893,661.4947;Inherit;False;84;Electricity;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-1523.211,-855.1364;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-2642.846,3039.209;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;184;-2652.849,2841.545;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;188;-2437.907,3203.56;Inherit;False;Property;_SphereSize;SphereSize;23;0;Create;True;0;0;False;0;0;40;0;40;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;183;-2358.83,2974.39;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-1148.717,-1167.286;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1609.7,282.527;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;10;-941.1358,-1168.835;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;187;-2144.686,2977.748;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;11;-1397.471,282.527;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-777.0213,-1169.89;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;-1231.525,284.2875;Inherit;False;Opacidad;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;189;-1936.715,2980.028;Inherit;False;OndulacionesEsfera;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;289.1898,757.9045;Inherit;False;189;OndulacionesEsfera;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;171;-2517.7,1297.691;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;139;-2769.44,1986.818;Inherit;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;False;0;-1;None;4f83293f1cba14040b1b6e1b1e9b1924;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;66;326.8956,567.3253;Inherit;False;65;Opacidad;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;341.1448,377.631;Inherit;False;63;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;680.5394,392.9875;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Border;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;26.7;10;25;True;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;147;0;146;0
WireConnection;88;0;87;0
WireConnection;74;0;149;0
WireConnection;79;0;148;0
WireConnection;79;1;83;0
WireConnection;81;0;79;0
WireConnection;81;2;90;0
WireConnection;81;1;80;0
WireConnection;18;1;16;0
WireConnection;18;0;17;0
WireConnection;219;1;217;0
WireConnection;76;0;74;0
WireConnection;76;2;89;0
WireConnection;76;1;73;0
WireConnection;118;1;76;0
WireConnection;19;0;18;0
WireConnection;114;1;81;0
WireConnection;226;0;219;0
WireConnection;226;2;220;0
WireConnection;226;1;222;0
WireConnection;225;0;224;0
WireConnection;225;2;221;0
WireConnection;225;1;223;0
WireConnection;240;1;225;0
WireConnection;228;1;225;0
WireConnection;231;1;226;0
WireConnection;239;1;226;0
WireConnection;115;0;130;0
WireConnection;115;1;114;0
WireConnection;115;2;116;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;117;0;129;0
WireConnection;117;1;118;0
WireConnection;117;2;119;0
WireConnection;4;0;3;0
WireConnection;233;0;239;0
WireConnection;233;1;231;0
WireConnection;233;2;229;0
WireConnection;77;1;117;0
WireConnection;5;2;56;0
WireConnection;5;3;57;0
WireConnection;6;4;4;0
WireConnection;6;2;59;0
WireConnection;6;3;60;0
WireConnection;78;1;115;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;234;0;240;0
WireConnection;234;1;228;0
WireConnection;234;2;230;0
WireConnection;24;0;22;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;175;0;173;0
WireConnection;175;1;176;0
WireConnection;237;0;234;0
WireConnection;237;1;233;0
WireConnection;82;0;77;0
WireConnection;82;1;78;0
WireConnection;238;0;237;0
WireConnection;174;0;172;0
WireConnection;174;1;175;0
WireConnection;8;0;7;0
WireConnection;25;0;24;0
WireConnection;84;0;82;0
WireConnection;177;0;174;0
WireConnection;177;1;186;0
WireConnection;52;0;14;0
WireConnection;52;1;53;0
WireConnection;241;0;243;0
WireConnection;241;1;242;0
WireConnection;241;2;244;0
WireConnection;61;0;27;0
WireConnection;61;1;62;0
WireConnection;107;0;86;0
WireConnection;107;1;108;0
WireConnection;181;0;182;0
WireConnection;181;1;177;0
WireConnection;183;0;184;0
WireConnection;183;1;181;0
WireConnection;13;0;52;0
WireConnection;13;1;61;0
WireConnection;13;2;107;0
WireConnection;13;3;241;0
WireConnection;12;0;15;0
WireConnection;12;1;26;0
WireConnection;12;2;85;0
WireConnection;10;0;13;0
WireConnection;187;0;183;0
WireConnection;187;1;188;0
WireConnection;11;0;12;0
WireConnection;63;0;10;0
WireConnection;65;0;11;0
WireConnection;189;0;187;0
WireConnection;171;1;76;0
WireConnection;139;1;81;0
WireConnection;0;2;64;0
WireConnection;0;9;66;0
WireConnection;0;11;190;0
ASEEND*/
//CHKSM=DBC10271DB6025F0E283B5AA5C55F5611BCD871D