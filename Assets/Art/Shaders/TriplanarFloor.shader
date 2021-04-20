// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplanar"
{
	Properties
	{
		_Terrain("Terrain", 2D) = "white" {}
		_Grass("Grass", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Grass;
		uniform float4 _Grass_ST;
		uniform sampler2D _Terrain;
		uniform float4 _Terrain_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Grass = i.uv_texcoord * _Grass_ST.xy + _Grass_ST.zw;
			float2 uv_Terrain = i.uv_texcoord * _Terrain_ST.xy + _Terrain_ST.zw;
			float3 temp_cast_0 = (-0.56).xxx;
			float3 temp_cast_1 = (2.0).xxx;
			float3 ase_worldPos = i.worldPos;
			float3 smoothstepResult112 = smoothstep( temp_cast_0 , temp_cast_1 , ase_worldPos);
			float4 lerpResult20 = lerp( tex2D( _Grass, uv_Grass ) , tex2D( _Terrain, uv_Terrain ) , float4( ( 1.0 - smoothstepResult112 ) , 0.0 ));
			float4 TriPlanar117 = saturate( lerpResult20 );
			o.Albedo = TriPlanar117.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;1057;938;-910.4342;418.0628;1.310818;False;False
Node;AmplifyShaderEditor.CommentaryNode;116;2025.137,-29.22088;Inherit;False;1295.604;1025.629;TriPlanar;10;112;28;114;20;17;18;27;117;124;125;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;2076.891,534.9344;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;125;2070.243,730.6631;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;2069.128,846.5986;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;False;0;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;2458.619,528.9908;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;17;2129.283,275.7872;Inherit;True;Property;_Terrain;Terrain;0;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;2101.656,28.46418;Inherit;True;Property;_Grass;Grass;1;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;124;2509.031,360.2386;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;20;2698.665,60.99751;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;28;2964.406,89.18669;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;3004.789,382.2308;Inherit;False;TriPlanar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;1322.661,88.90919;Inherit;False;117;TriPlanar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1583.322,108.9526;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;112;0;27;0
WireConnection;112;1;125;0
WireConnection;112;2;114;0
WireConnection;124;0;112;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;20;2;124;0
WireConnection;28;0;20;0
WireConnection;117;0;28;0
WireConnection;0;0;118;0
ASEEND*/
//CHKSM=672C6BF846907E61F08113835D559334D59DDD60