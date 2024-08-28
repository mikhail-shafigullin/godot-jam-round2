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
	
	/*
	 * * branch main -> FETCH_HEAD = [up to date] main -> origin/main hint:
	 * You have divergent branches and need to specify how to reconcile them.
	 * hint: You can do so by running one of the following commands sometime before hint: your next pull: hint: hint: git config pull.rebase false # merge hint: git config pull.rebase true # rebase hint: git config pull.ff only # fast-forward only hint: hint: You can replace "git config" with "git config --global" to set a default hint: preference for all repositories. You can also pass --rebase, --no-rebase, hint: or --ff-only on the command line to override the configured default per hint: invocation. Need to specify how to reconcile divergent branches.
	 */
}
