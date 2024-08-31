class_name MissionRes
extends Resource


var _missionDescription: String;
	
var tasks = [];

var _isComplete: bool = false;
var _isMissionHidden: bool = false;

var _missionManager: MissionManager;

signal OnMissionComplete();
signal OnVisibilityChanged(isVisible: bool);

func AddTask(taskRes: TaskRes):
	tasks.push_back(taskRes);
	taskRes.OnTaskComplete.connect(CheckTasks); 

func CheckTasks():
	var allTasksComplete = true;
	for task in tasks:
		if(!task.IsComplete()):
			allTasksComplete = false;
			break;
	
	if(allTasksComplete):
		print("Mission " + _missionDescription + " is completed!");
		OnMissionComplete.emit();
		_isComplete = true;

func IsComplete() -> bool:
	return _isComplete;

func GetMissionDescription() -> String:
	return _missionDescription;

func GetTasks():
	return tasks;

func SetMissionManager(missionManager: MissionManager):
	_missionManager = missionManager;
	for task:TaskRes in tasks:
		task.OnTaskComplete.connect(_missionManager.OnTaskComplete);

func HideMission():
	_isMissionHidden = true;
	OnVisibilityChanged.emit(false);

func ShowMission():
	_isMissionHidden = false;
	OnVisibilityChanged.emit(true);

func IsMissionHidden() -> bool:
	return _isMissionHidden;
