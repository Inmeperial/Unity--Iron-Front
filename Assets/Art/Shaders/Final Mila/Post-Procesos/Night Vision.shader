// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Nigth Vision"
{
	Properties
	{
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Cull Off
		ZWrite Off
		ZTest Always
		
		Pass
		{
			CGPROGRAM

			

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			
		
			struct ASEAttributesDefault
			{
				float3 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
			};

			struct ASEVaryingsDefault
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			

			
			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV (v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex (o.texcoord, 1.0);

				v.texcoord = o.texcoordStereo;
				float4 ase_ppsScreenPosVertexNorm = float4(o.texcoordStereo,0,1);

				

				return o;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.texcoordStereo,0,1);

				float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 color5 = IsGammaSpace() ? float4(0.2306426,0.6698113,0.3143435,0) : float4(0.04346795,0.4061945,0.08054288,0);
				

				float4 color = ( tex2D( _MainTex, uv_MainTex ) * color5 );
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
33;238;1807;781;999.5;390;1;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-591.5,-232;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-395.5,-166;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-299.5,181;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.2306426,0.6698113,0.3143435,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-46.5,52;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;300,10;Float;False;True;-1;2;ASEMaterialInspector;0;2;Nigth Vision;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;2;0;3;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;1;0;4;0
ASEEND*/
//CHKSM=E95DA4428658646FC44C40E7E0516103531DBBBD