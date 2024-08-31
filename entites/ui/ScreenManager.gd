class_name ScreenManager
extends Control

var _animationPlayer: AnimationPlayer
var _colorRect: ColorRect

# Called when the node enters the scene tree for the first time.
func _ready():
	_animationPlayer = get_node("%AnimationPlayer")
	Global._screenManager = self;

	_colorRect = get_node("%ColorRect")
	_colorRect.visible = true

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func play_fade_out():
	_animationPlayer.play("fade_out")
