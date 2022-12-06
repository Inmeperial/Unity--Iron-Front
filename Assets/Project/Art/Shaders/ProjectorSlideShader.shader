// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ProjectorSlideShader"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_InnerRedLines("InnerRedLines", Range( 0 , 1)) = 0.7580875
		_FramePic("FramePic", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _InnerRedLines;
		uniform sampler2D _MainTex;
		uniform sampler2D _FramePic;

		void surf( Input i , inout SurfaceOutput o )
		{
			float InnerRedLocalVar105 = _InnerRedLines;
			float AnimTransitionVar84 = step( i.uv_texcoord.x , InnerRedLocalVar105 );
			float2 panner97 = ( ( 1.0 - InnerRedLocalVar105 ) * float2( 1,0 ) + i.uv_texcoord);
			float4 ImgAndTransVar130 = ( ( 1.0 - AnimTransitionVar84 ) * ( tex2D( _MainTex, panner97 ) * ( 1.0 - tex2D( _FramePic, panner97 ).a ) ) );
			o.Albedo = ImgAndTransVar130.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
301;73;561;668;-1297.994;2668.144;4.872594;False;False
Node;AmplifyShaderEditor.CommentaryNode;56;2753.683,-1838.381;Inherit;False;869.653;580.1884;;6;84;35;36;107;34;105;Effect Transition;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;34;3099.431,-1738.441;Inherit;False;Property;_InnerRedLines;InnerRedLines;1;0;Create;True;0;0;False;0;0.7580875;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;3389.768,-1739.936;Inherit;False;InnerRedLocalVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;129;2407.905,-3916.524;Inherit;False;2405.977;1402.531;;16;130;91;89;123;85;110;94;112;93;1;111;97;99;109;98;108;Image and Transition;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;2464.156,-3135.852;Inherit;False;105;InnerRedLocalVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;2654.661,-3654.901;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;98;2676.512,-3396.609;Inherit;False;Constant;_Vector0;Vector 0;13;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;107;2873.338,-1476.782;Inherit;False;105;InnerRedLocalVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;2823.109,-1758.202;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;109;2735.956,-3175.608;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;36;3166.792,-1573.46;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;97;2901.037,-3454.159;Inherit;False;3;0;FLOAT2;2.38,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;3409.562,-1438.44;Inherit;False;AnimTransitionVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;111;3191.207,-3255.147;Inherit;True;Property;_FramePic;FramePic;12;0;Create;True;0;0;False;0;-1;None;e704fa8a8e5338c42b109cbbf506305c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;85;3669.123,-3789.991;Inherit;True;84;AnimTransitionVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;3186.255,-3547.51;Inherit;True;Property;_MainTex;_MainTex;0;0;Create;True;0;0;False;0;-1;None;c77456150c0dd9a4e8045ef4d255468e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;112;3551.298,-3289.769;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;89;4094.371,-3587.951;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;3833.406,-3381.147;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;4227.058,-3194.773;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;71;2753.523,622.3036;Inherit;False;1629.984;671.9666;;7;78;77;76;75;74;73;72;Fresnel3;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;130;4572.875,-3260.62;Inherit;False;ImgAndTransVar;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;59;2762.703,-999.3042;Inherit;False;1629.984;671.9666;;7;31;57;7;6;2;5;30;Fresnel1;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;62;2756.675,-177.6143;Inherit;False;1629.984;671.9666;;7;69;68;66;65;64;63;67;Fresnel2;1,1,1,1;0;0
Node;AmplifyShaderEditor.FresnelNode;74;3187.523,1002.271;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;4168.686,-937.463;Inherit;False;Fresnel1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;2806.675,218.3518;Inherit;False;Property;_Scale2;Scale2;6;0;Create;True;0;0;False;0;0;6.3;5;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;133;1216.698,-869.3754;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;3251.587,-910.2642;Inherit;False;Property;_ColorOfVintage;ColorOfVintage;2;0;Create;True;0;0;False;0;0.7075472,0.3654875,0.1435119,0;1,0.2436808,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;2812.703,-603.3381;Inherit;False;Property;_Scale;Scale;3;0;Create;True;0;0;False;0;0;4.62;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;65;3190.675,202.3519;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;1287.297,340.2404;Inherit;False;69;Fresnel2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;2831.852,1178.27;Inherit;False;Property;_Power3;Power3;10;0;Create;True;0;0;False;0;0;0.756;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;76;3548.927,872.9512;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;30;3558.106,-748.657;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;4091.448,-2881;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;94;3638.952,-2796.259;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;93;3143.381,-2872.607;Inherit;True;Property;_TextureFrame;TextureFrame;11;0;Create;True;0;0;False;0;-1;None;e704fa8a8e5338c42b109cbbf506305c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;92;1607.298,228.2406;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;2803.523,1018.271;Inherit;False;Property;_Scale3;Scale3;9;0;Create;True;0;0;False;0;0;5.49;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;140;2196.249,-266.4122;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;67;3525.769,224.0417;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;1785.843,-618.3668;Inherit;False;130;ImgAndTransVar;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;1287.297,244.2406;Inherit;False;57;Fresnel1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;3881.912,-949.3047;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;69;4159.021,-115.7731;Inherit;False;Fresnel2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;64;2835.004,378.3515;Inherit;False;Property;_Power2;Power2;7;0;Create;True;0;0;False;0;0;0.391;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;66;3245.559,-88.57426;Inherit;False;Property;_ColorOfVintage2;ColorOfVintage2;5;0;Create;True;0;0;False;0;0.7075472,0.3654875,0.1435119,0;1,0.7533744,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;75;3242.407,711.3437;Inherit;False;Property;_ColorOfVintage3;ColorOfVintage3;8;0;Create;True;0;0;False;0;0.7075472,0.3654875,0.1435119,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;3872.732,672.3033;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;2841.032,-443.3386;Inherit;False;Property;_Power;Power;4;0;Create;True;0;0;False;0;0;0.286;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;983.717,-825.695;Inherit;False;-1;;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;2;3196.703,-619.3381;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;1271.297,436.2404;Inherit;False;78;Fresnel3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;3875.885,-127.6147;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;1963.558,70.52991;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;4149.813,684.1448;Inherit;False;Fresnel3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2415.655,-513.9261;Float;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;ProjectorSlideShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;105;0;34;0
WireConnection;109;0;108;0
WireConnection;36;0;35;1
WireConnection;36;1;107;0
WireConnection;97;0;99;0
WireConnection;97;2;98;0
WireConnection;97;1;109;0
WireConnection;84;0;36;0
WireConnection;111;1;97;0
WireConnection;1;1;97;0
WireConnection;112;0;111;4
WireConnection;89;0;85;0
WireConnection;110;0;1;0
WireConnection;110;1;112;0
WireConnection;91;0;89;0
WireConnection;91;1;110;0
WireConnection;130;0;91;0
WireConnection;74;2;73;0
WireConnection;74;3;72;0
WireConnection;57;0;31;0
WireConnection;133;2;135;0
WireConnection;65;2;63;0
WireConnection;65;3;64;0
WireConnection;76;0;74;0
WireConnection;30;0;2;0
WireConnection;123;1;94;0
WireConnection;94;0;93;0
WireConnection;94;2;93;0
WireConnection;93;1;97;0
WireConnection;92;0;58;0
WireConnection;92;1;70;0
WireConnection;92;2;79;0
WireConnection;140;1;90;0
WireConnection;67;0;65;0
WireConnection;31;0;7;0
WireConnection;31;1;30;0
WireConnection;69;0;68;0
WireConnection;77;0;75;0
WireConnection;77;1;76;0
WireConnection;2;2;6;0
WireConnection;2;3;5;0
WireConnection;68;0;66;0
WireConnection;68;1;67;0
WireConnection;90;1;92;0
WireConnection;78;0;77;0
WireConnection;0;0;131;0
ASEEND*/
//CHKSM=0FB6C7CEDD06225B7887F7528403407FEC10C19C