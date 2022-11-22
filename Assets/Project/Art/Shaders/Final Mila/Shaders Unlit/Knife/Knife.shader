// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "p"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 7.8
		_MinOld("MinOld", Float) = 0
		_MinNew("MinNew", Float) = 0
		_MaxNew("MaxNew", Float) = 0
		_MaxOld("MaxOld", Float) = 0
		_MascaraBorde("MascaraBorde", Range( -0.53 , 3)) = -0.69
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _MinOld;
		uniform float _MaxOld;
		uniform float _MinNew;
		uniform float _MaxNew;
		uniform float _MascaraBorde;
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
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color1 = IsGammaSpace() ? float4(5.992157,0.7529412,0,0) : float4(51.36686,0.5271152,0,0);
			float2 panner21 = ( ( _Time.y * 0.06 ) * float2( 1,1 ) + i.uv_texcoord);
			float simplePerlin2D23 = snoise( panner21*15.0 );
			float RuidoIzq26 = simplePerlin2D23;
			float2 panner22 = ( ( _Time.y * 0.06 ) * float2( -1,-1 ) + i.uv_texcoord);
			float simplePerlin2D24 = snoise( panner22*15.0 );
			float RuidoDer25 = simplePerlin2D24;
			float Ruido33 = ( RuidoIzq26 + RuidoDer25 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float MascaraBorde56 = saturate( ( 1.0 - ( ase_vertex3Pos.y - _MascaraBorde ) ) );
			o.Emission = ( color1 * (_MinNew + (Ruido33 - _MinOld) * (_MaxNew - _MinNew) / (_MaxOld - _MinOld)) * MascaraBorde56 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
36;477;1807;702;3631.026;574.2559;2.138864;True;True
Node;AmplifyShaderEditor.CommentaryNode;9;-4118.598,395.7543;Inherit;False;1261.167;779.514;Ruido Der;8;25;24;22;18;17;12;11;35;Ruido Der;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;10;-4106.207,-416.0842;Inherit;False;1248.84;769.8459;Ruido Izq;8;26;23;21;20;19;16;15;34;Ruido Izq;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-4061.086,917.2675;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.06;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;11;-4063.895,701.7827;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-4047.515,-119.7233;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-4044.706,95.76154;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;False;0;0.06;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-3840.388,-366.0842;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-3837.979,709.6868;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-3821.599,-111.8193;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-3854.318,451.5201;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-3564.594,691.8412;Inherit;False;Constant;_Float6;Float 0;0;0;Create;True;0;0;False;0;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3556.24,-124.6047;Inherit;False;Constant;_Float5;Float 0;0;0;Create;True;0;0;False;0;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;21;-3598.544,-360.1412;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;22;-3597.148,451.2071;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;58;-2786.322,-416.5161;Inherit;False;1114.542;504.7227;Mascara Borde;6;51;55;52;56;50;53;Mascara Borde;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;24;-3341.751,447.7311;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;23;-3343.147,-363.6162;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2763.722,-174.7935;Inherit;False;Property;_MascaraBorde;MascaraBorde;9;0;Create;True;0;0;False;0;-0.69;3;-0.53;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;50;-2736.322,-366.5161;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;46;-4118.028,1213.162;Inherit;False;790.4263;466.6349;Ruido;4;30;29;28;33;Ruido;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-3100.431,445.7541;Inherit;False;RuidoDer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-3100.367,-364.2392;Inherit;False;RuidoIzq;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-4064.672,1449.797;Inherit;False;25;RuidoDer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-4068.028,1263.162;Inherit;False;26;RuidoIzq;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;51;-2486.734,-316.3192;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-3819.111,1312.443;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;55;-2273.027,-320.2293;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-3570.602,1301.685;Inherit;False;Ruido;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;52;-2089.759,-323.922;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-1916.78,-322.2634;Inherit;False;MascaraBorde;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-768.098,814.7522;Inherit;False;Property;_MaxNew;MaxNew;7;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-784.0372,368.458;Inherit;False;Property;_MaxOld;MaxOld;8;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-786.4912,-55.91763;Inherit;False;33;Ruido;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-778.7239,594.2614;Inherit;False;Property;_MinNew;MinNew;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-788.7346,153.0951;Inherit;False;Property;_MinOld;MinOld;5;0;Create;True;0;0;False;0;0;-2.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-388.6001,-14.8;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;5.992157,0.7529412,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;57;-387.3109,371.6651;Inherit;False;56;MascaraBorde;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;40;-409.2528,154.4599;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;75.67547,95.74592;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;49;413.4569,45.20272;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;p;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;7.8;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;11;0
WireConnection;18;1;12;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;21;0;20;0
WireConnection;21;1;19;0
WireConnection;22;0;17;0
WireConnection;22;1;18;0
WireConnection;24;0;22;0
WireConnection;24;1;35;0
WireConnection;23;0;21;0
WireConnection;23;1;34;0
WireConnection;25;0;24;0
WireConnection;26;0;23;0
WireConnection;51;0;50;2
WireConnection;51;1;53;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;55;0;51;0
WireConnection;33;0;30;0
WireConnection;52;0;55;0
WireConnection;56;0;52;0
WireConnection;40;0;39;0
WireConnection;40;1;41;0
WireConnection;40;2;43;0
WireConnection;40;3;44;0
WireConnection;40;4;45;0
WireConnection;5;0;1;0
WireConnection;5;1;40;0
WireConnection;5;2;57;0
WireConnection;49;2;5;0
ASEEND*/
//CHKSM=3C296FC7A8E11CDD4CBF29789EBDE64974FC2A7C