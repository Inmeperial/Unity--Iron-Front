// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CanMoveToNode Shader"
{
	Properties
	{
		[HDR]_Color1("Color 0", Color) = (0,0.7448263,1,0)
		_MinBright1("MinBright", Range( 0 , 1)) = 0.2
		_MaxBright1("MaxBright", Range( 0 , 1)) = 0.9
		[IntRange]_TimeCicle1("TimeCicle", Range( 0 , 10)) = 4
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			half filler;
		};

		uniform float4 _Color1;
		uniform float _TimeCicle1;
		uniform float _MinBright1;
		uniform float _MaxBright1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime30 = _Time.y * _TimeCicle1;
			float clampResult34 = clamp( sin( mulTime30 ) , _MinBright1 , _MaxBright1 );
			o.Emission = saturate( ( _Color1 * clampResult34 ) ).rgb;
			o.Alpha = saturate( clampResult34 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;847;938;2155.898;998.5396;2.788339;True;False
Node;AmplifyShaderEditor.CommentaryNode;28;-1334.817,151.9746;Inherit;False;970.2416;488.4174;ClampWithTime;6;34;33;32;31;30;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1314.538,211.7301;Inherit;False;Property;_TimeCicle1;TimeCicle;3;1;[IntRange];Create;True;0;0;False;0;4;8;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;30;-1075.26,308.8434;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;31;-938.601,213.3888;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1193.689,400.8878;Inherit;False;Property;_MinBright1;MinBright;1;0;Create;True;0;0;False;0;0.2;0.178;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1198.446,508.8393;Inherit;False;Property;_MaxBright1;MaxBright;2;0;Create;True;0;0;False;0;0.9;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;34;-735.168,365.0428;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;-745.4286,-317.6475;Inherit;False;Property;_Color1;Color 0;0;1;[HDR];Create;True;0;0;False;0;0,0.7448263,1,0;0,1,0.9917969,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-467.765,-146.8572;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;37;-202.0953,36.90468;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;38;-274.0599,234.0672;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;CanMoveToNode Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;29;0
WireConnection;31;0;30;0
WireConnection;34;0;31;0
WireConnection;34;1;32;0
WireConnection;34;2;33;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;37;0;36;0
WireConnection;38;0;34;0
WireConnection;0;2;37;0
WireConnection;0;9;38;0
ASEEND*/
//CHKSM=275839F7C0627FA0FB80AD99EBE24F754682AE2A