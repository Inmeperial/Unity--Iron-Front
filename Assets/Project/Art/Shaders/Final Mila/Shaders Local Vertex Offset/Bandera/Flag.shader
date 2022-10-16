// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flag"
{
	Properties
	{
		_Amplitud("Amplitud", Float) = 0.69
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 9.8
		_Velocidad("Velocidad", Float) = 4
		_Ondulaciones("Ondulaciones", Float) = 1.34
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_NormalIntensity("NormalIntensity", Range( 0 , 2)) = 1.15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Amplitud;
		uniform float _Velocidad;
		uniform float _Ondulaciones;
		uniform float _NormalIntensity;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _EdgeLength;


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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break18 = ase_vertex3Pos;
			float mulTime23 = _Time.y * _Velocidad;
			float4 appendResult19 = (float4(break18.x , ( ( _Amplitud * sin( ( break18.z + mulTime23 ) ) ) * v.texcoord.xy.y ) , break18.z , 0.0));
			float4 OndulacionesPrincipales49 = appendResult19;
			float4 appendResult39 = (float4(_SinTime.w , _CosTime.w , 0.0 , 0.0));
			float2 uv_TexCoord33 = v.texcoord.xy + appendResult39.xy;
			float simplePerlin2D34 = snoise( uv_TexCoord33*_Ondulaciones );
			simplePerlin2D34 = simplePerlin2D34*0.5 + 0.5;
			float OndulacionesExtra47 = ( simplePerlin2D34 * v.texcoord.xy.y );
			v.vertex.xyz += ( OndulacionesPrincipales49 + OndulacionesExtra47 ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _TextureSample1, uv_TextureSample1 ), _NormalIntensity );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;279;1807;740;2370.776;508.4979;2.298521;True;True
Node;AmplifyShaderEditor.CommentaryNode;51;-2866.448,-372.6772;Inherit;False;1210.311;605.3021;OndulacionesPrincipales;8;12;24;23;18;20;29;21;30;OndulacionesPrincipales;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;12;-2816.448,-320.1852;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-2699.714,-97.3127;Inherit;False;Property;_Velocidad;Velocidad;6;0;Create;True;0;0;False;0;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-2607.251,697.3692;Inherit;False;1048.415;632.2314;OndulacionesExtra;6;37;38;39;35;33;34;OndulacionesExtra;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-2539.974,-96.4355;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;18;-2596.935,-322.6772;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SinTimeNode;37;-2557.251,747.3693;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;44;-2258.689,236.41;Inherit;False;548.3418;351.2803;Mascara Borde;2;25;26;Mascara Borde;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-2288.367,-21.81724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosTime;38;-2557.251,938.7994;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;39;-2308.99,852.2564;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;45;-2107.329,1397.597;Inherit;False;548.3409;351.2804;Mascara Borde;2;40;41;Mascara Borde;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2208.689,288.6904;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;21;-2080.574,-20.37504;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2090.843,-222.3575;Inherit;False;Property;_Amplitud;Amplitud;0;0;Create;True;0;0;False;0;0.69;0.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1891.137,-44.33867;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;26;-1981.347,286.41;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-2057.329,1449.878;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-2090.967,803.3233;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-2031.121,1071.601;Inherit;False;Property;_Ondulaciones;Ondulaciones;7;0;Create;True;0;0;False;0;1.34;1.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-1822.837,931.3344;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;41;-1829.989,1447.597;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1618.717,3.379009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-1348.152,-323.468;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1382.589,933.8473;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;43;-780.9192,-342.0282;Inherit;False;643.9;551.9947;Textura;3;3;1;2;Textura;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-1142.906,-326.4731;Inherit;False;OndulacionesPrincipales;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-1174.491,930.1713;Inherit;False;OndulacionesExtra;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-730.9192,-48.03349;Inherit;False;Property;_NormalIntensity;NormalIntensity;10;0;Create;True;0;0;False;0;1.15;0.1609771;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-487.4791,433.3758;Inherit;False;47;OndulacionesExtra;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-529.6219,234.9648;Inherit;False;49;OndulacionesPrincipales;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;1;-473.498,-292.0282;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;False;0;-1;None;e97643e261cae614eb0dcbd2f6f290dc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-457.0191,-93.43346;Inherit;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;False;0;-1;None;1a1711663d430c048b8fb6cd66fd94f8;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-215.7403,280.102;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Flag;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;9.8;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;24;0
WireConnection;18;0;12;0
WireConnection;20;0;18;2
WireConnection;20;1;23;0
WireConnection;39;0;37;4
WireConnection;39;1;38;4
WireConnection;21;0;20;0
WireConnection;30;0;29;0
WireConnection;30;1;21;0
WireConnection;26;0;25;0
WireConnection;33;1;39;0
WireConnection;34;0;33;0
WireConnection;34;1;35;0
WireConnection;41;0;40;0
WireConnection;27;0;30;0
WireConnection;27;1;26;1
WireConnection;19;0;18;0
WireConnection;19;1;27;0
WireConnection;19;2;18;2
WireConnection;42;0;34;0
WireConnection;42;1;41;1
WireConnection;49;0;19;0
WireConnection;47;0;42;0
WireConnection;2;5;3;0
WireConnection;13;0;50;0
WireConnection;13;1;48;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;11;13;0
ASEEND*/
//CHKSM=BEBE0E8252B455395DB0441E3C4044F8697C21CB