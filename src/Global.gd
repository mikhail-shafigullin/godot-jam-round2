extends Node

var _ui: BasePlayerUI;
var _gameController: GameController;
var _missionManager: MissionManager;
var _mapUiController: MapUiController;
var _screenManager: ScreenManager;
var _player: DronPlayer;

var dome;

func _init() -> void:
	_missionManager = MissionManager.new();
