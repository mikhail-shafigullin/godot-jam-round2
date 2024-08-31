class_name ComputerUi
extends Node

var mapUIController: MapUiController;

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	DialogueManager.got_dialogue.connect(_on_got_dialogue)
	DialogueManager.dialogue_ended.connect(_on_dialogue_ended)
	
	mapUIController = get_node("%MapUIController");
	pass # Replace with function body.
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_got_dialogue(dialogue_line):
	mapUIController.visible = false

func _on_dialogue_ended(dialogue_resource):
	mapUIController.visible = true
