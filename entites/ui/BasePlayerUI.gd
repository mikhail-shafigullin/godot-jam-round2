class_name BasePlayerUI
extends Control

var _event_text: RichTextLabel = null
var _action_progress: TextureProgressBar = null
var _repair_block: Control = null
var _screen_manager: ScreenManager = null
var _tutorial_block: Control = null
var _leftBlock: Control = null;

func _ready():
	AudioServer.set_bus_volume_db(0, lerpf(-80, 6, 0.8))

	Global._ui = self;

	_event_text = get_node("%EventText")
	_action_progress = get_node("%ActionProgress")

	_repair_block = get_node("%RepairBlock")
	_screen_manager = get_node("%ScreenManager")

	_tutorial_block = get_node("%TutorialBlock")
	_leftBlock = get_node("%LeftBlock")

	DialogueManager.got_dialogue.connect(_on_got_dialogue)
	DialogueManager.dialogue_ended.connect(_on_dialogue_ended)
	
func _gui_input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	
	if Input.is_action_just_pressed("ui_cancel"):
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _on_got_dialogue(dialogue_line):
	_tutorial_block.visible = false
	_leftBlock.visible = false;

func _on_dialogue_ended(dialogue_resource):
	_tutorial_block.visible = true
	_leftBlock.visible = true;

func show_event_text(show):
	_event_text.visible = show

func show_action_progress(show):
	_repair_block.visible = show

func set_event_text(text):
	_event_text.text = "[center]" + text

func set_action_progress(value):
	_action_progress.value = value


func _on_volume_slider_value_changed(value):
	AudioServer.set_bus_volume_db(0, lerpf(-80, 6, value))
