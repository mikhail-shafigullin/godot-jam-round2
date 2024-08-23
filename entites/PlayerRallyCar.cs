using Godot;
using System;

public partial class PlayerRallyCar : CharacterBody3D
{
	public const float Speed = 1.0f;
	public const float RotationSpeed = 1.5f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// {
		// 	velocity.Y = JumpVelocity;
		// }

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (inputDir.X != 0)
		{
			
			Rotation += new Vector3(0, -inputDir.X * RotationSpeed * (float)delta, 0);
		}
		
		if (inputDir.Y != 0)
		{
			// Slow down backward movement.
			if (inputDir.Y > 0)
			{
				inputDir = new Vector2(inputDir.X, inputDir.Y * 0.5f);
			}
			
			velocity.X = Transform.Basis.Z.X * inputDir.Y * Speed;
			velocity.Z = Transform.Basis.Z.Z * inputDir.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed * 2 * (float)delta);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, Speed * 2 * (float)delta);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
