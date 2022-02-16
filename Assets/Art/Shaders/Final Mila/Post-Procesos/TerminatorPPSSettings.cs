// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( TerminatorPPSRenderer ), PostProcessEvent.AfterStack, "Terminator", true )]
public sealed class TerminatorPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "MaxOld" )]
	public FloatParameter _MaxOld = new FloatParameter { value = 2.787647f };
	[Tooltip( "Float 0" )]
	public FloatParameter _Float0 = new FloatParameter { value = 0f };
	[Tooltip( "Float 1" )]
	public FloatParameter _Float1 = new FloatParameter { value = 0f };
	[Tooltip( "Float 2" )]
	public FloatParameter _Float2 = new FloatParameter { value = 0f };
}

public sealed class TerminatorPPSRenderer : PostProcessEffectRenderer<TerminatorPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Terminator" ) );
		sheet.properties.SetFloat( "_MaxOld", settings._MaxOld );
		sheet.properties.SetFloat( "_Float0", settings._Float0 );
		sheet.properties.SetFloat( "_Float1", settings._Float1 );
		sheet.properties.SetFloat( "_Float2", settings._Float2 );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
