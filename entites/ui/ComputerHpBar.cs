using Godot;
using System;
using GodotJamRound2.entites.ui;

[Tool]
public partial class ComputerHpBar : Control
{
	[Export]
	private MechaPart _mechaPart = MechaPart.LEFT_ARM;
	
	private float _repairProgress = 0.0f;
	private RichTextLabel _label = null;
	private ProgressBar _progressBar = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<RichTextLabel>("%HPBarLabel");
		_progressBar = GetNode<ProgressBar>("%HPBarProgress");
		ChangeLabelTextFromPart();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void ChangeLabelTextFromPart()
	{
		switch (_mechaPart)
		{
			case MechaPart.LEFT_ARM:
				_label.Text = "Left Arm";
				break;
			case MechaPart.RIGHT_ARM:
				_label.Text = "Right Arm";
				break;
			case MechaPart.LEFT_LEG:
				_label.Text = "Left Leg";
				break;
			case MechaPart.RIGHT_LEG:
				_label.Text = "Right Leg";
				break;
			case MechaPart.TORSO:
				_label.Text = "Torso";
				break;
			case MechaPart.HEAD:
				_label.Text = "Head";
				break;
		}
	}
	
}
