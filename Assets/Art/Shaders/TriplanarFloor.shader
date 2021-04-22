// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Triplanar"
{
	Properties
	{
		_TextureUp("TextureUp", 2D) = "white" {}
		_TextureDown("TextureDown", 2D) = "white" {}
		_NormalUp("NormalUp", 2D) = "white" {}
		_NormalDown("NormalDown", 2D) = "white" {}
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

		uniform sampler2D _NormalDown;
		uniform float4 _NormalDown_ST;
		uniform sampler2D _NormalUp;
		uniform float4 _NormalUp_ST;
		uniform sampler2D _TextureDown;
		uniform float4 _TextureDown_ST;
		uniform sampler2D _TextureUp;
		uniform float4 _TextureUp_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalDown = i.uv_texcoord * _NormalDown_ST.xy + _NormalDown_ST.zw;
			float2 uv_NormalUp = i.uv_texcoord * _NormalUp_ST.xy + _NormalUp_ST.zw;
			float3 temp_cast_0 = (-0.56).xxx;
			float3 temp_cast_1 = (2.0).xxx;
			float3 ase_worldPos = i.worldPos;
			float3 smoothstepResult143 = smoothstep( temp_cast_0 , temp_cast_1 , ase_worldPos);
			float grayscale38 = Luminance(saturate( smoothstepResult143 ));
			float4 lerpResult138 = lerp( tex2D( _NormalDown, uv_NormalDown ) , tex2D( _NormalUp, uv_NormalUp ) , grayscale38);
			float4 NormalVar168 = lerpResult138;
			o.Normal = NormalVar168.rgb;
			float2 uv_TextureDown = i.uv_texcoord * _TextureDown_ST.xy + _TextureDown_ST.zw;
			float2 uv_TextureUp = i.uv_texcoord * _TextureUp_ST.xy + _TextureUp_ST.zw;
			float3 temp_cast_3 = (-0.56).xxx;
			float3 temp_cast_4 = (1.0).xxx;
			float3 smoothstepResult112 = smoothstep( temp_cast_3 , temp_cast_4 , ase_worldPos);
			float4 lerpResult20 = lerp( tex2D( _TextureDown, uv_TextureDown ) , tex2D( _TextureUp, uv_TextureUp ) , float4( saturate( smoothstepResult112 ) , 0.0 ));
			float4 AlbedoVar117 = lerpResult20;
			o.Albedo = AlbedoVar117.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;556;938;105.6067;348.5079;2.847821;False;False
Node;AmplifyShaderEditor.CommentaryNode;167;71.63401,560.3706;Inherit;False;1224.867;1369.544;Comment;7;138;34;38;166;155;143;141;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;116;27.66533,-689.5591;Inherit;False;1295.604;1025.629;TriPlanar;9;112;114;20;17;18;27;117;125;153;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;142;136.4299,1636.675;Inherit;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;141;182.6511,1309.92;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;140;129.1208,1748.449;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;False;0;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;68.76424,219.5175;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;27;104.1581,-95.20828;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;125;76.07332,107.7428;Inherit;False;Constant;_Float5;Float 5;9;0;Create;True;0;0;False;0;-0.56;-0.7157001;-2;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;143;432.0952,1426.667;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;155;568.904,1302.539;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;112;400.2176,-113.6561;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;34;545.4686,903.0439;Inherit;True;Property;_NormalUp;NormalUp;2;0;Create;True;0;0;False;0;-1;5e8b6878fb7b02844bc5f04799942ca8;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;38;692.81,1170.758;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;166;533.275,613.0455;Inherit;True;Property;_NormalDown;NormalDown;3;0;Create;True;0;0;False;0;-1;f92f5c81820409b41a637c769d326a7c;41c9f08c66bb48d4c8f7e06583c9b701;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;181.014,-406.6425;Inherit;True;Property;_TextureUp;TextureUp;0;0;Create;True;0;0;False;0;-1;f65aaa57caa82244d8711f190f2e61aa;4a9a63b8916ca1b45b78790a93ecb63c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;183.7505,-638.201;Inherit;True;Property;_TextureDown;TextureDown;1;0;Create;True;0;0;False;0;-1;4a9a63b8916ca1b45b78790a93ecb63c;f65aaa57caa82244d8711f190f2e61aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;153;524.4636,-269.9815;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;138;915.193,923.4479;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;807.7594,-590.5364;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;1093.273,-589.8612;Inherit;False;AlbedoVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;1043.746,644.2113;Inherit;False;NormalVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;1523.572,450.9339;Inherit;False;168;NormalVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;1548.695,345.2562;Inherit;False;117;AlbedoVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1842.275,358.9763;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Triplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;143;0;141;0
WireConnection;143;1;142;0
WireConnection;143;2;140;0
WireConnection;155;0;143;0
WireConnection;112;0;27;0
WireConnection;112;1;125;0
WireConnection;112;2;114;0
WireConnection;38;0;155;0
WireConnection;153;0;112;0
WireConnection;138;0;166;0
WireConnection;138;1;34;0
WireConnection;138;2;38;0
WireConnection;20;0;18;0
WireConnection;20;1;17;0
WireConnection;20;2;153;0
WireConnection;117;0;20;0
WireConnection;168;0;138;0
WireConnection;0;0;118;0
WireConnection;0;1;169;0
ASEEND*/
//CHKSM=B4E63A3DC8B7EE9901A4BC5B0E96371889F87BDE