// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SmokeBomb Shader"
{
	Properties
	{
		_Vector0("Vector 0", Vector) = (50,30,0,0)
		_OpacityTotal("OpacityTotal", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float2 _Vector0;
		uniform float _OpacityTotal;


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
			float4 color8 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 color16 = IsGammaSpace() ? float4(0.245283,0.1423104,0.1423104,0) : float4(0.0490081,0.01788768,0.01788768,0);
			float2 temp_cast_0 = (1.0).xx;
			float2 uv_TexCoord10 = i.uv_texcoord * ( 1.0 - _Vector0 );
			float2 panner12 = ( _Time.y * temp_cast_0 + uv_TexCoord10);
			float simplePerlin2D4 = snoise( panner12*0.5 );
			simplePerlin2D4 = simplePerlin2D4*0.5 + 0.5;
			float4 lerpResult15 = lerp( color8 , color16 , simplePerlin2D4);
			o.Albedo = lerpResult15.rgb;
			o.Alpha = ( simplePerlin2D4 * _OpacityTotal );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;716;938;1343.276;417.4512;1;False;False
Node;AmplifyShaderEditor.Vector2Node;11;-1800.452,-393.4515;Inherit;False;Property;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;50,30;50,30;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;6;-1713.318,-94.97033;Inherit;False;Constant;_Speed;Speed;1;0;Create;True;0;0;False;0;1;1.032998;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;-1608.854,-367.1653;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1438.813,-365.9617;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1298.918,3.029687;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;12;-1118.617,-270.1672;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1103.742,210.1713;Inherit;False;Constant;_NoiseTotal;NoiseTotal;2;0;Create;True;0;0;False;0;0.5;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-802.5554,-496.9027;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;4;-680.3858,-6.685331;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-802.4046,-280.592;Inherit;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;0.245283,0.1423104,0.1423104,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-570.959,360.9273;Inherit;False;Property;_OpacityTotal;OpacityTotal;1;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-275.0204,237.2016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-404.0453,-401.2941;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;3;-16.40619,-120.312;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SmokeBomb Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;11;0
WireConnection;10;0;21;0
WireConnection;5;0;6;0
WireConnection;12;0;10;0
WireConnection;12;2;6;0
WireConnection;12;1;5;0
WireConnection;4;0;12;0
WireConnection;4;1;14;0
WireConnection;19;0;4;0
WireConnection;19;1;20;0
WireConnection;15;0;8;0
WireConnection;15;1;16;0
WireConnection;15;2;4;0
WireConnection;3;0;15;0
WireConnection;3;9;19;0
ASEEND*/
//CHKSM=0BA8B8D5C4D2DCD3F14943EC5D98BE89F77A8ABA