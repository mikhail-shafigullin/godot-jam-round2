extends Node

class_name GameController

var _timer = null
var _missionManager: MissionManager = null
var _shipRes = null

@export var _dronPlayer: DronPlayer = null

@export_group("firstMission")
@export var RepairTriggersFirstMission: Array[RepairTrigger] = []

@export_group("secondMission")
@export var RepairTriggersSecondMission: Array[RepairTrigger] = []

@export_group("thirdMission")
@export var RepairTriggersThirdMission: Array[RepairTrigger] = []

@export_group("fourthMission")
@export var RepairTriggersFourthMission: Array[RepairTrigger] = []

@export_group("fifthMission")
@export var RepairTriggersFifthMission: Array[RepairTrigger] = []

@export_group("sixthMission")
@export var RepairTriggersSixthMission: Array[RepairTrigger] = []

@export_group("seventhMission")
@export var RepairTriggersSeventhMission: Array[RepairTrigger] = []

@export_group("eighthMission")
@export var RepairTriggersEighthMission: Array[RepairTrigger] = []

var zBackMarker = null
var zForwardMarker = null
var yTopMarker = null
var yBottomMarker = null
var xLeftMarker = null
var xRightMarker = null

var isControlsDisabled = false

signal OnGameStart()

signal OnGameEnd()

func _ready():
	Global._gameController = self;

	_timer = Timer.new()
	_timer.one_shot = true
	add_child(_timer)

	_missionManager = Global._missionManager;

	_timer.wait_time = 1
	_timer.start()
	_timer.timeout.connect(StartFirstDialogue)

	_shipRes = ShipRes.new()

	zBackMarker = get_parent().get_node("%zBack")
	zForwardMarker = get_parent().get_node("%zForward")
	yTopMarker = get_parent().get_node("%yTop")
	yBottomMarker = get_parent().get_node("%yBottom")
	xLeftMarker = get_parent().get_node("%xLeft")
	xRightMarker = get_parent().get_node("%xRight")

	Global._player = _dronPlayer;

func _process(delta):
	pass;

func DisableControls(disable):
	isControlsDisabled = disable
	_dronPlayer.set_controls_disabled(disable)

func StartFirstDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "firstDialogue")
	DialogueManager.dialogue_ended.connect(StartFirstMission)
	_missionManager.set_hidden(true)

func StartFirstMission(dialogue_resource):
	var firstMission = MissionRes.new()
	firstMission._missionDescription = "CHECK CARGO AREA [C6-0]";
	var brokenPartsCount = 0
	for repairTrigger: RepairTrigger in RepairTriggersFirstMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("firstMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled) 
		var task = TaskRes.new();
		task.init("Repair damage", Signal(brokenPartRes, "OnPartRepaired"))
		firstMission.AddTask(task)
		brokenPartsCount += 1
	firstMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartSecondDialogue))
		
	_missionManager.start_mission(firstMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartFirstDialogue)
	DialogueManager.dialogue_ended.disconnect(StartFirstMission)
	_missionManager.set_hidden(false)

func StartSecondDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "secondDialogue")
	DialogueManager.dialogue_ended.connect(StartSecondMission)
	for repairTrigger in RepairTriggersFirstMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartSecondMission(dialogue_resource):
	var secondMission = MissionRes.new()
	secondMission._missionDescription = "CHECK COOLANT LINE";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersSecondMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("secondMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Check ES-01" + str(brokenPartsCount), Signal(brokenPartRes, "OnPartRepaired"))
		secondMission.AddTask(task)
		brokenPartsCount += 1
	secondMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartThirdDialogue)
	)
	_missionManager.start_mission(secondMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartSecondDialogue)
	DialogueManager.dialogue_ended.disconnect(StartSecondMission)
	_missionManager.set_hidden(false)

func StartThirdDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "thirdDialogue")
	DialogueManager.dialogue_ended.connect(StartThirdMission)
	for repairTrigger in RepairTriggersSecondMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartThirdMission(dialogue_resource):
	var thirdMission = MissionRes.new()
	thirdMission._missionDescription = "CHECK CARGO AREA [C5-12]";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersThirdMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("thirdMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new();
		task.init("Repair damage", Signal(brokenPartRes, "OnPartRepaired"))
		thirdMission.AddTask(task)
		brokenPartsCount += 1
	thirdMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartFourthDialogue)
	)
	_missionManager.start_mission(thirdMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartThirdDialogue)
	DialogueManager.dialogue_ended.disconnect(StartThirdMission)
	_missionManager.set_hidden(false)

func StartFourthDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "fourthDialogue")
	DialogueManager.dialogue_ended.connect(StartFourthMission)
	for repairTrigger in RepairTriggersThirdMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartFourthMission(dialogue_resource):
	var fourthMission = MissionRes.new()
	fourthMission._missionDescription = "OVERWRITE FLOW CONTROL";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersFourthMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("fourthMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Redirect the flow valve", Signal(brokenPartRes, "OnPartRepaired"))
		fourthMission.AddTask(task)
		brokenPartsCount += 1
	fourthMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartFifthDialogue)
	)
	_missionManager.start_mission(fourthMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartFourthDialogue)
	DialogueManager.dialogue_ended.disconnect(StartFourthMission)
	_missionManager.set_hidden(false)

func StartFifthDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "fifthDialogue")
	DialogueManager.dialogue_ended.connect(StartFifthMission)
	for repairTrigger in RepairTriggersFourthMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartFifthMission(dialogue_resource):
	var fifthMission = MissionRes.new()
	fifthMission._missionDescription = "ESTABLISH REMOTE CONTROL";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersFifthMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("fifthMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Path the WF-6", Signal(brokenPartRes, "OnPartRepaired"))
		fifthMission.AddTask(task)
		brokenPartsCount += 1
	fifthMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartSixthDialogue)
	)
	_missionManager.start_mission(fifthMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartFifthDialogue)
	DialogueManager.dialogue_ended.disconnect(StartFifthMission)
	_missionManager.set_hidden(false)

func StartSixthDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "sixthDialogue")
	DialogueManager.dialogue_ended.connect(StartSixthMission)
	for repairTrigger in RepairTriggersFifthMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartSixthMission(dialogue_resource):
	var sixthMission = MissionRes.new()
	sixthMission._missionDescription = "ATMOSPHERIC MAINTENANCE";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersSixthMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("sixthMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Desolder the pressure gauge", Signal(brokenPartRes, "OnPartRepaired"))
		sixthMission.AddTask(task)
		brokenPartsCount += 1
	sixthMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartSeventhDialogue)
	)
	_missionManager.start_mission(sixthMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartSixthDialogue)
	DialogueManager.dialogue_ended.disconnect(StartSixthMission)
	_missionManager.set_hidden(false)

func StartSeventhDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "seventhDialogue")
	DialogueManager.dialogue_ended.connect(StartSeventhMission)
	for repairTrigger in RepairTriggersSixthMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartSeventhMission(dialogue_resource):
	var seventhMission = MissionRes.new()
	seventhMission._missionDescription = "FIRE DRILL";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersSeventhMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("seventhMission")
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Trigger fire alarm", Signal(brokenPartRes, "OnPartRepaired"))
		seventhMission.AddTask(task)
		brokenPartsCount += 1
	seventhMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(StartEighthDialogue)
	)
	_missionManager.start_mission(seventhMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartSeventhDialogue)
	DialogueManager.dialogue_ended.disconnect(StartSeventhMission)
	_missionManager.set_hidden(false)

func StartEighthDialogue():
	var dialogue = load("res://assets/dialogues/firstDialogue.dialogue")
	DialogueManager.show_dialogue_balloon(dialogue, "eighthDialogue")
	DialogueManager.dialogue_ended.connect(StartEighthMission)
	for repairTrigger in RepairTriggersSeventhMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	_missionManager.set_hidden(true)

func StartEighthMission(dialogue_resource):
	var eighthMission = MissionRes.new()
	eighthMission._missionDescription = "OUT OF ORDERS";
	var brokenPartsCount = 0
	for repairTrigger in RepairTriggersEighthMission:
		var brokenPartRes = _shipRes.CreateBrokenPartResForMission("eighthMission") 
		repairTrigger.SetBrokenPart(brokenPartRes)
		repairTrigger.SetDisabled(false)
		Global._mapUiController.add_broken_part(repairTrigger, GetNodePositionForMap(repairTrigger))
		_missionManager.OnChangeVisibility.connect(repairTrigger.SetDisabled)
		var task = TaskRes.new()
		task.init("Return to charging station", Signal(brokenPartRes, "OnPartRepaired"))
		eighthMission.AddTask(task)
		brokenPartsCount += 1
	eighthMission.OnMissionComplete.connect(func():
		_timer.wait_time = 1
		_timer.start()
		_timer.timeout.connect(EndGame)
	)
	_missionManager.start_mission(eighthMission)
	emit_signal("OnGameStart")
	_timer.timeout.disconnect(StartEighthDialogue)
	DialogueManager.dialogue_ended.disconnect(StartEighthMission)
	_missionManager.set_hidden(false)

func EndGame():
	for repairTrigger in RepairTriggersEighthMission:
		_missionManager.OnChangeVisibility.disconnect(repairTrigger.SetDisabled)
	emit_signal("OnGameEnd")
	Global.cutscener.end()
	_dronPlayer._computerUi.visible = false
	_dronPlayer.set_mute(true)

func GetMissionManager():
	return _missionManager

func GetNodePositionForMap(node: Node3D):
	var mapPosition = Vector3.ZERO
	mapPosition.x = (node.global_position.x - xLeftMarker.global_position.x) / (xRightMarker.global_position.x - xLeftMarker.global_position.x)
	mapPosition.y = (node.global_position.y - yBottomMarker.global_position.y) / (yTopMarker.global_position.y - yBottomMarker.global_position.y)
	mapPosition.z = (node.global_position.z - zBackMarker.global_position.z) / (zForwardMarker.global_position.z - zBackMarker.global_position.z)
	return mapPosition

func GetPlayerPositionForMap():
	return GetNodePositionForMap(_dronPlayer)
