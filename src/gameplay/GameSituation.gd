class_name GameSituation
extends Resource

var runableSignal: Signal
var time: float

func _init(runableSignal: Signal, time: float) -> void:
	self.runableSignal = runableSignal
	self.time = time

func run(timer: Timer) -> void:
	timer.start(time)
	run_signal()

func run_signal() -> void:
	runableSignal.get_object().emit_signal(runableSignal.get_name())
