using Godot;
using System;

public partial class BasePlayerUI : Control
{
	private Globals _globals = null;
	private RichTextLabel _eventText = null;
	private TextureProgressBar _actionProgress = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetPlayerUI(this);	
		
		_eventText = GetNode<RichTextLabel>("%EventText");
		_actionProgress = GetNode<TextureProgressBar>("%ActionProgress");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void ShowEventText(bool show)
	{
		_eventText.Visible = show;
	}
	
	public void ShowActionProgress(bool show)
	{
		_actionProgress.Visible = show;
	}

	public void SetEventText(String str)
	{
		_eventText.Text = "[center]" + str;
	}
	
	public void SetActionProgress(float value)
	{
		_actionProgress.Value = value;
	}
}
