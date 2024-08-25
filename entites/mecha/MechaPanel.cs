using Godot;
using System;
using Godot.Collections;
using GodotJamRound2.entites.ui;
using GodotJamRound2.mechas;

public partial class MechaPanel : Node3D
{
	private HangarPanelRes _hangarPanelRes;

	private Node3D MechaMesh;
	private Node3D RepairTriggers;
	
	private Dictionary<EMechaPartType, RepairTrigger> RepairTriggersDictionary { get; set; } = new Dictionary<EMechaPartType, RepairTrigger>();

	private MechaRes _mechaRes;
	private bool _IsMechaAvailable = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MechaMesh = GetNode<Node3D>("%Mecha");
		RepairTriggers = GetNode<Node3D>("%RepairTriggers");

		foreach (Node3D repairTrigger in RepairTriggers.GetChildren())
		{
			RepairTrigger trigger = (RepairTrigger)repairTrigger;
			RepairTriggersDictionary[trigger.GetMechaPartType()] = trigger;
		}
		
		HideMecha();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetHangarRes(HangarPanelRes hangarPanelRes)
	{
		this._hangarPanelRes = hangarPanelRes;
		this._hangarPanelRes.OnCreateMecha += ShowMecha;
		this._hangarPanelRes.OnLaunchMecha += HideMecha;
	}

	public void ShowMecha()
	{
		_IsMechaAvailable = true;
		MechaMesh.Visible = true;
		RepairTriggers.Visible = true;
		EnableRepairTriggers();
	}
	
	public void HideMecha()
	{
		_IsMechaAvailable = false;
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
	
	public void SetMechaRes(MechaRes mechaRes)
	{
		_mechaRes = mechaRes;
	}
}
