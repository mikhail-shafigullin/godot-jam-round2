class_name CurrentMission
extends Control

var _mission_manager: MissionManager
var _mission_label: RichTextLabel
var _task_container: Control

# Called when the node enters the scene tree for the first time.
func _ready():
	_mission_manager = Global._missionManager

	_mission_manager.OnCurrentMissionChanged.connect(_on_current_mission_changed);
	_mission_manager.OnChangeVisibility.connect(_on_visibility_changed);

	_mission_label = get_node("%HeaderRichText")
	_task_container = get_node("%TaskContainer")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _on_current_mission_changed():
	print("Current Mission Changed")
	visible = true
	var current_mission_res: MissionRes = _mission_manager.GetCurrentMission()
	_mission_label.text = current_mission_res._missionDescription
	
	for child in _task_container.get_children():
		_task_container.remove_child(child)
		child.queue_free()

	if current_mission_res._isComplete:
		_mission_label.text = "[s][color=green]" + _mission_label.text + "[/color]"

	for task: TaskRes in current_mission_res.GetTasks():
		var task_label = RichTextLabel.new()
		task_label.fit_content = true
		task_label.bbcode_enabled = true
		if task.IsComplete():
			task_label.text = "[s][color=green]" + task._name
		else:
			task_label.text = task._name
		_task_container.add_child(task_label)

func _on_visibility_changed(is_hidden: bool):
	visible = not is_hidden
