using Godot;
using System;
using Godot.Collections;
using GodotJamRound2.gameplay;
using GodotJamRound2.mechas;
using GodotJamRound2.ship;
using Array = System.Array;

public partial class GameController : Node
{
	private Globals _globals = null;
	private Timer _timer;
	private MissionManager _missionManager;
	private ShipRes _shipRes;

	[ExportGroup("firstMission")]
	[Export] private Array<RepairTrigger> RepairTriggers;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetGameController(this);
		
		_timer = new Timer();
		_timer.OneShot = true;
		AddChild(_timer);

		_missionManager = _globals.GetMissionManager();
		
		_timer.WaitTime = 3;
		_timer.Start();
		_timer.Timeout += StartFirstMission;

		_shipRes = new ShipRes();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	[Signal]
	public delegate void OnGameStartEventHandler();
	
	[Signal]
	public delegate void OnGameEndEventHandler();
	
	public void StartFirstMission()
	{
		MissionRes firstMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (BrokenPartRes brokenPartRes in _shipRes.GetBrokenParts("firstMission"))
		{
			RepairTrigger repairTrigger = RepairTriggers[brokenPartsCount];
			repairTrigger.SetBrokenPart(brokenPartRes);
			TaskRes task = new TaskRes("Repair part of external shell", new Signal(brokenPartRes, "OnPartRepaired"));
			firstMission.AddTask(task);
			
			brokenPartsCount++;
		}
		firstMission.OnMissionComplete += () =>
		{
			_timer.WaitTime = 3;
			_timer.Start();
			_timer.Timeout += StartFirstMission;
			EmitSignal("OnGameEnd");
		};
		
		_missionManager.StartMission(firstMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartFirstMission;
	}
	
	public MissionManager GetMissionManager()
	{
		return _missionManager;
	}
	
}
