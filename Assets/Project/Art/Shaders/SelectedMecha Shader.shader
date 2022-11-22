// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FresnelSelected"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0.4917288,1,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.15
		_Color0("Color 0", Color) = (1,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input {
			half filler;
		};
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			half filler;
		};

		uniform float4 _Color0;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Alpha = _Color0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;752;938;723.5952;249.9827;1;True;False
Node;AmplifyShaderEditor.SaturateNode;6;-207.3622,-203.6904;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1038.652,-279.1606;Inherit;False;Constant;_Scale;Scale;3;0;Create;True;0;0;False;0;1;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1036.267,-126.7407;Inherit;False;Constant;_PowerFresnel;PowerFresnel;2;0;Create;True;0;0;False;0;3;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-653.7269,-262.8551;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-288.2165,-325.6924;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-333.8832,156.2878;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2.271945,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FresnelSelected;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.15;0,0.4917288,1,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;1;0
WireConnection;1;2;4;0
WireConnection;1;3;5;0
WireConnection;3;1;1;0
WireConnection;0;9;2;0
ASEEND*/
//CHKSM=1F1A9945B262735D609CFD18C157C6EC52D37272