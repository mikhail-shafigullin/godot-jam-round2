using Godot;
using System;
using GodotJamRound2.gameplay;

public partial class CurrentMission : Control
{
	private MissionManager _missionManager;
	
	private RichTextLabel _missionLabel;
	private Control _taskContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Globals _globals = GetNode<Globals>("/root/Globals");
		_missionManager = _globals.GetMissionManager();
		
		_missionManager.OnCurrentMissionChanged += OnCurrentMissionChanged;
		_missionManager.OnChangeVisibility += OnVisibilityChanged;
		
		_missionLabel = GetNode<RichTextLabel>("%HeaderRichText");
		_taskContainer = GetNode<Control>("%TaskContainer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnCurrentMissionChanged()
	{
		GD.Print("Current Mission Changed");
		Visible = true;
		MissionRes currentMissionRes = _missionManager.GetCurrentMission();
		_missionLabel.Text = currentMissionRes.GetMissionDescription();
		foreach (Node child in _taskContainer.GetChildren())
		{
			_taskContainer.RemoveChild(child);
			child.QueueFree();
		}

		if (currentMissionRes.IsComplete())
		{
			_missionLabel.Text = "[s][color=green]" + _missionLabel.Text + "[/color]";
		}
			
		foreach (TaskRes task in currentMissionRes.GetTasks())
		{
			RichTextLabel taskLabel = new RichTextLabel();
			taskLabel.FitContent = true;
			taskLabel.BbcodeEnabled = true;
			if (task.IsComplete())
			{
				taskLabel.Text = "[s][color=green]" + task.GetName();
			}
			else
			{
				taskLabel.Text = task.GetName();
			}
			_taskContainer.AddChild(taskLabel);
			
		}
	}
	
	public void OnVisibilityChanged(bool isHidden)
	{
		Visible = !isHidden;
	}
}
