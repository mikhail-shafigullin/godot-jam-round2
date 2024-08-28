using Godot;
using System;
using System.Linq;
using System.Numerics;
using Godot.Collections;
using Quaternion = Godot.Quaternion;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class DronPlayer : CharacterBody3D
{
	Array<RayCast3D> raycasts = new Array<RayCast3D>();
	RayCast3D MiddleRayCast;
	public Camera3D Camera3D;
	private SpringArm3D SpringArm3D;
	private Node3D RotateXNode;
	private float minArm = 1.5f;
	private float maxArm = 5.0f;
	
	private Node3D Visual;
	private const float RotationSpeed = 1.0f;
	private const float MoveSpeed = 5.0f;
	private const float MouseSensitivity = 0.4f;
	
	private Vector3[] _bottomPoints = new Vector3[8];
	private Vector3[] _bottomNormals = new Vector3[8];

	private Vector3 inputDirection;
	private Vector3 strongInputDirection;
	private Vector3 middleCollisionPoint;
	private Vector3 middleCollisionNormal;
	private Vector3 middleCollisionNormalComputed;

	private bool isMoving = false;
	private bool isFirstMove = true;
	
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
		SpringArm3D = GetNode<SpringArm3D>("%SpringArm");
		Visual = GetNode<Node3D>("%Visual");
		RotateXNode = GetNode<Node3D>("%RotateXNode");

		strongInputDirection = GlobalBasis * Vector3.Forward;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
		{
			Vector2 RotationDelta = new Vector2();
			RotationDelta.X = Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity);
			RotationDelta.Y = Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity);
			if (middleCollisionNormalComputed != Vector3.Zero)
			{
				RotateXNode.RotateY(RotationDelta.X);
				
				SpringArm3D.RotateX(RotationDelta.Y);
				SpringArm3D.Rotation = new Vector3((float)Mathf.Clamp(SpringArm3D.Rotation.X, -Math.PI/3, Math.PI/4), SpringArm3D.Rotation.Y, 0);
				// Rotate(middleCollisionNormalComputed, RotationDelta.X);	

			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		isMoving = false;
		Vector3 offsetVector = middleCollisionNormal * 0.2f;
		GlobalTransform = new Transform3D(GlobalTransform.Basis, middleCollisionPoint + offsetVector);
		
		
		Vector3 cameraBackward = Camera3D.GlobalTransform.Basis.Z.Normalized();
		Vector3 cameraBackwardProject = cameraBackward - cameraBackward.Project(middleCollisionNormal);
		Vector3 cameraRight = cameraBackward.Cross(middleCollisionNormal);
		Vector3 cameraRightProject = cameraRight - cameraRight.Project(middleCollisionNormal);
		Vector3 localCameraBackward = cameraBackwardProject * Basis;
		Vector3 localCameraRight = cameraRightProject * Basis;
		DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + cameraRightProject);
		DebugDraw3D.DrawLine(middleCollisionPoint, middleCollisionPoint + cameraBackwardProject);
		
		Velocity = Vector3.Zero;

		Vector3 velocityInput = Vector3.Zero;
		if (Input.IsActionPressed("player_forward"))
		{
			isMoving = true;
			velocityInput = -cameraBackwardProject.Normalized();
		}
		if (Input.IsActionPressed("player_backward"))
		{
			isMoving = true;
			velocityInput = cameraBackwardProject.Normalized();
		}
		if (Input.IsActionPressed("player_left"))
		{
			isMoving = true;
			velocityInput = -cameraRightProject.Normalized();
		}
		if (Input.IsActionPressed("player_right"))
		{
			isMoving = true;
			velocityInput = cameraRightProject.Normalized();
		}

		if (isMoving)
		{
			Velocity = velocityInput.Normalized() * MoveSpeed;
			MoveAndSlide();			
		}

		if (Input.IsActionPressed("camera_zoom_up"))
		{
			SpringArm3D.SpringLength = Mathf.Clamp(SpringArm3D.SpringLength + 0.1f, minArm, maxArm);
		}

		if (Input.IsActionPressed("camera_zoom_down"))
		{
			SpringArm3D.SpringLength = Mathf.Clamp(SpringArm3D.SpringLength - 0.1f, minArm, maxArm);
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
		
		
		
		if (isMoving)
		{
			if (isFirstMove)
			{
				Visual.GlobalTransform = Transform
					.LookingAt(Visual.GlobalTransform.Origin - cameraBackwardProject, middleCollisionNormal)
					.Orthonormalized();
				isFirstMove = false;
			}
			
			Quaternion currentRotation = Visual.GlobalTransform.Basis.Orthonormalized().GetRotationQuaternion();
			Quaternion targetRotation = Transform
				.LookingAt(Visual.GlobalTransform.Origin - cameraBackwardProject, middleCollisionNormalComputed)
				.Basis
				.Orthonormalized()
				.GetRotationQuaternion();
			
			Transform3D TargetedTransform = new Transform3D(new Basis(targetRotation), Visual.GlobalTransform.Origin);
			Visual.GlobalTransform = Visual.GlobalTransform.InterpolateWith(
				Transform
				.LookingAt(Visual.GlobalTransform.Origin - cameraBackwardProject, middleCollisionNormalComputed)
				.Orthonormalized(), 
				0.1f);
		}
		
		
		
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
		GlobalTransform = GlobalTransform.InterpolateWith(trans, 0.2f);
	}
}
