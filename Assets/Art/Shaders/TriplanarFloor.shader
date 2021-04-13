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
			float3 ase_worldPos = i.worldPos;
			float4 lerpResult20 = lerp( tex2D( _Grass, uv_Grass ) , tex2D( _Terrain, uv_Terrain ) , float4( saturate( ase_worldPos ) , 0.0 ));
			o.Albedo = saturate( lerpResult20 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
561;73;756;666;1174.821;947.5075;2.237238;False;False
Node;AmplifyShaderEditor.WorldPosInputsNode;27;-838.2324,254.4226;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;17;-881.8638,-39.61579;Inherit;True;Property;_Terrain;Terrain;0;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-921.6959,-239.1365;Inherit;True;Property;_Grass;Grass;2;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;29;-588.6896,278.2986;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;20;-392.6057,-6.834968;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;40;-866.0229,-488.9213;Inherit;True;Property;_Alpha3;Alpha1;5;0;Create;True;0;0;False;0;-1;41c9f08c66bb48d4c8f7e06583c9b701;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;28;97.42522,209.4569;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;41;-957.8779,-749.7206;Inherit;True;Property;_Grass1;Grass;3;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-1146.415,462.7186;Inherit;True;Property;_Alpha1;Alpha1;4;0;Create;True;0;0;False;0;-1;41c9f08c66bb48d4c8f7e06583c9b701;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-44.91402,386.944;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;32;-795.3061,885.2694;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-523.3204,-574.7598;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;33;-1145.737,696.2718;Inherit;True;Property;_Alpha2;Alpha2;1;0;Create;True;0;0;False;0;-1;939aa491bfac0da49bb09562b9498276;939aa491bfac0da49bb09562b9498276;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;31;-1022.107,970.9683;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCGrayscale;38;-347.584,575.1718;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;-608.956,558.619;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;418.6974,129.7095;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;27;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;20;2;29;0
WireConnection;28;0;20;0
WireConnection;37;0;20;0
WireConnection;37;1;38;0
WireConnection;32;0;31;0
WireConnection;39;0;41;0
WireConnection;38;0;35;0
WireConnection;35;0;34;0
WireConnection;35;1;33;0
WireConnection;35;2;32;0
WireConnection;0;0;28;0
ASEEND*/
//CHKSM=AD2A7AFD11C338445900E9F217E554A7A53BB578