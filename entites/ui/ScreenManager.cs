using Godot;
using System;

public partial class ScreenManager : Control
{
	private AnimationPlayer AnimationPlayer;
	private Globals _globals = null;
	private ColorRect _colorRect;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetScreenManager(this);
		
		_colorRect = GetNode<ColorRect>("%ColorRect");
		_colorRect.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void PlayFadeOut()
	{
		AnimationPlayer.Play("fade_out");
	}
}
