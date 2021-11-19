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
		_Opp("Opp", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
		uniform float _Opp;
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
			float4 tex2DNode24 = tex2D( _TextureSample0, temp_cast_2 );
			o.Normal = tex2DNode24.rgb;
			float4 lerpResult17 = lerp( _WaterColorBottom , _WaterColorTop , simplePerlin2D16);
			o.Albedo = lerpResult17.rgb;
			float4 color45 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth42 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth42 = abs( ( screenDepth42 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.5 ) );
			float4 temp_output_46_0 = ( color45 * ( 1.0 - distanceDepth42 ) );
			o.Emission = temp_output_46_0.rgb;
			o.Smoothness = 0.9;
			float temp_output_50_0 = _Opp;
			o.Alpha = temp_output_50_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;73;778;938;604.3427;534.5939;1.966069;False;False
Node;AmplifyShaderEditor.CommentaryNode;1;-1249.564,-1004.704;Inherit;False;905.8179;266.8411;WorldSpace;3;4;3;2;WorldSpace;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1199.564,-954.7044;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;3;-870.1852,-916.8634;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-597.7452,-942.7711;Inherit;False;WorldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1952.596,-693.0665;Inherit;False;4;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;5;-1930.01,-567.334;Inherit;False;Property;_WaveForm;Wave Form;6;0;Create;True;0;0;False;0;0.23,0.09;0.09,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;10;-1674.981,-362.0315;Inherit;False;Property;_WaveTile;WaveTile;1;0;Create;True;0;0;False;0;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1824.399,-22.3396;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1634.972,-558.7684;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1830.413,154.0885;Inherit;False;Property;_WaveSpeed;Wave Speed;0;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1155.831,1233.404;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0.5;0.1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-1657.995,-208.7924;Inherit;False;Property;_WaveDirection;Wave Direction;7;0;Create;True;0;0;False;0;-1,0.1;-1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1448.489,-370.741;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1613.888,-24.34439;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;14;-1296.912,-225.5636;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;42;-840.514,1126.746;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-1118.159,-465.1668;Inherit;False;Property;_WaterColorTop;Water Color Top;4;0;Create;True;0;0;False;0;0,0,0,0;0.2330007,0.4832963,0.6415094,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;15;-1096.303,217.3074;Inherit;True;Property;_WaveUp;Wave Up;8;0;Create;True;0;0;False;0;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;21;-1079.976,-671.9719;Inherit;False;Property;_WaterColorBottom;Water Color Bottom;5;0;Create;True;0;0;False;0;0.04454434,0.1623221,0.7264151,0;0.1268242,0.2680302,0.3584903,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;44;-519.611,1122.24;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;45;-529.3875,888.2198;Inherit;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-1031.146,-218.8491;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1208.706,796.8567;Inherit;False;Constant;_Float1;Float 1;9;0;Create;True;0;0;False;0;0.3;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;31.38625,164.7811;Inherit;False;Property;_Opp;Opp;10;0;Create;True;0;0;False;0;0;0.564;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-568.6321,-46.40639;Inherit;False;Property;_NormalScale;NormalScale;3;0;Create;True;0;0;False;0;10;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;-478.0012,624.8046;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;540.7919,647.539;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;214.2491,75.64095;Inherit;False;Constant;_Smooth;Smooth;7;0;Create;True;0;0;False;0;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;317.7439,464.169;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;28;-321.4531,618.3372;Inherit;False;Global;_GrabScreen0;Grab Screen 0;8;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;212.8722,298.4364;Inherit;False;Property;_Tessellation;Tessellation;2;0;Create;True;0;0;False;0;8;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;29;-1262.518,603.1092;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-688.9672,196.4163;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;30;-946.9863,631.7728;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-112.2498,635.1285;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;-292.184,-7.106943;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;-1;None;387dd08a0b0843a4f80cbb9c6130a6ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-305.5898,1054.621;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;34;93.19102,644.8306;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;17;-685.5842,-463.8501;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-681.9249,623.0499;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;401.8333,994.7933;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;53;70.55868,1162.38;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-889.4452,748.9044;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;406.6515,-24.94796;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Water Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;42;0;43;0
WireConnection;44;0;42;0
WireConnection;16;0;14;0
WireConnection;36;0;33;0
WireConnection;54;0;50;0
WireConnection;28;0;36;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;30;0;29;0
WireConnection;49;0;28;0
WireConnection;24;1;16;0
WireConnection;46;0;45;0
WireConnection;46;1;44;0
WireConnection;34;0;49;0
WireConnection;17;0;21;0
WireConnection;17;1;41;0
WireConnection;17;2;16;0
WireConnection;33;0;30;0
WireConnection;33;1;32;0
WireConnection;52;0;46;0
WireConnection;53;0;46;0
WireConnection;32;0;31;0
WireConnection;32;1;24;0
WireConnection;0;0;17;0
WireConnection;0;1;24;0
WireConnection;0;2;46;0
WireConnection;0;4;18;0
WireConnection;0;9;50;0
WireConnection;0;11;19;0
WireConnection;0;14;23;0
ASEEND*/
//CHKSM=9033503783BA2DF2DD3A8594132A9DEC5FDCACA0