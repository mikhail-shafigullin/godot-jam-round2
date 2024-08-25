using Godot;
using System;
using GodotJamRound2.entites.mecha;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 8.5f;
	public const float MouseSensitivity = 0.1f;
	
	public Camera3D Camera = null;
	public ComputerUi ComputerUi = null;
	
	private Globals _globals = null;
	private ITriggerable _hoveredTrigger = null;
	private bool _computerOpened = false;

	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		Camera = GetNode<Camera3D>("Camera3D");
		ComputerUi = GetNode<ComputerUi>("%ComputerUi");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent && !_computerOpened)
		{
			Vector2 RotationDelta = new Vector2();
			RotationDelta.Y = Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity);
			RotationDelta.X = Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity);
			RotationDelta.X = Mathf.Clamp(RotationDelta.X, -70, 70);
			Camera.RotateX(RotationDelta.Y);
			RotateY(RotationDelta.X);
		}
		
		if(@event.IsActionPressed("player_use_action") && _hoveredTrigger != null && !_computerOpened)
		{
			_hoveredTrigger.Trigger();
		}

		if (@event.IsActionReleased("player_use_action") && _hoveredTrigger != null && !_computerOpened)
		{
			_hoveredTrigger.RemoveTrigger();
		}
		
		if(@event.IsActionPressed("player_computer"))
		{
			if (ComputerUi.Visible)
			{
				ComputerUi.Visible = false;
				Input.MouseMode = Input.MouseModeEnum.Captured;
				_computerOpened = false;
			}
			else
			{
				ComputerUi.Visible = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
				_computerOpened = true;
			}
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("player_jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("player_left", "player_right", "player_forward", "player_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		
		// Cast a ray from the camera center
		Vector3 from = Camera.GlobalTransform.Origin;
		Vector3 to = from - Camera.GlobalTransform.Basis.Z * 100; // Adjust the length as needed

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
		PhysicsRayQueryParameters3D rayParams = new PhysicsRayQueryParameters3D();
		rayParams.From = from;
		rayParams.To = to;
		rayParams.CollisionMask = 128;
		var result = spaceState.IntersectRay(rayParams);

		if (result.Count > 0)
		{
			var collider = (Node3D) result["collider"];
			var parent = collider.GetParent<ITriggerable>();
			if (parent != null)
			{
				_hoveredTrigger = parent;
				_globals.GetPlayerUI().ShowEventText(true);
				_globals.GetPlayerUI().SetEventText("Oppa");
			}
			else
			{
				GD.Print("No parent! " + collider.Name);
			}
		}else{
			_globals.GetPlayerUI().ShowEventText(false);
			_hoveredTrigger = null;
		}
	}
	
}
