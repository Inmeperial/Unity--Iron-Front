// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RainDrop"
{
	Properties
	{
		_Tiling("Tiling", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_Speed("Speed", Float) = 8
		_Humedad("Humedad", Range( 0 , 1)) = 0.2
		_OffsetSegundo("Offset Segundo", Vector) = (0.29,0.72,0,0)
		_IntensidadNormales("Intensidad Normales", Range( 0 , 2)) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Tinte("Tinte", Color) = (1,1,1,0)
		_AmbientOclussion("Ambient Oclussion", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_WaterPuddle("WaterPuddle", 2D) = "white" {}
		_IntensidadNormalTextura("IntensidadNormalTextura", Range( 0 , 2)) = 0
		_PuddleIntensidad("Puddle Intensidad", Float) = 0
		_PuddleTilling("Puddle Tilling", Float) = 1
		_SpeedPuddle("Speed Puddle", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float _IntensidadNormales;
		uniform float _Tiling;
		uniform float2 _OffsetSegundo;
		uniform float _Speed;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _IntensidadNormalTextura;
		uniform sampler2D _WaterPuddle;
		uniform float _PuddleIntensidad;
		uniform float _SpeedPuddle;
		uniform float _PuddleTilling;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _AmbientOclussion;
		uniform float4 _AmbientOclussion_ST;
		uniform float4 _Tinte;
		uniform float _Humedad;


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
			float Tiling51 = _Tiling;
			float2 temp_cast_0 = (Tiling51).xx;
			float2 uv_TexCoord34 = i.uv_texcoord * temp_cast_0;
			float2 panner35 = ( ( _Time.y * 0.3 ) * float2( 1,1 ) + uv_TexCoord34);
			float simplePerlin2D33 = snoise( panner35 );
			float RuidoIzq90 = simplePerlin2D33;
			float2 temp_cast_1 = (Tiling51).xx;
			float2 uv_TexCoord46 = i.uv_texcoord * temp_cast_1;
			float2 panner47 = ( ( _Time.y * 0.3 ) * float2( -1,-1 ) + uv_TexCoord46);
			float simplePerlin2D48 = snoise( panner47 );
			float RuidoDer91 = simplePerlin2D48;
			float Mascara40 = ( _IntensidadNormales * ( RuidoIzq90 + RuidoDer91 ) );
			float2 temp_cast_2 = (( Tiling51 / 0.6 )).xx;
			float2 uv_TexCoord25 = i.uv_texcoord * temp_cast_2 + _OffsetSegundo;
			float4 appendResult28 = (float4(frac( uv_TexCoord25.x ) , frac( uv_TexCoord25.y ) , 0.0 , 0.0));
			float temp_output_4_0_g4 = 4.0;
			float temp_output_5_0_g4 = 4.0;
			float2 appendResult7_g4 = (float2(temp_output_4_0_g4 , temp_output_5_0_g4));
			float totalFrames39_g4 = ( temp_output_4_0_g4 * temp_output_5_0_g4 );
			float2 appendResult8_g4 = (float2(totalFrames39_g4 , temp_output_5_0_g4));
			float Tiempo94 = ( _Time.y * _Speed );
			float clampResult42_g4 = clamp( 0.0 , 0.0001 , ( totalFrames39_g4 - 1.0 ) );
			float temp_output_35_0_g4 = frac( ( ( Tiempo94 + clampResult42_g4 ) / totalFrames39_g4 ) );
			float2 appendResult29_g4 = (float2(temp_output_35_0_g4 , ( 1.0 - temp_output_35_0_g4 )));
			float2 temp_output_15_0_g4 = ( ( appendResult28.xy / appendResult7_g4 ) + ( floor( ( appendResult8_g4 * appendResult29_g4 ) ) / appendResult7_g4 ) );
			float2 temp_cast_4 = (Tiling51).xx;
			float2 uv_TexCoord9 = i.uv_texcoord * temp_cast_4;
			float4 appendResult19 = (float4(frac( uv_TexCoord9.x ) , frac( uv_TexCoord9.y ) , 0.0 , 0.0));
			float temp_output_4_0_g5 = 4.0;
			float temp_output_5_0_g5 = 4.0;
			float2 appendResult7_g5 = (float2(temp_output_4_0_g5 , temp_output_5_0_g5));
			float totalFrames39_g5 = ( temp_output_4_0_g5 * temp_output_5_0_g5 );
			float2 appendResult8_g5 = (float2(totalFrames39_g5 , temp_output_5_0_g5));
			float clampResult42_g5 = clamp( 0.0 , 0.0001 , ( totalFrames39_g5 - 1.0 ) );
			float temp_output_35_0_g5 = frac( ( ( Tiempo94 + clampResult42_g5 ) / totalFrames39_g5 ) );
			float2 appendResult29_g5 = (float2(temp_output_35_0_g5 , ( 1.0 - temp_output_35_0_g5 )));
			float2 temp_output_15_0_g5 = ( ( appendResult19.xy / appendResult7_g5 ) + ( floor( ( appendResult8_g5 * appendResult29_g5 ) ) / appendResult7_g5 ) );
			float3 NormalesGoteo67 = BlendNormals( UnpackScaleNormal( tex2D( _Texture0, temp_output_15_0_g4 ), Mascara40 ) , UnpackScaleNormal( tex2D( _Texture0, temp_output_15_0_g5 ), Mascara40 ) );
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 NormalTexture74 = ( UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) ) * _IntensidadNormalTextura );
			float2 temp_cast_6 = (_SpeedPuddle).xx;
			float2 temp_cast_7 = (_PuddleTilling).xx;
			float2 uv_TexCoord77 = i.uv_texcoord * temp_cast_7;
			float2 panner87 = ( 1.0 * _Time.y * temp_cast_6 + uv_TexCoord77);
			float2 temp_cast_8 = (_SpeedPuddle).xx;
			float2 panner88 = ( -1.0 * _Time.y * temp_cast_8 + uv_TexCoord77);
			float3 PuddleNormals78 = BlendNormals( UnpackScaleNormal( tex2D( _WaterPuddle, panner87 ), _PuddleIntensidad ) , UnpackScaleNormal( tex2D( _WaterPuddle, panner88 ), _PuddleIntensidad ) );
			float3 SumatoriaDeNormales97 = BlendNormals( BlendNormals( NormalesGoteo67 , NormalTexture74 ) , PuddleNormals78 );
			o.Normal = SumatoriaDeNormales97;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float2 uv_AmbientOclussion = i.uv_texcoord * _AmbientOclussion_ST.xy + _AmbientOclussion_ST.zw;
			float4 Albedo65 = ( ( tex2D( _Albedo, uv_Albedo ) * tex2D( _AmbientOclussion, uv_AmbientOclussion ) ) * _Tinte );
			o.Albedo = Albedo65.rgb;
			o.Smoothness = _Humedad;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
-68;957;1807;704;1807.609;698.6948;1.708448;True;True
Node;AmplifyShaderEditor.CommentaryNode;103;-3379.328,-1446.629;Inherit;False;464.1809;313.6581;Goteo Tiling;2;10;51;Goteo Tiling;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-3329.328,-1396.629;Inherit;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;False;0;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;106;-3432.928,-167.4183;Inherit;False;1261.167;779.514;Mascara Der;8;42;43;55;46;45;47;48;91;Mascara Der;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;105;-3420.537,-979.2567;Inherit;False;1248.84;769.8459;Mascara Izq;8;38;54;37;34;39;35;33;90;Mascara Izq;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-3158.147,-1390.971;Inherit;False;Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-3359.036,-467.411;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;0.3;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-3370.537,-906.9098;Inherit;False;51;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;37;-3361.845,-682.8958;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-3375.416,354.0952;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.3;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;43;-3378.225,138.6102;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-3382.928,-93.43645;Inherit;False;51;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-3154.718,-929.2567;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-3135.929,-674.9918;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-3168.648,-111.6524;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-3152.309,146.5142;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;35;-2912.874,-923.3137;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;47;-2911.478,-111.9654;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;99;-2097.979,-768.755;Inherit;False;2699.629;1373.142;Goteos;23;53;52;31;32;9;25;26;27;18;17;19;28;96;95;41;23;50;8;15;22;11;29;67;Goteos;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;33;-2657.477,-926.7887;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;48;-2656.081,-115.4414;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;-2414.697,-927.4117;Inherit;False;RuidoIzq;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-2047.979,-691.2737;Inherit;False;51;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;107;-3432.358,690.8181;Inherit;False;997.0349;653.066;Mascara Para Goteo;6;93;92;49;57;56;40;Mascara Para Goteo;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-2414.761,-117.4184;Inherit;False;RuidoDer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;104;-2842.866,-1671.018;Inherit;False;682.7109;531.5023;Tiempo;4;12;14;13;94;Tiempo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-2792.866,-1621.018;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-1711.253,164.6825;Inherit;False;51;Tiling;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2782.41,-1397.516;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;False;0;8;18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;32;-1860.653,-470.6086;Inherit;False;Property;_OffsetSegundo;Offset Segundo;4;0;Create;True;0;0;False;0;0.29,0.72;1.12,1.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;92;-3379.002,1113.884;Inherit;False;91;RuidoDer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-1840.634,-691.7385;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;-3382.358,927.249;Inherit;False;90;RuidoIzq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1571.869,-605.3243;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.42,0.65;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1482.305,148.6845;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2605.582,-1526.854;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-3133.441,976.5301;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-3180.233,740.818;Inherit;False;Property;_IntensidadNormales;Intensidad Normales;5;0;Create;True;0;0;False;0;0;0.42;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;18;-1196.896,264.3508;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;17;-1194.083,106.7996;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;26;-1286.46,-489.6581;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-2403.155,-1538.884;Inherit;False;Tiempo;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;27;-1285.529,-718.755;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2895.212,952.78;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;102;-2092.546,-1713.215;Inherit;False;2515.635;908.8949;Charcos;11;82;77;86;83;88;84;87;85;71;89;78;Charcos;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-1023.535,-393.0681;Inherit;False;94;Tiempo;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-1032.822,-607.4099;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2042.547,-1344.012;Inherit;False;Property;_PuddleTilling;Puddle Tilling;13;0;Create;True;0;0;False;0;1;1.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1005.022,155.2606;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-2678.323,953.163;Inherit;False;Mascara;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-978.837,374.387;Inherit;False;94;Tiempo;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-1844.669,-1360.769;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;15;-786.6172,-235.8784;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;None;ab65fc2688a32aa4680d7f78483449cf;True;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-696.4762,-420.7033;Inherit;False;40;Mascara;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-690.5084,-42.30165;Inherit;False;40;Mascara;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;8;-745.6406,154.8579;Inherit;False;Flipbook;-1;;5;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;4;False;5;FLOAT;4;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.RangedFloatNode;86;-1371.786,-1405.916;Inherit;False;Property;_SpeedPuddle;Speed Puddle;14;0;Create;True;0;0;False;0;0;0.016;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;23;-771.6662,-610.5705;Inherit;False;Flipbook;-1;;4;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;4;False;5;FLOAT;4;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.CommentaryNode;100;634.2194,-1101.191;Inherit;False;839.6226;497.8378;Normal Textura;4;73;69;72;74;Normal Textura;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;84;-1081.38,-1401.265;Inherit;True;Property;_WaterPuddle;WaterPuddle;10;0;Create;True;0;0;False;0;None;2dd3788f8589b40bf82a92d76ffc5091;True;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;22;-288.1233,-352.3652;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;684.2194,-1051.191;Inherit;True;Property;_NormalMap;NormalMap;9;0;Create;True;0;0;False;0;-1;None;8d6abaa52f55b1d4292cd03ce2fa2940;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-281.5712,-109.8472;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;-716.0811,-1360.004;Inherit;False;Property;_PuddleIntensidad;Puddle Intensidad;12;0;Create;True;0;0;False;0;0;1.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;87;-1093.735,-1663.215;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;88;-1082.987,-1057.32;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;711.2374,-861.353;Inherit;False;Property;_IntensidadNormalTextura;IntensidadNormalTextura;11;0;Create;True;0;0;False;0;0;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;-426.3753,-1530.229;Inherit;True;Property;_PuddleNormals;PuddleNormals;11;0;Create;True;0;0;False;0;-1;None;2dd3788f8589b40bf82a92d76ffc5091;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;85;-419.7777,-1223.08;Inherit;True;Property;_TextureSample2;Texture Sample 2;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;1009.002,-940.2858;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;29;63.41629,-211.5365;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;329.6497,-213.8999;Inherit;False;NormalesGoteo;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;101;638.7946,-1657.601;Inherit;False;1072.805;566.7689;Albedo Textura;6;58;60;61;63;64;65;Albedo Textura;1,1,1,1;0;0
Node;AmplifyShaderEditor.BlendNormalsNode;89;-95.53633,-1410.45;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;108;-2270.558,781.7754;Inherit;False;1149.505;575.1799;Sumatoria de Normales;6;75;68;80;70;79;97;Sumatoria de Normales;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;1222.842,-942.2842;Inherit;False;NormalTexture;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-2220.558,831.7754;Inherit;False;67;NormalesGoteo;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;60;695.0873,-1413.711;Inherit;True;Property;_AmbientOclussion;Ambient Oclussion;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;75;-2209.546,1029.197;Inherit;False;74;NormalTexture;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;58;688.7946,-1607.601;Inherit;True;Property;_Albedo;Albedo;6;0;Create;True;0;0;False;0;-1;None;e79ee784ef4343f43abf08bf810a7202;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;172.0888,-1410.332;Inherit;False;PuddleNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;70;-1975.577,909.8635;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1011.112,-1516.879;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;1020.299,-1297.832;Inherit;False;Property;_Tinte;Tinte;7;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;80;-1953.288,1126.955;Inherit;False;78;PuddleNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;1256.023,-1358.953;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;79;-1687.769,932.7999;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;1468.6,-1358.661;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1420.052,933.9958;Inherit;False;SumatoriaDeNormales;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;1548.173,-153.8962;Inherit;False;65;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;98;1548.109,32.60632;Inherit;False;97;SumatoriaDeNormales;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;16;1547.028,222.9478;Inherit;False;Property;_Humedad;Humedad;3;0;Create;True;0;0;False;0;0.2;0.631;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1882.444,-58.19621;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;RainDrop;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;10;0
WireConnection;34;0;54;0
WireConnection;39;0;37;0
WireConnection;39;1;38;0
WireConnection;46;0;55;0
WireConnection;45;0;43;0
WireConnection;45;1;42;0
WireConnection;35;0;34;0
WireConnection;35;1;39;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;33;0;35;0
WireConnection;48;0;47;0
WireConnection;90;0;33;0
WireConnection;91;0;48;0
WireConnection;31;0;53;0
WireConnection;25;0;31;0
WireConnection;25;1;32;0
WireConnection;9;0;52;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;49;0;93;0
WireConnection;49;1;92;0
WireConnection;18;0;9;2
WireConnection;17;0;9;1
WireConnection;26;0;25;2
WireConnection;94;0;13;0
WireConnection;27;0;25;1
WireConnection;56;0;57;0
WireConnection;56;1;49;0
WireConnection;28;0;27;0
WireConnection;28;1;26;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;40;0;56;0
WireConnection;77;0;82;0
WireConnection;8;13;19;0
WireConnection;8;2;95;0
WireConnection;23;13;28;0
WireConnection;23;2;96;0
WireConnection;22;0;15;0
WireConnection;22;1;23;0
WireConnection;22;5;41;0
WireConnection;11;0;15;0
WireConnection;11;1;8;0
WireConnection;11;5;50;0
WireConnection;87;0;77;0
WireConnection;87;2;86;0
WireConnection;88;0;77;0
WireConnection;88;2;86;0
WireConnection;71;0;84;0
WireConnection;71;1;87;0
WireConnection;71;5;83;0
WireConnection;85;0;84;0
WireConnection;85;1;88;0
WireConnection;85;5;83;0
WireConnection;72;0;69;0
WireConnection;72;1;73;0
WireConnection;29;0;22;0
WireConnection;29;1;11;0
WireConnection;67;0;29;0
WireConnection;89;0;71;0
WireConnection;89;1;85;0
WireConnection;74;0;72;0
WireConnection;78;0;89;0
WireConnection;70;0;68;0
WireConnection;70;1;75;0
WireConnection;61;0;58;0
WireConnection;61;1;60;0
WireConnection;64;0;61;0
WireConnection;64;1;63;0
WireConnection;79;0;70;0
WireConnection;79;1;80;0
WireConnection;65;0;64;0
WireConnection;97;0;79;0
WireConnection;0;0;66;0
WireConnection;0;1;98;0
WireConnection;0;4;16;0
ASEEND*/
//CHKSM=1A7F97CC80190ED2E2D2379AF4AE91429DF5634D