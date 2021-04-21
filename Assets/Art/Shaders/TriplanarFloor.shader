// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplanar"
{
	Properties
	{
		_Terrain("Terrain", 2D) = "white" {}
		_Grass("Grass", 2D) = "white" {}
		_Alpha1("Alpha1", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
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

		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _Alpha1;
		uniform float4 _Alpha1_ST;
		uniform sampler2D _Terrain;
		uniform float4 _Terrain_ST;
		uniform sampler2D _Grass;
		uniform float4 _Grass_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float2 uv_Alpha1 = i.uv_texcoord * _Alpha1_ST.xy + _Alpha1_ST.zw;
			float3 temp_cast_0 = (-0.56).xxx;
			float3 temp_cast_1 = (2.0).xxx;
			float3 ase_worldPos = i.worldPos;
			float3 smoothstepResult143 = smoothstep( temp_cast_0 , temp_cast_1 , ase_worldPos);
			float grayscale38 = Luminance(saturate( smoothstepResult143 ));
			float4 lerpResult138 = lerp( tex2D( _TextureSample3, uv_TextureSample3 ) , tex2D( _Alpha1, uv_Alpha1 ) , grayscale38);
			o.Normal = lerpResult138.rgb;
			float2 uv_Terrain = i.uv_texcoord * _Terrain_ST.xy + _Terrain_ST.zw;
			float2 uv_Grass = i.uv_texcoord * _Grass_ST.xy + _Grass_ST.zw;
			float3 temp_cast_3 = (-0.56).xxx;
			float3 temp_cast_4 = (1.0).xxx;
			float3 smoothstepResult112 = smoothstep( temp_cast_3 , temp_cast_4 , ase_worldPos);
			float4 lerpResult20 = lerp( tex2D( _Terrain, uv_Terrain ) , tex2D( _Grass, uv_Grass ) , float4( saturate( smoothstepResult112 ) , 0.0 ));
			float4 TriPlanar117 = lerpResult20;
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
0;73;558;938;-1073.158;-375.3217;1.3;False;False
Node;AmplifyShaderEditor.CommentaryNode;116;27.66533,-689.5591;Inherit;False;1295.604;1025.629;TriPlanar;9;112;114;20;17;18;27;117;125;153;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;114;68.76424,219.5175;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;104.1581,-95.20828;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;125;76.07332,107.7428;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;400.2176,-113.6561;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;141;619.6098,1490.193;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;142;639.184,1651.143;Inherit;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;140;631.875,1762.917;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;153;524.4636,-269.9815;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;17;181.6052,-621.1633;Inherit;True;Property;_Terrain;Terrain;0;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;164.2215,-408.3331;Inherit;True;Property;_Grass;Grass;3;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;143;934.8492,1441.135;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;155;1140.38,1338.708;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;20;807.7594,-590.5364;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;1093.273,-589.8612;Inherit;False;TriPlanar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;38;1264.286,1206.927;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;1012.474,997.3682;Inherit;True;Property;_Alpha1;Alpha1;5;0;Create;True;0;0;False;0;-1;5e8b6878fb7b02844bc5f04799942ca8;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;166;1010.296,770.6995;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;-1;f92f5c81820409b41a637c769d326a7c;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;138;1486.669,959.6174;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;131;-829.8016,606.9297;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;152;-653.446,441.0387;Inherit;False;2;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-481.5473,781.4304;Inherit;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;5.531263;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;160;-407.2271,-636.22;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;132;-530.0547,523.5609;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;146;-555.8379,922.676;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;147;-284.2803,956.248;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;1548.695,345.2562;Inherit;False;117;TriPlanar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;165;1441.932,70.23491;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;161;-141.3615,-664.1724;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;159;-181.0236,553.0928;Inherit;False;NormalCreate;1;;1;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1842.275,358.9763;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;112;0;27;0
WireConnection;112;1;125;0
WireConnection;112;2;114;0
WireConnection;153;0;112;0
WireConnection;143;0;141;0
WireConnection;143;1;142;0
WireConnection;143;2;140;0
WireConnection;155;0;143;0
WireConnection;20;0;17;0
WireConnection;20;1;18;0
WireConnection;20;2;153;0
WireConnection;117;0;20;0
WireConnection;38;0;155;0
WireConnection;138;0;166;0
WireConnection;138;1;34;0
WireConnection;138;2;38;0
WireConnection;132;0;131;0
WireConnection;147;0;146;1
WireConnection;147;1;146;3
WireConnection;161;0;160;0
WireConnection;0;0;118;0
WireConnection;0;1;138;0
ASEEND*/
//CHKSM=CF6B87164E2C2BDEF0361A8CA24A95B140452E11