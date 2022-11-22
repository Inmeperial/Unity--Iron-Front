// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OutLine Shader"
{
	Properties
	{
		[Toggle]_isOutLineOn("isOutLineOn", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input
		{
			half filler;
		};
		uniform float _isOutLineOn;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = (( _isOutLineOn )?( 0.09 ):( (float)0 ));
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float4 color3 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			o.Emission = color3.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color8 = IsGammaSpace() ? float4(0.3619615,0.9245283,0.8718572,0) : float4(0.1077518,0.8368256,0.7328666,0);
			o.Albedo = color8.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;583;938;1218.589;120.8392;1.324043;False;False
Node;AmplifyShaderEditor.IntNode;1;-825.433,463.1494;Inherit;False;Constant;_Int0;Int 0;8;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-888.1088,591.1495;Inherit;False;Constant;_FloatOutLine;FloatOutLine;1;0;Create;True;0;0;False;0;0.09;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;4;-569.433,511.1496;Inherit;False;Property;_isOutLineOn;isOutLineOn;0;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-809.433,223.1499;Inherit;False;Constant;_ColorOutLine;ColorOutLine;7;0;Create;True;0;0;False;0;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OutlineNode;5;-377.4326,335.1495;Inherit;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;-400.9463,-79.90601;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.3619615,0.9245283,0.8718572,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;OutLine Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;5;0;3;0
WireConnection;5;1;4;0
WireConnection;0;0;8;0
WireConnection;0;11;5;0
ASEEND*/
//CHKSM=148F9C202697619DF0EF42D6F9A104AB436EF17D