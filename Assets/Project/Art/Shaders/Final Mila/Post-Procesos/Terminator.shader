// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Terminator"
{
	Properties
	{
		_MaxOld("MaxOld", Range( 0 , 3)) = 3
		_Float1("Float 1", Float) = 0
		_Float2("Float 2", Float) = 0
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
			#include "UnityShaderVariables.cginc"

		
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
			
			uniform float _MaxOld;
			uniform float _Float1;
			uniform float _Float2;


					float2 voronoihash25( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi25( float2 v, float time, inout float2 id, float smoothness )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mr = 0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash25( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						 		}
						 	}
						}
						return F1;
					}
			
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
				float4 color4 = IsGammaSpace() ? float4(1,0.1839623,0.1839623,0) : float4(1,0.02832596,0.02832596,0);
				float2 uv05 = i.texcoord.xy * float2( 2,2 ) + float2( -1,-1 );
				float temp_output_6_0 = distance( uv05 , float2( 0,0 ) );
				float time25 = 1.24;
				float2 uv021 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords25 = uv021 * 11.16;
				float2 id25 = 0;
				float voroi25 = voronoi25( coords25, time25,id25, 0 );
				float2 temp_cast_0 = (voroi25).xx;
				float simplePerlin2D23 = snoise( temp_cast_0*0.13 );
				simplePerlin2D23 = simplePerlin2D23*0.5 + 0.5;
				float lerpResult27 = lerp( (_Float1 + (temp_output_6_0 - ( (-0.28 + (simplePerlin2D23 - 0.45) * (1.0 - -0.28) / (0.73 - 0.45)) * ( _CosTime.w + -3.0 ) )) * (_Float2 - _Float1) / (_MaxOld - ( (-0.28 + (simplePerlin2D23 - 0.45) * (1.0 - -0.28) / (0.73 - 0.45)) * ( _CosTime.w + -3.0 ) ))) , 0.0 , temp_output_6_0);
				

				float4 color = ( ( tex2D( _MainTex, uv_MainTex ) * color4 ) * -lerpResult27 );
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
240;447;1807;662;2760.425;205.87;1.886744;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2991.387,-26.3771;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;25;-2718.268,-24.3616;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;1.24;False;2;FLOAT;11.16;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.RangedFloatNode;24;-2659.493,245.214;Inherit;False;Constant;_Float4;Float 4;4;0;Create;True;0;0;False;0;0.13;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosTime;16;-2436.873,527.3383;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-2316.121,730.0297;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;False;0;-3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;23;-2378.449,198.9072;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;26;-2085.249,243.0225;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.45;False;2;FLOAT;0.73;False;3;FLOAT;-0.28;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2091.755,-176.0632;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-2107.679,533.0884;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;6;-1760.847,-139.5677;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1685.045,405.1483;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1259.553,735.3038;Inherit;False;Property;_Float1;Float 1;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1273.333,506.7444;Inherit;False;Property;_MaxOld;MaxOld;0;0;Create;True;0;0;False;0;3;8;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1129.615,1035.612;Inherit;False;Property;_Float2;Float 2;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-865.3254,-215.7841;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;7;-872.579,391.5785;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.26;False;2;FLOAT;7.72;False;3;FLOAT;1.15;False;4;FLOAT;2.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;-469.9187,318.6446;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-504.5,-149;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-475.8021,55.26908;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,0.1839623,0.1839623,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-202.0647,-6.580827;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NegateNode;9;-191.5317,423.5027;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;214.9852,37.46541;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;554.7632,6.650011;Float;False;True;-1;2;ASEMaterialInspector;0;2;Terminator;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;25;0;21;0
WireConnection;23;0;25;0
WireConnection;23;1;24;0
WireConnection;26;0;23;0
WireConnection;17;0;16;4
WireConnection;17;1;18;0
WireConnection;6;0;5;0
WireConnection;19;0;26;0
WireConnection;19;1;17;0
WireConnection;7;0;6;0
WireConnection;7;1;19;0
WireConnection;7;2;11;0
WireConnection;7;3;14;0
WireConnection;7;4;15;0
WireConnection;27;0;7;0
WireConnection;27;2;6;0
WireConnection;2;0;1;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;9;0;27;0
WireConnection;8;0;3;0
WireConnection;8;1;9;0
WireConnection;0;0;8;0
ASEEND*/
//CHKSM=3E39186DB0BAE4F9351CE9C8C3A2E1033966B6AE