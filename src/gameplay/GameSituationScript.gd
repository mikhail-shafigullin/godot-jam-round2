class_name GameSituationScript
extends Resource

var _gameSituations = []
var _timer = null
var eventIndex = 0
var _isRunning = false
var _isFinished = false

func _init():
	_gameSituations = []

func add_game_situation(game_situation):
	_gameSituations.append(game_situation)

func run(timer):
	_timer = timer
	_isRunning = true
	run_next_situation()

func run_next_situation():
	if eventIndex == 0:
		_timer.timeout.connect(run_next_situation)
	if eventIndex >= _gameSituations.size():
		_timer.timeout.disconnect(run_next_situation)
		_isRunning = false
		_isFinished = true
		return

	var current_situation = _gameSituations[eventIndex]
	print("Game Situation Started ", current_situation.signal.name, " ", str(current_situation.signal.owner))
	_gameSituations[eventIndex].run(_timer)
	eventIndex += 1
