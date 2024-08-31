class_name DronPlayer
extends CharacterBody3D

var raycasts: Array[RayCast3D] = []
var MiddleRayCast: RayCast3D
var _Camera3D: Camera3D
var _SpringArm3D: SpringArm3D
var RotateXNode: Node3D
var minArm = 2.0
var maxArm = 7.0

var Visual: Node3D
const RotationSpeed = 1.0
const MoveSpeed = 30.0
const FastMoveSpeed = 50.0
const MouseSensitivity = 0.4

var _bottomPoints: Array[Vector3] = []
var _bottomNormals: Array[Vector3] = []

var inputDirection = Vector3.ZERO
var strongInputDirection = Vector3.ZERO
var middleCollisionPoint = Vector3.ZERO
var middleCollisionNormal = Vector3.ZERO
var middleCollisionNormalComputed = Vector3.ZERO

var isMoving = false
var isFirstMove = true

var _computerUi: ComputerUi = null
var _computerOpened = false

var _AnimationPlayer: AnimationPlayer
var isControlsDisabled = false
var isEscapePressed = false

func _ready():
	_bottomPoints.resize(32);
	_bottomNormals.resize(32);
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

	# Get all raycasts
	for child in get_node("Raycasts").get_children():
		if child is RayCast3D:
			raycasts.append(child)

	_Camera3D = get_node("%Camera3D")
	MiddleRayCast = get_node("%MiddleRayCast")
	_SpringArm3D = get_node("%SpringArm")
	Visual = get_node("%Visual")
	RotateXNode = get_node("%RotateXNode")
	_computerUi = get_node("%ComputerUi")
	_AnimationPlayer = get_node("%spacePlayerRobot/AnimationPlayer")

	strongInputDirection = global_transform.basis * Vector3.FORWARD

func _input(event):
	if event is InputEventMouseMotion and not _computerOpened and not isControlsDisabled:
		var RotationDelta = Vector2()
		RotationDelta.x = deg_to_rad(-event.relative.x * MouseSensitivity)
		RotationDelta.y = deg_to_rad(-event.relative.y * MouseSensitivity)
		if middleCollisionNormalComputed != Vector3.ZERO:
			RotateXNode.rotate_y(RotationDelta.x)
			_SpringArm3D.rotate_x(RotationDelta.y)
			_SpringArm3D.rotation.x = clamp(_SpringArm3D.rotation.x, -PI/3, PI/4);
			_SpringArm3D.rotation.z = 0;

	if event.is_action_pressed("player_computer") and not isControlsDisabled:
		if _computerUi.visible:
			_computerUi.visible = false
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
			_computerOpened = false
		else:
			_computerUi.visible = true
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
			_computerOpened = true

	if event.is_action_pressed("game_menu"):
		if isEscapePressed:
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
		else:
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
		isEscapePressed = not isEscapePressed

func _process(delta):
	isMoving = false
	var offsetVector = middleCollisionNormal * 0.3
	global_transform = Transform3D(global_transform.basis, middleCollisionPoint + offsetVector);

	var cameraBackward = _Camera3D.global_transform.basis.z.normalized()
	var cameraBackwardProject = cameraBackward - cameraBackward.project(middleCollisionNormalComputed)
	var cameraLeft = cameraBackward.cross(middleCollisionNormalComputed)
	var cameraLeftProject = cameraLeft - cameraLeft.project(middleCollisionNormalComputed)
	
	DebugDraw3D.draw_line(middleCollisionPoint, middleCollisionPoint + cameraLeftProject);
	DebugDraw3D.draw_line(middleCollisionPoint, middleCollisionPoint + cameraBackwardProject);

	if Input.is_action_pressed("camera_zoom_up") and not isControlsDisabled:
		_SpringArm3D.spring_length = clamp(_SpringArm3D.spring_length + 0.1, minArm, maxArm)

	if Input.is_action_pressed("camera_zoom_down") and not isControlsDisabled:
		_SpringArm3D.spring_length = clamp(_SpringArm3D.spring_length - 0.1, minArm, maxArm)

	for i in 32:
		if raycasts[i].is_colliding():
			_bottomPoints[i] = raycasts[i].get_collision_point()
			_bottomNormals[i] = raycasts[i].get_collision_normal()

	DebugDraw3D.draw_points(_bottomPoints);
	for i in _bottomPoints.size():
		DebugDraw3D.draw_line(_bottomPoints[i], _bottomPoints[i] + _bottomNormals[i]);

	if MiddleRayCast.is_colliding():
		middleCollisionPoint = MiddleRayCast.get_collision_point()
		middleCollisionNormal = MiddleRayCast.get_collision_normal().normalized()

	middleCollisionNormalComputed = get_dot_normal_for_collision().normalized()

	align_with_floor()

	velocity = Vector3.ZERO

	var velocityInput = Vector3.ZERO
	if Input.is_action_pressed("player_forward") and not isControlsDisabled:
		isMoving = true
		velocityInput += -cameraBackward.normalized()
	if Input.is_action_pressed("player_backward") and not isControlsDisabled:
		isMoving = true
		velocityInput += cameraBackward.normalized()
	if Input.is_action_pressed("player_left") and not isControlsDisabled:
		isMoving = true
		velocityInput += cameraLeft.normalized()
	if Input.is_action_pressed("player_right") and not isControlsDisabled:
		isMoving = true
		velocityInput += -cameraLeft.normalized()

	if isMoving:
		var speed = MoveSpeed
		if Input.is_action_pressed("player_accelerate") and not isControlsDisabled:
			speed = FastMoveSpeed
		velocity = velocityInput.normalized() * speed
		move_and_slide()

	if isMoving:
		var currentRotation = Visual.global_transform.basis.orthonormalized().get_rotation_quaternion()
		var targetRotation = transform.looking_at(Visual.global_transform.origin - cameraBackwardProject, middleCollisionNormalComputed).basis.orthonormalized().get_rotation_quaternion()
		var TargetedTransform = Transform3D(Basis(targetRotation), Visual.global_transform.origin)
		Visual.global_transform = Visual.global_transform.interpolate_with(transform.looking_at(Visual.global_transform.origin - cameraBackwardProject, middleCollisionNormalComputed).orthonormalized(), 0.1)

	_Camera3D.global_scale(Vector3(1, 1, 1))

func get_dot_normal_for_collision() -> Vector3:
	var sum = Vector3.ZERO
	for normal in _bottomNormals:
		sum += normal
	return sum / _bottomNormals.size()

func align_with_floor():
	var newX = -global_transform.basis.z.cross(middleCollisionNormalComputed)
	var newY = middleCollisionNormalComputed
	var newZ = global_transform.basis.z
	var basis = Basis(newX, newY, newZ).orthonormalized()
	var trans = Transform3D(basis, global_transform.origin)
	global_transform = global_transform.interpolate_with(trans, 0.2)

func repairing():
	_AnimationPlayer.play("weld")

func stop_repairing():
	_AnimationPlayer.play("idleFly")

func set_controls_disabled(disabled: bool):
	isControlsDisabled = disabled
