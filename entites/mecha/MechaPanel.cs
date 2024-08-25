using Godot;
using System;
using GodotJamRound2.mechas;

public partial class MechaPanel : Node3D
{
	private HangarRes _hangarRes;

	private Node3D MechaMesh;
	private Node3D RepairTriggers;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MechaMesh = GetNode<Node3D>("%Mecha");
		RepairTriggers = GetNode<Node3D>("%RepairTriggers");
		HideMecha();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetHangarRes(HangarRes hangarRes)
	{
		this._hangarRes = hangarRes;
	}

	public void ShowMecha()
	{
		MechaMesh.Visible = true;
		RepairTriggers.Visible = true;
		EnableRepairTriggers();
	}
	
	public void HideMecha()
	{
		MechaMesh.Visible = false;
		RepairTriggers.Visible = false;
		DisableRepairTriggers();
	}
	
	public void DisableRepairTriggers()
	{
		foreach (Node3D repairTrigger in RepairTriggers.GetChildren())
		{
			RepairTrigger trigger = (RepairTrigger)repairTrigger;
			trigger.SetDisabled(true);
		}
	}
	
	public void EnableRepairTriggers()
	{
		foreach (Node3D repairTrigger in RepairTriggers.GetChildren())
		{
			RepairTrigger trigger = (RepairTrigger)repairTrigger;
			trigger.SetDisabled(false);
		}
	}
}
