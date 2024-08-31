class_name MapUiController
extends Control

var _globals = null
var left_texture_rect = null
var top_texture_rect = null

var z_back_left_marker = null
var z_forward_left_marker = null
var y_top_left_marker = null
var y_bottom_left_marker = null

var z_back_top_marker = null
var z_forward_top_marker = null
var x_left_top_marker = null
var x_right_top_marker = null

var left_player_marker = null
var top_player_marker = null

var _game_controller = null
var _broken_part_markers = []

@export var _left_broken_part_template: Control = null
@export var _top_broken_part_template: Control = null

func _ready():
	Global._mapUiController = self;

	z_back_left_marker = get_node("%zBackLeft")
	z_forward_left_marker = get_node("%zForwardLeft")
	y_top_left_marker = get_node("%yTopLeft")
	y_bottom_left_marker = get_node("%yBottomLeft")

	z_back_top_marker = get_node("%zBackTop")
	z_forward_top_marker = get_node("%zForwardTop")
	x_left_top_marker = get_node("%xLeftTop")
	x_right_top_marker = get_node("%xRightTop")

	left_player_marker = get_node("%LeftPlayerMarker")
	top_player_marker = get_node("%TopPlayerMarker")

	left_player_marker.visible = true
	top_player_marker.visible = true

func _process(delta):
	draw_player_position()
	pass;

func draw_player_position():
	if _game_controller == null:
		_game_controller = Global._gameController

	var player_position_for_map = _game_controller.GetPlayerPositionForMap()

	var player_position_on_left_map = Vector2()
	player_position_on_left_map.x = z_back_left_marker.global_position.x + (z_forward_left_marker.global_position.x - z_back_left_marker.global_position.x) * player_position_for_map.z
	player_position_on_left_map.y = y_bottom_left_marker.global_position.y + (y_top_left_marker.global_position.y - y_bottom_left_marker.global_position.y) * player_position_for_map.y

	var player_position_on_top_map = Vector2()
	player_position_on_top_map.x = z_back_top_marker.global_position.x + (z_forward_top_marker.global_position.x - z_back_top_marker.global_position.x) * player_position_for_map.z
	player_position_on_top_map.y = x_left_top_marker.global_position.y + (x_right_top_marker.global_position.y - x_left_top_marker.global_position.y) * player_position_for_map.x

	left_player_marker.global_position = player_position_on_left_map
	top_player_marker.global_position = player_position_on_top_map

func add_broken_part(repair_trigger: RepairTrigger, position_on_map):
	var player_position_on_left_map = Vector2()
	player_position_on_left_map.x = z_back_left_marker.global_position.x + (z_forward_left_marker.global_position.x - z_back_left_marker.global_position.x) * position_on_map.z
	player_position_on_left_map.y = y_bottom_left_marker.global_position.y + (y_top_left_marker.global_position.y - y_bottom_left_marker.global_position.y) * position_on_map.y

	var left_broken_part_marker = _left_broken_part_template.duplicate()
	left_broken_part_marker.visible = true
	_left_broken_part_template.get_parent().add_child(left_broken_part_marker)
	_broken_part_markers.append(left_broken_part_marker)

	var player_position_on_top_map = Vector2()
	player_position_on_top_map.x = z_back_top_marker.global_position.x + (z_forward_top_marker.global_position.x - z_back_top_marker.global_position.x) * position_on_map.z
	player_position_on_top_map.y = x_left_top_marker.global_position.y + (x_right_top_marker.global_position.y - x_left_top_marker.global_position.y) * position_on_map.x

	var top_broken_part_marker = _top_broken_part_template.duplicate()
	top_broken_part_marker.visible = true
	_top_broken_part_template.get_parent().add_child(top_broken_part_marker)
	_broken_part_markers.append(top_broken_part_marker)

	left_broken_part_marker.global_position = player_position_on_left_map
	top_broken_part_marker.global_position = player_position_on_top_map

	repair_trigger.OnPartRepaired.connect(func():
		_broken_part_markers.erase(left_broken_part_marker)
		left_broken_part_marker.queue_free()
		_broken_part_markers.erase(top_broken_part_marker)
		top_broken_part_marker.queue_free()
	)
