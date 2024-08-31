class_name MissionManager
extends Resource

var _currentMissionRes: MissionRes;
var previousMissions: Array = [];
var isHidden: bool = false;

signal OnCurrentMissionChanged();
signal OnChangeVisibility(hidden: bool);

func GetCurrentMission() -> MissionRes:
	return _currentMissionRes;


func IsMissionComplete() -> bool:
	return _currentMissionRes.IsComplete();
	

func OnTaskComplete():
	OnCurrentMissionChanged.emit()
	if(IsMissionComplete()):
		previousMissions.push_back(_currentMissionRes);

func start_mission(missionRes: MissionRes):
	_currentMissionRes = missionRes;
	missionRes.SetMissionManager(self);
	OnCurrentMissionChanged.emit();


func set_hidden(hidden: bool):
	isHidden = hidden;
	OnChangeVisibility.emit(hidden);
