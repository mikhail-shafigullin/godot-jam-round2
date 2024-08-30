using Godot;
using System;
using DialogueManagerRuntime;
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

	[Export] private DronPlayer DronPlayer;
	
	[ExportGroup("firstMission")]
	[Export] private Array<RepairTrigger> RepairTriggersFirstMission;
	
	[ExportGroup("secondMission")]
	[Export] private Array<RepairTrigger> RepairTriggersSecondMission;

	private Marker3D zBackMarker;
	private Marker3D zForwardMarker;
	private Marker3D yTopMarker;
	private Marker3D yBottomMarker;
	private Marker3D xLeftMarker;
	private Marker3D xRightMarker;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetGameController(this);
		
		_timer = new Timer();
		_timer.OneShot = true;
		AddChild(_timer);

		_missionManager = _globals.GetMissionManager();
		
		_timer.WaitTime = 1;
		_timer.Start();
		_timer.Timeout += StartFirstDialogue;

		_shipRes = new ShipRes();
		
		zBackMarker = GetParent().GetNode<Marker3D>("%zBack");
		zForwardMarker = GetParent().GetNode<Marker3D>("%zForward");
		yTopMarker = GetParent().GetNode<Marker3D>("%yTop");
		yBottomMarker = GetParent().GetNode<Marker3D>("%yBottom");
		xLeftMarker = GetParent().GetNode<Marker3D>("%xLeft");
		xRightMarker = GetParent().GetNode<Marker3D>("%xRight");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetPlayerPositionForMap();
	}
	
	[Signal]
	public delegate void OnGameStartEventHandler();
	
	[Signal]
	public delegate void OnGameEndEventHandler();

	public void StartFirstDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "firstDialogue");
		DialogueManager.DialogueEnded += StartFirstMission;
		_missionManager.SetHidden(true);
	}
	
	public void StartFirstMission(Resource dialogueResource)
	{
		
		MissionRes firstMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (BrokenPartRes brokenPartRes in _shipRes.GetBrokenParts("firstMission"))
		{
			RepairTrigger repairTrigger = RepairTriggersFirstMission[brokenPartsCount];
			repairTrigger.SetBrokenPart(brokenPartRes);
			repairTrigger.SetDisabled(false);
			_globals.GetMapUiController().AddBrokenPart(repairTrigger, GetNodePositionForMap(repairTrigger));
			_missionManager.OnChangeVisibility += repairTrigger.SetDisabled;
			TaskRes task = new TaskRes("Repair part of external shell", new Signal(brokenPartRes, "OnPartRepaired"));
			firstMission.AddTask(task);
			
			brokenPartsCount++;
		}
		firstMission.OnMissionComplete += () =>
		{
			_timer.WaitTime = 1;
			_timer.Start();
			_timer.Timeout += StartSecondDialogue;
		};
		
		_missionManager.StartMission(firstMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartFirstDialogue;
		DialogueManager.DialogueEnded -= StartFirstMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartSecondDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "secondDialogue");
		DialogueManager.DialogueEnded += StartSecondMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersFirstMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartSecondMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (BrokenPartRes brokenPartRes in _shipRes.GetBrokenParts("secondMission"))
		{
			RepairTrigger repairTrigger = RepairTriggersSecondMission[brokenPartsCount];
			repairTrigger.SetBrokenPart(brokenPartRes);
			repairTrigger.SetDisabled(false);
			_globals.GetMapUiController().AddBrokenPart(repairTrigger, GetNodePositionForMap(repairTrigger));
			_missionManager.OnChangeVisibility += repairTrigger.SetDisabled;
			TaskRes task = new TaskRes("Repair part of external shell", new Signal(brokenPartRes, "OnPartRepaired"));
			secondMission.AddTask(task);
			
			brokenPartsCount++;
		}
		secondMission.OnMissionComplete += () =>
		{
			_timer.WaitTime = 1;
			_timer.Start();
			_timer.Timeout += StartThirdDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartSecondDialogue;
		DialogueManager.DialogueEnded -= StartSecondMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartThirdDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "thirdDialogue");
		foreach (RepairTrigger repairTrigger in RepairTriggersSecondMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public MissionManager GetMissionManager()
	{
		return _missionManager;
	}

	public Vector3 GetNodePositionForMap(Node3D node)
	{
		Vector3 mapPosition = Vector3.Zero;
		mapPosition.X = (node.GlobalPosition.X - xLeftMarker.GlobalPosition.X) / (xRightMarker.GlobalPosition.X - xLeftMarker.GlobalPosition.X);
		mapPosition.Y = (node.GlobalPosition.Y - yBottomMarker.GlobalPosition.Y) / (yTopMarker.GlobalPosition.Y - yBottomMarker.GlobalPosition.Y);
		mapPosition.Z = (node.GlobalPosition.Z - zBackMarker.GlobalPosition.Z) / (zForwardMarker.GlobalPosition.Z - zBackMarker.GlobalPosition.Z);

		return mapPosition;
	}
	
	public Vector3 GetPlayerPositionForMap()
	{
		Vector3 mapPosition = GetNodePositionForMap(DronPlayer);
		return mapPosition;
	}
	
	
}


/*
 Show dialogue
 Start first mission (Show repair triggers) (Show tasks in panel)
 Repair one broken part
 Trigger disappear + task crossed in panel
 Repair another broken part
 Trigger disappear + task crossed in panel
 Mission complete 
 1 second delay
 Show dialogue
  
*/
