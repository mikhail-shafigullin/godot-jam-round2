using Godot;
using System;
using GodotJamRound2.entites.mecha;

public partial class RepairTrigger : Node3D, ITriggerable
{
	private Globals _globals = null;
	private bool _isRepairing = false;
	
	[Export]
	private float _repairProgress = 0.0f;
	[Export]
	private float _repairSpeed = 20.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_isRepairing)
		{
			_repairProgress += _repairSpeed * (float)delta;
			_globals.GetPlayerUI().SetActionProgress(_repairProgress);
		}
	}

	public void Trigger()
	{
		BasePlayerUI ui = _globals.GetPlayerUI();
		ui.ShowActionProgress(true);
		
		GD.Print("Repairing...");
		_isRepairing = true;
	}
	
	public void RemoveTrigger()
	{
		BasePlayerUI ui = _globals.GetPlayerUI();
		ui.ShowActionProgress(false);
		_isRepairing = false;
	}
}
