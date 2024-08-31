class_name TaskRes
extends Resource

var _name: String;
var _taskCompleteSignal: Signal;
	
var _isComplete: bool = false;

signal OnTaskComplete();

func init(name: String, taskCompleteSignal: Signal):
	_name = name;
	_taskCompleteSignal = taskCompleteSignal;

	var completeCallable: Callable = Complete;
	var obj: Object = _taskCompleteSignal.get_object();
	obj.connect(_taskCompleteSignal.get_name(), completeCallable);

func SetName(name: String):
	_name = name;

func GetName() -> String:
	return _name;

func IsComplete() -> bool:
	return _isComplete;

func Complete():
	print("Task " + _name + " is completed!");
	_isComplete = true;
	OnTaskComplete.emit();
