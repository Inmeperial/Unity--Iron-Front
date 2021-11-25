// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water Shader"
{
	Properties
	{
		_WaveSpeed("Wave Speed", Float) = 1
		_WaveTile("WaveTile", Float) = 1
		_Tessellation("Tessellation", Float) = 8
		_WaterColorTop("Water Color Top", Color) = (0,0,0,0)
		_WaterColorBottom("Water Color Bottom", Color) = (0.04454434,0.1623221,0.7264151,0)
		_WaveForm("Wave Form", Vector) = (0.23,0.09,0,0)
		_WaveDirection("Wave Direction", Vector) = (-1,0.1,0,0)
		_WaveUp("Wave Up", Vector) = (0,1,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _WaveSpeed;
		uniform float2 _WaveDirection;
		uniform float2 _WaveForm;
		uniform float _WaveTile;
		uniform float3 _WaveUp;
		uniform sampler2D _TextureSample0;
		uniform float4 _WaterColorBottom;
		uniform float4 _WaterColorTop;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Tessellation;


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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_2 = (_Tessellation).xxxx;
			return temp_cast_2;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult3 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile4 = appendResult3;
			float2 panner14 = ( ( _Time.y * _WaveSpeed ) * _WaveDirection + ( ( WorldSpaceTile4 * float4( _WaveForm, 0.0 , 0.0 ) ) * _WaveTile ).xy);
			float simplePerlin2D16 = snoise( panner14 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			v.vertex.xyz += ( simplePerlin2D16 * _WaveUp );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult3 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile4 = appendResult3;
			float2 panner14 = ( ( _Time.y * _WaveSpeed ) * _WaveDirection + ( ( WorldSpaceTile4 * float4( _WaveForm, 0.0 , 0.0 ) ) * _WaveTile ).xy);
			float simplePerlin2D16 = snoise( panner14 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float2 temp_cast_2 = (simplePerlin2D16).xx;
			o.Normal = tex2D( _TextureSample0, temp_cast_2 ).rgb;
			float4 color45 = IsGammaSpace() ? float4(0.5529412,0.8470588,0.9529412,0) : float4(0.2663557,0.6866854,0.8962694,0);
			float4 lerpResult17 = lerp( _WaterColorBottom , _WaterColorTop , simplePerlin2D16);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth42 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth42 = abs( ( screenDepth42 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.6 ) );
			float4 lerpResult59 = lerp( color45 , lerpResult17 , saturate( distanceDepth42 ));
			o.Albedo = lerpResult59.rgb;
			o.Smoothness = 0.9;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;632;938;718.2031;-165.3017;1;False;False
Node;AmplifyShaderEditor.CommentaryNode;1;-1249.564,-1004.704;Inherit;False;905.8179;266.8411;WorldSpace;3;4;3;2;WorldSpace;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1199.564,-954.7044;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;3;-870.1852,-916.8634;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-597.7452,-942.7711;Inherit;False;WorldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1952.596,-693.0665;Inherit;False;4;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;5;-1930.01,-567.334;Inherit;False;Property;_WaveForm;Wave Form;5;0;Create;True;0;0;False;0;0.23,0.09;0.09,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;7;-1830.413,154.0885;Inherit;False;Property;_WaveSpeed;Wave Speed;0;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1634.972,-558.7684;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1824.399,-22.3396;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1674.981,-362.0315;Inherit;False;Property;_WaveTile;WaveTile;1;0;Create;True;0;0;False;0;1;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1448.489,-370.741;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;11;-1657.995,-208.7924;Inherit;False;Property;_WaveDirection;Wave Direction;6;0;Create;True;0;0;False;0;-1,0.1;-1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1613.888,-24.34439;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-989.5674,878.4109;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0.6;0.1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;14;-1296.912,-225.5636;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;21;-1079.976,-671.9719;Inherit;False;Property;_WaterColorBottom;Water Color Bottom;4;0;Create;True;0;0;False;0;0.04454434,0.1623221,0.7264151,0;0.1268241,0.2680301,0.3584902,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-1031.146,-218.8491;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;42;-621.4373,758.5494;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-1118.159,-465.1668;Inherit;False;Property;_WaterColorTop;Water Color Top;3;0;Create;True;0;0;False;0;0,0,0,0;0.2129316,0.366761,0.5188679,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;15;-1096.303,217.3074;Inherit;True;Property;_WaveUp;Wave Up;7;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;45;-347.0622,453.3582;Inherit;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;0.5529412,0.8470588,0.9529412,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;17;-685.5842,-463.8501;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;60;-343.2122,769.8727;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;34;315.4536,1966.361;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-667.1828,2070.435;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-986.4437,2118.387;Inherit;False;Constant;_Float1;Float 1;9;0;Create;True;0;0;False;0;0.3;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;-255.7386,1946.335;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-459.6624,1944.58;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;24;-318.5167,-65.0389;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;False;0;-1;None;387dd08a0b0843a4f80cbb9c6130a6ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;309.9829,450.165;Inherit;False;Property;_Opp;Opp;9;0;Create;True;0;0;False;0;0;0.423;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-688.9672,196.4163;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;23;474.7673,310.4316;Inherit;False;Property;_Tessellation;Tessellation;2;0;Create;True;0;0;False;0;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;29.61819,494.3752;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;30;-724.7239,1953.303;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GrabScreenPosition;29;-1040.255,1924.64;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;214.2491,75.64095;Inherit;False;Constant;_Smooth;Smooth;7;0;Create;True;0;0;False;0;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;28;-99.19051,1939.868;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;49;110.0128,1956.659;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;682.3647,-32.00052;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Water Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;1
WireConnection;3;1;2;3
WireConnection;4;0;3;0
WireConnection;9;0;6;0
WireConnection;9;1;5;0
WireConnection;12;0;9;0
WireConnection;12;1;10;0
WireConnection;13;0;8;0
WireConnection;13;1;7;0
WireConnection;14;0;12;0
WireConnection;14;2;11;0
WireConnection;14;1;13;0
WireConnection;16;0;14;0
WireConnection;42;0;43;0
WireConnection;17;0;21;0
WireConnection;17;1;41;0
WireConnection;17;2;16;0
WireConnection;60;0;42;0
WireConnection;34;0;49;0
WireConnection;32;0;31;0
WireConnection;36;0;33;0
WireConnection;33;0;30;0
WireConnection;33;1;32;0
WireConnection;24;1;16;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;59;0;45;0
WireConnection;59;1;17;0
WireConnection;59;2;60;0
WireConnection;30;0;29;0
WireConnection;49;0;28;0
WireConnection;0;0;59;0
WireConnection;0;1;24;0
WireConnection;0;4;18;0
WireConnection;0;11;19;0
WireConnection;0;14;23;0
ASEEND*/
//CHKSM=901363F27A81DA17D29640AEB27DAC5E82AB1E69