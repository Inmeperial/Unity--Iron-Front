// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sepia"
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
				float4 color9 = IsGammaSpace() ? float4(0.9528302,0.8267707,0.5348434,0) : float4(0.8960326,0.6502011,0.2477207,0);
				float2 uv012 = i.texcoord.xy * float2( 10,10 ) + float2( 5,10 );
				float simplePerlin2D17 = snoise( uv012*5.0 );
				simplePerlin2D17 = simplePerlin2D17*0.5 + 0.5;
				float4 temp_cast_0 = (saturate( ( 1.0 - (0.0 + (step( simplePerlin2D17 , 0.14 ) - 0.0) * (0.74 - 0.0) / (5.65 - 0.0)) ) )).xxxx;
				float2 uv039 = i.texcoord.xy * float2( 10.5,10.5 ) + float2( 2,2 );
				float simplePerlin2D44 = snoise( uv039*10.0 );
				simplePerlin2D44 = simplePerlin2D44*0.5 + 0.5;
				float4 temp_cast_1 = (saturate( ( 1.0 - (0.0 + (step( simplePerlin2D44 , 0.14 ) - 0.0) * (0.74 - 0.0) / (3.84 - 0.0)) ) )).xxxx;
				float mulTime31 = _Time.y * 20.0;
				float4 lerpResult23 = lerp( temp_cast_0 , temp_cast_1 , cos( mulTime31 ));
				float mulTime54 = _Time.y * 0.54;
				float2 temp_cast_2 = (-0.21).xx;
				float2 temp_cast_3 = (2).xx;
				float2 uv052 = i.texcoord.xy * temp_cast_3 + float2( 0,0 );
				float2 temp_cast_4 = (uv052.x).xx;
				float2 panner55 = ( mulTime54 * temp_cast_2 + temp_cast_4);
				float simplePerlin2D57 = snoise( panner55*50.0 );
				simplePerlin2D57 = simplePerlin2D57*0.5 + 0.5;
				float Franjas61 = saturate( ( 1.0 - (0.0 + (step( simplePerlin2D57 , 0.24 ) - 0.0) * (0.27 - 0.0) / (5.0 - 0.0)) ) );
				

				float4 color = ( tex2D( _MainTex, uv_MainTex ) * color9 * lerpResult23 * Franjas61 );
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
42;564;1807;793;4966.925;-480.5578;2.586127;True;False
Node;AmplifyShaderEditor.RangedFloatNode;51;-3043.431,2144.896;Inherit;False;Constant;_Float8;Float 4;2;0;Create;True;0;0;False;0;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-3201.737,1619.367;Inherit;False;Constant;_Vector3;Vector 0;2;0;Create;True;0;0;False;0;0,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;54;-2874.406,2148.614;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-2998.94,-71.91095;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;10,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;53;-2997.369,1918.943;Inherit;False;Constant;_Float3;Float 0;2;0;Create;True;0;0;False;0;-0.21;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-3018.492,1649.605;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;37;-3772.399,904.0707;Inherit;False;Constant;_Vector1;Vector 0;2;0;Create;True;0;0;False;0;10.5,10.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;49;-3754.228,1176.172;Inherit;False;Constant;_Vector2;Vector 0;2;0;Create;True;0;0;False;0;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;56;-2634.942,1901.006;Inherit;False;Constant;_Float9;Float 5;2;0;Create;True;0;0;False;0;50;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2574.93,-6.732566;Inherit;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;55;-2709.537,1678.761;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-3301.276,1023.802;Inherit;False;Constant;_Float7;Float 5;2;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-3544.774,940.0675;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;5,10;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-2826.398,-92.51059;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;5,10;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;17;-2390.54,-96.23726;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;44;-3110.496,927.8773;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;57;-2398.011,1703.068;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;58;-2157.032,1709.039;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;18;-2149.561,-89.26636;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;45;-2869.517,934.8482;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.14;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-1929.508,-97.48445;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;5.65;False;3;FLOAT;0;False;4;FLOAT;0.74;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;59;-1927.047,1700.821;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;5;False;3;FLOAT;0;False;4;FLOAT;0.27;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1719.903,940.4346;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;46;-2649.464,926.6301;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;3.84;False;3;FLOAT;0;False;4;FLOAT;0.74;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;-2385.016,934.6144;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;22;-1665.06,-89.50015;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;31;-1560.694,942.9132;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;63;-1603.784,1707.531;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;30;-1397.22,940.7852;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-1507.941,-90.00246;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;48;-2227.897,934.1121;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;60;-1335.487,1707.749;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;7;-1301.816,-414.2461;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-990.4941,-27.75365;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.9528302,0.8267707,0.5348434,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1018.944,-267.269;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-1158.47,1709.62;Inherit;False;Franjas;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-1026.18,443.6682;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-603.882,12.52921;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;-15,11;Float;False;True;-1;2;ASEMaterialInspector;0;2;Sepia;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;54;0;51;0
WireConnection;52;0;50;2
WireConnection;55;0;52;1
WireConnection;55;2;53;0
WireConnection;55;1;54;0
WireConnection;39;0;37;0
WireConnection;39;1;49;0
WireConnection;12;0;11;0
WireConnection;17;0;12;0
WireConnection;17;1;15;0
WireConnection;44;0;39;0
WireConnection;44;1;42;0
WireConnection;57;0;55;0
WireConnection;57;1;56;0
WireConnection;58;0;57;0
WireConnection;18;0;17;0
WireConnection;45;0;44;0
WireConnection;19;0;18;0
WireConnection;59;0;58;0
WireConnection;46;0;45;0
WireConnection;47;0;46;0
WireConnection;22;0;19;0
WireConnection;31;0;32;0
WireConnection;63;0;59;0
WireConnection;30;0;31;0
WireConnection;20;0;22;0
WireConnection;48;0;47;0
WireConnection;60;0;63;0
WireConnection;6;0;7;0
WireConnection;61;0;60;0
WireConnection;23;0;20;0
WireConnection;23;1;48;0
WireConnection;23;2;30;0
WireConnection;8;0;6;0
WireConnection;8;1;9;0
WireConnection;8;2;23;0
WireConnection;8;3;61;0
WireConnection;2;0;8;0
ASEEND*/
//CHKSM=50969EF23274A2ED3BFDB92427716CD6714C5E98