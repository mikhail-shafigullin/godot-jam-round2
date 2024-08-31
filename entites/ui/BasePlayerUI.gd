class_name BasePlayerUI
extends Control

var _event_text: RichTextLabel = null
var _action_progress: TextureProgressBar = null
var _repair_block: Control = null
var _screen_manager: ScreenManager = null
var _tutorial_block: Control = null

func _ready():
	Global._ui = self;

	_event_text = get_node("%EventText")
	_action_progress = get_node("%ActionProgress")

	_repair_block = get_node("%RepairBlock")
	_screen_manager = get_node("%ScreenManager")

	_tutorial_block = get_node("%TutorialBlock")

	DialogueManager.got_dialogue.connect(_on_got_dialogue)
	DialogueManager.dialogue_ended.connect(_on_dialogue_ended)

func _on_got_dialogue(dialogue_line):
	_tutorial_block.visible = false

func _on_dialogue_ended(dialogue_resource):
	_tutorial_block.visible = true

func show_event_text(show):
	_event_text.visible = show

func show_action_progress(show):
	_repair_block.visible = show

func set_event_text(text):
	_event_text.text = "[center]" + text

func set_action_progress(value):
	_action_progress.value = value
