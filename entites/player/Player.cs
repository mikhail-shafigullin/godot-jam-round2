using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.1f;
	
	public Camera3D Camera = null;

	public override void _Ready()
	{
		Camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			Vector2 RotationDelta = new Vector2();
			RotationDelta.Y = Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity);
			RotationDelta.X = Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity);
			RotationDelta.X = Mathf.Clamp(RotationDelta.X, -70, 70);
			Camera.RotateX(RotationDelta.Y);
			RotateY(RotationDelta.X);
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
	}
}
