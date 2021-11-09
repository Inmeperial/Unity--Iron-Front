// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GradientRed"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Red("Red", Range( 0 , 1)) = 0
		_Green("Green", Range( 0 , 1)) = 0
		_Blue("Blue", Range( 0 , 1)) = 0

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float _Green;
			uniform float _Blue;
			uniform float _Red;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float4 appendResult50 = (float4(0.0 , _Green , _Blue , 1.0));
				float2 uv026 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float ifLocalVar71 = 0;
				if( _Blue >= 0.0 )
				ifLocalVar71 = _Red;
				float ifLocalVar69 = 0;
				if( _Blue > 0.0 )
				ifLocalVar69 = _Red;
				else if( _Blue == 0.0 )
				ifLocalVar69 = 1.0;
				float ifLocalVar68 = 0;
				if( _Green > 0.0 )
				ifLocalVar68 = ifLocalVar71;
				else if( _Green == 0.0 )
				ifLocalVar68 = ifLocalVar69;
				float ifLocalVar67 = 0;
				if( _Red > 0.0 )
				ifLocalVar67 = ifLocalVar68;
				else if( _Red == 0.0 )
				ifLocalVar67 = ( 1.0 - _Red );
				float4 appendResult56 = (float4(ifLocalVar67 , _Green , _Blue , 1.0));
				float2 uv058 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				
				half4 color = ( ( ( float4(0,0,0,0) + appendResult50 ) * ( 1.0 - uv026.x ) ) + ( appendResult56 * uv058.x ) );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
292;73;1142;612;1845.189;-521.3604;1.506923;False;False
Node;AmplifyShaderEditor.CommentaryNode;72;-1132.57,819.4688;Inherit;False;1256.21;1066.87;Red1;9;70;71;69;68;67;58;56;59;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1635.095,932.2057;Inherit;False;Property;_Blue;Blue;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-1644.831,653.879;Inherit;False;Property;_Red;Red;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1082.57,1373.914;Inherit;False;Constant;_PureRed;PureRed;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;69;-808.5666,1235.476;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;71;-815.5709,1014.913;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1635.923,772.3649;Inherit;False;Property;_Green;Green;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;61;-978.7703,869.4688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;54;-1193.4,-8.141068;Inherit;False;1337.324;755.151;Red0;6;52;51;50;27;53;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1532.788,1049.452;Inherit;False;Constant;_Alpha;Alpha;3;0;Create;True;0;0;False;0;1;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;68;-588.5666,973.476;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1106.308,41.85893;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;50;-713.1394,468.0779;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;51;-705.0077,246.8901;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;67;-424.5665,923.476;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;53;-506.415,48.4091;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-797.3154,1587.339;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;56;-213.0341,1101.427;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-444.787,399.7702;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-91.07556,104.8295;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-111.3609,1351.469;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;370.6243,533.8362;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;18;741.3054,532.0963;Float;False;True;-1;2;ASEMaterialInspector;0;4;GradientRed;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;69;0;3;0
WireConnection;69;2;1;0
WireConnection;69;3;70;0
WireConnection;71;0;3;0
WireConnection;71;2;1;0
WireConnection;71;3;1;0
WireConnection;61;0;1;0
WireConnection;68;0;2;0
WireConnection;68;2;71;0
WireConnection;68;3;69;0
WireConnection;50;1;2;0
WireConnection;50;2;3;0
WireConnection;50;3;5;0
WireConnection;67;0;1;0
WireConnection;67;2;68;0
WireConnection;67;3;61;0
WireConnection;53;0;26;1
WireConnection;56;0;67;0
WireConnection;56;1;2;0
WireConnection;56;2;3;0
WireConnection;56;3;5;0
WireConnection;52;0;51;0
WireConnection;52;1;50;0
WireConnection;27;0;52;0
WireConnection;27;1;53;0
WireConnection;59;0;56;0
WireConnection;59;1;58;1
WireConnection;60;0;27;0
WireConnection;60;1;59;0
WireConnection;18;0;60;0
ASEEND*/
//CHKSM=76074F6611885259988C12590238097C45B34053