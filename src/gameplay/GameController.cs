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
	
	[ExportGroup("thirdMission")]
	[Export] private Array<RepairTrigger> RepairTriggersThirdMission;
	
	[ExportGroup("fourthMission")]
	[Export] private Array<RepairTrigger> RepairTriggersFourthMission;
	
	[ExportGroup("fifthMission")]
	[Export] private Array<RepairTrigger> RepairTriggersFifthMission;
	
	[ExportGroup("sixthMission")]
	[Export] private Array<RepairTrigger> RepairTriggersSixthMission;
	
	[ExportGroup("seventhMission")]
	[Export] private Array<RepairTrigger> RepairTriggersSeventhMission;
	
	[ExportGroup("eighthMission")]
	[Export] private Array<RepairTrigger> RepairTriggersEighthMission;

	private Marker3D zBackMarker;
	private Marker3D zForwardMarker;
	private Marker3D yTopMarker;
	private Marker3D yBottomMarker;
	private Marker3D xLeftMarker;
	private Marker3D xRightMarker;
	
	private bool isControlsDisabled = false;
	
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
		
		_globals.SetPlayer(DronPlayer);
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

	public void DisableControls(bool disable)
	{
		isControlsDisabled = disable;
		DronPlayer.SetControlsDisabled(disable);
	}
	
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
		foreach (RepairTrigger repairTrigger in RepairTriggersFirstMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("firstMission");
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
		MissionRes secondMission = new MissionRes("CHECK COOLANT LINE");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersSecondMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("secondMission");
			repairTrigger.SetBrokenPart(brokenPartRes);
			repairTrigger.SetDisabled(false);
			_globals.GetMapUiController().AddBrokenPart(repairTrigger, GetNodePositionForMap(repairTrigger));
			_missionManager.OnChangeVisibility += repairTrigger.SetDisabled;
			TaskRes task = new TaskRes("Fix Coolant Line #" + brokenPartsCount, new Signal(brokenPartRes, "OnPartRepaired"));
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
		DialogueManager.DialogueEnded += StartThirdMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersSecondMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartThirdMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersThirdMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("thirdMission");
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
			_timer.Timeout += StartFourthDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartThirdDialogue;
		DialogueManager.DialogueEnded -= StartThirdMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartFourthDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "fourthDialogue");
		DialogueManager.DialogueEnded += StartFourthMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersThirdMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartFourthMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersFourthMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("fourthMission");
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
			_timer.Timeout += StartFifthDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartFourthDialogue;
		DialogueManager.DialogueEnded -= StartFourthMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartFifthDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "fifthDialogue");
		DialogueManager.DialogueEnded += StartFifthMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersFourthMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartFifthMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersFifthMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("fifthMission");
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
			_timer.Timeout += StartSixthDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartFifthDialogue;
		DialogueManager.DialogueEnded -= StartFifthMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartSixthDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "sixthDialogue");
		DialogueManager.DialogueEnded += StartSixthMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersFifthMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartSixthMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersSixthMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("sixthMission");
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
			_timer.Timeout += StartSeventhDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartSixthDialogue;
		DialogueManager.DialogueEnded -= StartSixthMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartSeventhDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "seventhDialogue");
		DialogueManager.DialogueEnded += StartSeventhMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersSixthMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartSeventhMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersSeventhMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("seventhMission");
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
			_timer.Timeout += StartEighthDialogue;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartSeventhDialogue;
		DialogueManager.DialogueEnded -= StartSeventhMission;
		_missionManager.SetHidden(false);
	}
	
	public void StartEighthDialogue()
	{
		var dialogue = GD.Load<Resource>("res://assets/dialogues/firstDialogue.dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "eighthDialogue");
		DialogueManager.DialogueEnded += StartEighthMission;
		foreach (RepairTrigger repairTrigger in RepairTriggersSeventhMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		_missionManager.SetHidden(true);
	}
	
	public void StartEighthMission(Resource dialogueResource)
	{
		MissionRes secondMission = new MissionRes("Repair all broken parts");
		int brokenPartsCount = 0;
		foreach (RepairTrigger repairTrigger in RepairTriggersEighthMission)
		{
			BrokenPartRes brokenPartRes = _shipRes.CreateBrokenPartResForMission("eighthMission");
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
			_timer.Timeout += EndGame;
		};
		
		_missionManager.StartMission(secondMission);
		EmitSignal("OnGameStart");
		_timer.Timeout -= StartEighthDialogue;
		DialogueManager.DialogueEnded -= StartEighthMission;
		_missionManager.SetHidden(false);
	}
	
	public void EndGame()
	{
		foreach (RepairTrigger repairTrigger in RepairTriggersEighthMission)
		{
			_missionManager.OnChangeVisibility -= repairTrigger.SetDisabled;
		}
		EmitSignal("OnGameEnd");
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
