// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( NigthVisionPPSRenderer ), PostProcessEvent.AfterStack, "NigthVision", true )]
public sealed class NigthVisionPPSSettings : PostProcessEffectSettings
{
}

public sealed class NigthVisionPPSRenderer : PostProcessEffectRenderer<NigthVisionPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Nigth Vision" ) );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
