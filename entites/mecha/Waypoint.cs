using Godot;
using System;

public partial class Waypoint : Node3D
{
	private Camera3D Camera;
	private Node2D Origin;
	private VisibleOnScreenNotifier3D _visibleOnScreenNotifier;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Camera = GetViewport().GetCamera3D();
		Origin = GetNode<Node2D>("Origin");
		_visibleOnScreenNotifier = GetNode<VisibleOnScreenNotifier3D>("VisibleOnScreenNotifier3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var markerPos = Camera.UnprojectPosition(GlobalTransform.Origin);
		Origin.Position = markerPos;
		
		if(_visibleOnScreenNotifier.IsOnScreen())
		{
			Origin.Visible = true;
		}
		else
		{
			Origin.Visible = false;
		}
	}
}
