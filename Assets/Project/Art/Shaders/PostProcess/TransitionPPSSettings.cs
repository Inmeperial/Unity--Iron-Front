// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( TransitionPPSRenderer ), PostProcessEvent.AfterStack, "Transition", true )]
public sealed class TransitionPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "Vertical" )]
	public FloatParameter _Vertical = new FloatParameter { value = 0f };
	[Tooltip( "Speed" )]
	public FloatParameter _Speed = new FloatParameter { value = 2f };
	[Tooltip( "Duration" )]
	public FloatParameter _Duration = new FloatParameter { value = 2f };
	[Tooltip( "Automatic" )]
	public FloatParameter _Automatic = new FloatParameter { value = 0f };
	[Tooltip( "Mask" )]
	public FloatParameter _Mask = new FloatParameter { value = 0f };
}

public sealed class TransitionPPSRenderer : PostProcessEffectRenderer<TransitionPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Transition" ) );
		sheet.properties.SetFloat( "_Vertical", settings._Vertical );
		sheet.properties.SetFloat( "_Speed", settings._Speed );
		sheet.properties.SetFloat( "_Duration", settings._Duration );
		sheet.properties.SetFloat( "_Automatic", settings._Automatic );
		sheet.properties.SetFloat( "_Mask", settings._Mask );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
