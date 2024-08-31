class_name BrokenPartRes
extends Resource

var _position: Vector3;
var repairProgress: float = 0;
static var maxRepairProgress: float = 100;
var isRepaired: bool = false;
var isDisabled: bool = false;

signal OnPartRepaired();
signal OnRepairChanged();
signal OnPartDisabled();

func SetPosition(position: Vector3):
	_position = position;

func GetPosition() -> Vector3:
	return _position;

func SetRepairProgress(progress: float):
	if (!isRepaired):
		repairProgress = clamp(progress, 0, maxRepairProgress);
		OnRepairChanged.emit()
		if(repairProgress >= maxRepairProgress):
			isRepaired = true;
			OnPartRepaired.emit()

func SetDisabled(disabled: bool):
	isDisabled = disabled;
	OnPartDisabled.emit();
