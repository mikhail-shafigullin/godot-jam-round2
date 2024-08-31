class_name RepairTrigger
extends Node3D
	
var _isRepairing = false;
var _isRepaired = false;
	
var _isDisabled = true;
@export var _repairProgress = 0.0;
@export var _repairSpeed = 20.0;
	
var maxRepairProgress = BrokenPartRes.maxRepairProgress;
	
var _brokenPart: BrokenPartRes = null;
@onready var _textTrigger: RichTextLabel = %triggerLabel;
var _player: DronPlayer;

@onready var _area3D: Area3D = %Area3D;
@onready var effectsBlock: Node3D = %Effects;

@export var withEffects = true;

signal OnPartRepaired();


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	SetDisabled(true);
	effectsBlock.visible = withEffects;
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if (_isRepairing ):
		_repairProgress += _repairSpeed * delta;
		Global._ui.set_action_progress(_repairProgress);
		if(_repairProgress >= maxRepairProgress):
			_brokenPart.SetRepairProgress(_repairProgress);
			_isRepairing = false;
	
	if(_player == null):
		_player = Global._player;
	
	if(!_isDisabled && _player != null):
		var distanceTo = _player.global_position.distance_to(global_position);
		_textTrigger.text = str(distanceTo).pad_decimals(2)  + " m";


func Trigger() -> void: 
	if (!_isDisabled):
		var ui = Global._ui;
		ui.show_action_progress(true);

		print("Repairing...");
		_isRepairing = true;
	
func RemoveTrigger() -> void:
	if (!_isDisabled):
		var ui = Global._ui;
		ui.show_action_progress(false);
		_isRepairing = false;
		
		if(_brokenPart != null):
			_brokenPart.SetRepairProgress(_repairProgress);

func SetDisabled(disabled: bool) -> void:
	if(disabled):
		RemoveTrigger();
	_isDisabled = disabled;
	_area3D.monitoring = !disabled;
	visible = !disabled;

func SetBrokenPart(brokenPart: BrokenPartRes) -> void:
	_brokenPart = brokenPart;
	brokenPart.OnPartRepaired.connect(PartRepaired);

func PartRepaired():
	_isRepaired = true;
	SetDisabled(true);
	OnPartRepaired.emit();
	effectsBlock.visible = false;

func _on_area_3d_body_entered(node: Node3D):
	if(node is DronPlayer):
		var player = node;
		Trigger();
		player.repairing();

func _on_area_3d_body_exited(node: Node3D):
	if(node is DronPlayer):
		var player = node;
		RemoveTrigger();
		player.stop_repairing();
