extends Node3D

func _ready():
	Global.cutscener = self

func end():
	get_node("./AnimationPlayer").play("end")
