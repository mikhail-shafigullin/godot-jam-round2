using Godot;
using System;
using GodotJamRound2.entites.mecha;
using GodotJamRound2.entites.ui;
using GodotJamRound2.gameplay;
using GodotJamRound2.ship;

public partial class RepairTrigger : Node3D, ITriggerable
{
	private Globals _globals = null;
	
	private bool _isRepairing = false;
	private bool _isRepaired = false;
	
	private bool _isDisabled = true;
	[Export]
	private float _repairProgress = 0.0f;
	[Export]
	private float _repairSpeed = 20.0f;
	
	private float maxRepairProgress = BrokenPartRes.maxRepairProgress;
	
	private BrokenPartRes _brokenPart = null;

	private Area3D _area3D;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_area3D = GetNode<Area3D>("%Area3D");

		SetDisabled(true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_isRepairing )
		{
			_repairProgress += _repairSpeed * (float)delta;
			_globals.GetPlayerUI().SetActionProgress(_repairProgress);
			if(_repairProgress >= maxRepairProgress)
			{
				_brokenPart.SetRepairProgress(_repairProgress);
				_isRepairing = false;
			}
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
			
			if(_brokenPart != null)
			{
				_brokenPart.SetRepairProgress(_repairProgress);
			}
		}
	}
	
	public void SetDisabled(bool disabled)
	{
		if(disabled)
		{
			RemoveTrigger();
		}
		_isDisabled = disabled;
		_area3D.Monitoring = !disabled;
		Visible = !disabled;
	}
	
	public void SetBrokenPart(BrokenPartRes brokenPart)
	{
		_brokenPart = brokenPart;
		brokenPart.OnPartRepaired += PartRepaired;
	}
	
	public void PartRepaired()
	{
		_isRepaired = true;
		SetDisabled(true);
		EmitSignal(nameof(OnPartRepaired));
	}

	public void _on_area_3d_body_entered(Node3D node)
	{
		if(node is DronPlayer)
		{
			DronPlayer player = (DronPlayer)node;
			Trigger();
			player.Repairing();
		}
	}
	
	public void _on_area_3d_body_exited(Node3D node)
	{
		if(node is DronPlayer)
		{
			DronPlayer player = (DronPlayer)node;
			RemoveTrigger();
			player.StopRepairing();
		}
	}
	
	[Signal]
	public delegate void OnPartRepairedEventHandler();
	
}
