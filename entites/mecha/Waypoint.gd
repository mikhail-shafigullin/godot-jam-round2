class_name Waypoint
extends Node3D

var Camera: Camera3D;
var Origin: Node2D;
var _visibleOnScreenNotifier: VisibleOnScreenNotifier3D;

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	Camera = get_viewport().get_camera_3d();
	Origin = get_node("Origin");
	_visibleOnScreenNotifier = get_node("VisibleOnScreenNotifier3D");
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var markerPos = Camera.unproject_position(global_transform.origin);
	Origin.position = markerPos;
	
	if(_visibleOnScreenNotifier.is_on_screen()):
		Origin.visible = true;
	else:
		Origin.visible = false;
