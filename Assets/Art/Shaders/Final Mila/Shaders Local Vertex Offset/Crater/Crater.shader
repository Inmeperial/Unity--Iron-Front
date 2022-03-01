// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Crater"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_Intensity("Intensity", Float) = 1.5
		_DiametroHoyo("Diametro Hoyo", Float) = 0.82
		_DeformationBorder("Deformation Border", Float) = 1.52
		_MinOld("MinOld", Range( -2 , 2)) = 0
		_MaxOld("MaxOld", Range( -2 , 2)) = 0
		_MinNew("MinNew", Range( -2 , 2)) = 0
		_MaxNew("MaxNew", Range( -2 , 2)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _DiametroHoyo;
		uniform float _MinOld;
		uniform float _MaxOld;
		uniform float _MinNew;
		uniform float _MaxNew;
		uniform float _DeformationBorder;
		uniform float _Intensity;
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
			float simplePerlin2D20 = snoise( v.texcoord.xy*_DeformationBorder );
			simplePerlin2D20 = simplePerlin2D20*0.5 + 0.5;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 OffSet32 = ( ( (_MinNew + (sin( ( ( distance( float2( 0.5,0.5 ) , v.texcoord.xy ) / _DiametroHoyo ) * 0.7 * 6.28318548202515 ) ) - _MinOld) * (_MaxNew - _MinNew) / (_MaxOld - _MinOld)) * simplePerlin2D20 ) * ase_vertexNormal );
			v.vertex.xyz += OffSet32;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _TextureSample1, uv_TextureSample1 ), _Intensity );
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
0;20;1807;717;3447.933;-44.62395;2.546827;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2982.299,562.136;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;9;-2932.365,296.4711;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;13;-2675.36,609.4211;Inherit;False;Property;_DiametroHoyo;Diametro Hoyo;8;0;Create;True;0;0;False;0;0.82;0.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;10;-2691.283,392.7791;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;18;-2413.279,849.98;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2413.248,629.1479;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;12;-2437.81,403.3043;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2170.248,543.1479;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2010.616,1013.963;Inherit;False;Property;_MaxNew;MaxNew;13;0;Create;True;0;0;False;0;0;1;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;17;-1967.979,534.9271;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1686.733,1245.141;Inherit;False;Property;_DeformationBorder;Deformation Border;9;0;Create;True;0;0;False;0;1.52;1.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1989.552,764.6357;Inherit;False;Property;_MinOld;MinOld;10;0;Create;True;0;0;False;0;0;0.91;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1996.288,845.6276;Inherit;False;Property;_MaxOld;MaxOld;11;0;Create;True;0;0;False;0;0;1;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2005.69,928.4873;Inherit;False;Property;_MinNew;MinNew;12;0;Create;True;0;0;False;0;0;0;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1692.52,977.9169;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;20;-1409.695,990.2902;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-1686.096,747.6073;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.91;False;2;FLOAT;1.03;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1152.108,823.4139;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;26;-1161.604,1041.052;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-928.9886,922.1887;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-744.2798,262.704;Inherit;False;Property;_Intensity;Intensity;7;0;Create;True;0;0;False;0;1.5;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-704.2605,913.1573;Inherit;False;OffSet;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-533.5,132.5;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;-1;None;2394c9eabb5f7de40811085368f1142a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-537.5,-61.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;-1;None;4a1fa457a1cf7d24ab43a6a3b9d33f2e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;33;-238.0145,324.2784;Inherit;False;32;OffSet;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Crater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;12;0;10;0
WireConnection;12;1;13;0
WireConnection;15;0;12;0
WireConnection;15;1;16;0
WireConnection;15;2;18;0
WireConnection;17;0;15;0
WireConnection;20;0;23;0
WireConnection;20;1;24;0
WireConnection;19;0;17;0
WireConnection;19;1;27;0
WireConnection;19;2;28;0
WireConnection;19;3;29;0
WireConnection;19;4;31;0
WireConnection;21;0;19;0
WireConnection;21;1;20;0
WireConnection;25;0;21;0
WireConnection;25;1;26;0
WireConnection;32;0;25;0
WireConnection;2;5;3;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;11;33;0
ASEEND*/
//CHKSM=AA1C1D7F82B0DBDCA4E8B13BB953629EB5AD1F8A