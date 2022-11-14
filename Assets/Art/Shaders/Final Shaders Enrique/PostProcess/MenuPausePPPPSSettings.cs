// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( MenuPausePPPPSRenderer ), PostProcessEvent.AfterStack, "MenuPausePP", true )]
public sealed class MenuPausePPPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "lerpPower" )]
	public FloatParameter _lerpPower = new FloatParameter { value = 0f };
}

public sealed class MenuPausePPPPSRenderer : PostProcessEffectRenderer<MenuPausePPPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "MenuPausePP" ) );
		sheet.properties.SetFloat( "_lerpPower", settings._lerpPower );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
