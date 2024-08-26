using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class DronPlayer : RigidBody3D
{
	Array<RayCast3D> raycasts = new Array<RayCast3D>();
	RayCast3D MiddleRayCast;
	private Camera3D Camera3D;
	private const float RotationSpeed = 1.0f;
	private const float MoveSpeed = 5.0f;
	private const float MouseSensitivity = 0.5f;
	
	private Vector3[] _bottomPoints = new Vector3[8];
	private Vector3[] _bottomNormals = new Vector3[8];
	
	private Vector3 middleCollisionPoint;
	private Vector3 middleCollisionNormal;
	private Vector3 middleCollisionNormalComputed;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		// Get all raycasts
		foreach (Node child in GetNode<Node>("Raycasts").GetChildren())
		{
			if (child is RayCast3D)
			{
				GD.Print("Ray cast added", child.ToString());
				raycasts.Add((RayCast3D)child);
			}
		}
		
		Camera3D = GetNode<Camera3D>("%Camera3D");
		MiddleRayCast = GetNode<RayCast3D>("%MiddleRayCast");
		
		foreach (var raycast in raycasts)
		{
			Vector3 directionToMiddle = (MiddleRayCast.GlobalTransform.Origin - raycast.GlobalTransform.Origin).Normalized();
			Vector3 newDirection = raycast.GlobalTransform.Basis.Z.Lerp(directionToMiddle, 0.1f).Normalized();
			raycast.GlobalTransform = new Transform3D(new Basis(newDirection, raycast.GlobalTransform.Basis.Y, raycast.GlobalTransform.Basis.X), raycast.GlobalTransform.Origin);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			Vector2 RotationDelta = new Vector2();
			RotationDelta.Y = Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity);
			RotationDelta.X = Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity);
			RotationDelta.X = Mathf.Clamp(RotationDelta.X, -70, 70);
			RotateX(RotationDelta.Y);
			// RotateY(RotationDelta.X);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 offsetVector = middleCollisionNormalComputed * 0.2f;
		GlobalTransform = new Transform3D(GlobalTransform.Basis, middleCollisionPoint + offsetVector);
		
		if (Input.IsActionPressed("player_forward"))
		{
			Translate(Vector3.Forward * MoveSpeed * (float)delta);
		}
		if (Input.IsActionPressed("player_backward"))
		{
			Translate(Vector3.Back * MoveSpeed * (float)delta);
		}
		if (Input.IsActionPressed("player_left"))
		{
			Translate(Vector3.Left * MoveSpeed * (float)delta);
		}
		if (Input.IsActionPressed("player_right"))
		{
			Translate(Vector3.Right * MoveSpeed * (float)delta);
		}
		
		int index = 0;
		foreach (var raycast in raycasts)
		{
			
			if (raycast.IsColliding())
			{
				_bottomPoints[index] = raycast.GetCollisionPoint();
				_bottomNormals[index] = raycast.GetCollisionNormal();
			}
			index++;
			
		}
		DebugDraw3D.DrawPoints(_bottomPoints);
		for(int i = 0; i < _bottomPoints.Length; i++)
		{
			DebugDraw3D.DrawLine(_bottomPoints[i], _bottomPoints[i] + _bottomNormals[i]);
		}


		if (MiddleRayCast.IsColliding())
		{
			middleCollisionPoint = MiddleRayCast.GetCollisionPoint();
			middleCollisionNormal = MiddleRayCast.GetCollisionNormal();
		}
		
		DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + middleCollisionNormal);

		middleCollisionNormalComputed = GetDotNormalForCollision();

		DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + middleCollisionNormalComputed,
			Colors.Green);

		RotatePlayerPerpendicularToNormal();
	}

	private Vector3 GetDotNormalForCollision()
	{
		Vector3 sum = Vector3.Zero;
		foreach (var normal in _bottomNormals)
		{
			sum += normal;
		}
		return sum / _bottomNormals.Length;
	}
	
	private void RotatePlayerPerpendicularToNormal()
	{
		// Compute the average normal vector
		Vector3 averageNormal = middleCollisionNormalComputed;

		// Calculate the target rotation quaternion from the normal vector
		Quaternion targetRotation = new Quaternion(Vector3.Up, averageNormal);
		
		// Convert the quaternion to a basis (rotation matrix)
		Basis targetBasis = new Basis(targetRotation);
		
		Basis currentBasis = GlobalTransform.Basis;
		Basis interpolatedBasis = currentBasis.Slerp(targetBasis, 0.1f);

		// Apply the rotation to the player
		GlobalTransform = new Transform3D(interpolatedBasis, GlobalTransform.Origin);
	}
}
