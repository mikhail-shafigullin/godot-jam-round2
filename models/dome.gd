extends Node3D

func _ready():
	Global.dome = self
	
func boom():
	$AnimationPlayer.play("boom")
