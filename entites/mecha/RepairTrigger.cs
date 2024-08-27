using Godot;
using System;
using GodotJamRound2.entites.mecha;
using GodotJamRound2.entites.ui;
using GodotJamRound2.ship;

public partial class RepairTrigger : Node3D, ITriggerable
{
	private Globals _globals = null;
	private bool _isRepairing = false;
	private bool _isDisabled = false;
	
	[Export]
	private float _repairProgress = 0.0f;
	[Export]
	private float _repairSpeed = 20.0f;
	
	private BrokenPartRes _brokenPart = null;
	
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
		if (!_isDisabled)
		{
			BasePlayerUI ui = _globals.GetPlayerUI();
			ui.ShowActionProgress(true);

			GD.Print("Repairing...");
			_isRepairing = true;
		}
	}
	
	public void RemoveTrigger()
	{
		if (!_isDisabled)
		{
			BasePlayerUI ui = _globals.GetPlayerUI();
			ui.ShowActionProgress(false);
			_isRepairing = false;
		}
	}
	
	public void SetDisabled(bool disabled)
	{
		if(disabled)
		{
			RemoveTrigger();
		}
		_isDisabled = disabled;
		
	}
	
	public void SetBrokenPart(BrokenPartRes brokenPart)
	{
		_brokenPart = brokenPart;
	}

	public void _on_area_3d_body_entered(Node3D node)
	{
		if(node is DronPlayer)
		{
			Trigger();
		}
	}
	
	public void _on_area_3d_body_exited(Node3D node)
	{
		if(node is DronPlayer)
		{
			RemoveTrigger();
		}
	}
	
}
