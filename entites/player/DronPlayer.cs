using Godot;
using System;
using System.Linq;
using System.Numerics;
using Godot.Collections;
using Quaternion = Godot.Quaternion;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class DronPlayer : RigidBody3D
{
	Array<RayCast3D> raycasts = new Array<RayCast3D>();
	RayCast3D MiddleRayCast;
	private Camera3D Camera3D;
	private const float RotationSpeed = 1.0f;
	private const float MoveSpeed = 15.0f;
	private const float MouseSensitivity = 0.5f;
	
	private Vector3[] _bottomPoints = new Vector3[8];
	private Vector3[] _bottomNormals = new Vector3[8];

	private Vector3 inputDirection;
	private Vector3 strongInputDirection;
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
				raycasts.Add((RayCast3D)child);
			}
		}
		
		Camera3D = GetNode<Camera3D>("%Camera3D");
		MiddleRayCast = GetNode<RayCast3D>("%MiddleRayCast");

		strongInputDirection = GlobalBasis * Vector3.Forward;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			GD.Print(mouseEvent.Relative);
			Vector2 RotationDelta = new Vector2();
			RotationDelta.X = Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity);
			RotationDelta.X = Mathf.Clamp(RotationDelta.X, -70, 70);
			if (middleCollisionNormalComputed != Vector3.Zero)
			{
				Rotate(middleCollisionNormalComputed, RotationDelta.X);	
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 offsetVector = middleCollisionNormal * 0.2f;
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
		
		
		Translate(inputDirection * MoveSpeed * (float)delta);
		
		
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
		// DebugDraw3D.DrawPoints(_bottomPoints);
		// for(int i = 0; i < _bottomPoints.Length; i++)
		// {
		// 	DebugDraw3D.DrawLine(_bottomPoints[i], _bottomPoints[i] + _bottomNormals[i]);
		// }


		if (MiddleRayCast.IsColliding())
		{
			middleCollisionPoint = MiddleRayCast.GetCollisionPoint();
			middleCollisionNormal = MiddleRayCast.GetCollisionNormal().Normalized();
		}
		
		// DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + middleCollisionNormal);

		middleCollisionNormalComputed = GetDotNormalForCollision().Normalized();

		// DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + middleCollisionNormalComputed,
		// 	Colors.Green);
		
		AlignWithFloor();
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

	private void AlignWithFloor()
	{
		Vector3 newX = GlobalTransform.Basis.Z.Cross(middleCollisionNormalComputed);
		Vector3 newY = middleCollisionNormalComputed;
		Vector3 newZ = GlobalTransform.Basis.Z;
		Basis basis = new Basis(newX, newY, newZ).Orthonormalized();
		Transform3D trans = new Transform3D(basis, GlobalTransform.Origin);
		GlobalTransform = GlobalTransform.InterpolateWith(trans, 0.1f);
	}
}
