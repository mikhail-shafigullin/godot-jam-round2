using Godot;
using System;
using Godot.Collections;
using GodotJamRound2.gameplay;
using GodotJamRound2.ship;

public partial class MapUiController : Control
{
	private Globals _globals = null;
	
	private TextureRect leftTextureRect;
	private TextureRect topTextureRect;

	private Control zBackLeftMarker;
	private Control zForwardLeftMarker;
	private Control yTopLeftMarker;
	private Control yBottomLeftMarker;
	
	private Control zBackTopMarker;
	private Control zForwardTopMarker;
	private Control xLeftTopMarker;
	private Control xRightTopMarker;

	private Control LeftPlayerMarker;
	private Control TopPlayerMarker;
	
	private GameController _gameController;
	
	private Array<Control> _brokenPartMarkers = new Array<Control>();
	
	[Export]
	private Control _leftBrokenPartTemplate;
	[Export]
	private Control _topBrokenPartTemplate;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetMapUiController(this);
		
		zBackLeftMarker = GetNode<Control>("%zBackLeft");
		zForwardLeftMarker = GetNode<Control>("%zForwardLeft");
		yTopLeftMarker = GetNode<Control>("%yTopLeft");
		yBottomLeftMarker = GetNode<Control>("%yBottomLeft");
		
		zBackTopMarker = GetNode<Control>("%zBackTop");
		zForwardTopMarker = GetNode<Control>("%zForwardTop");
		xLeftTopMarker = GetNode<Control>("%xLeftTop");
		xRightTopMarker = GetNode<Control>("%xRightTop");
		
		LeftPlayerMarker = GetNode<Control>("%LeftPlayerMarker");
		TopPlayerMarker = GetNode<Control>("%TopPlayerMarker");

		LeftPlayerMarker.Visible = true;
		TopPlayerMarker.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DrawPlayerPosition();
	}

	public void DrawPlayerPosition()
	{
		if (_gameController == null)
		{
			_gameController = _globals.GetGameController();
		}

		Vector3 playerPositionForMap = _gameController.GetPlayerPositionForMap();

		Vector2 playerPositionOnLeftMap = Vector2.Zero;
		playerPositionOnLeftMap.X = zBackLeftMarker.GlobalPosition.X + (zForwardLeftMarker.GlobalPosition.X - zBackLeftMarker.GlobalPosition.X) * playerPositionForMap.Z;
		playerPositionOnLeftMap.Y = yBottomLeftMarker.GlobalPosition.Y + (yTopLeftMarker.GlobalPosition.Y - yBottomLeftMarker.GlobalPosition.Y) * playerPositionForMap.Y;
		
		Vector2 playerPositionOnTopMap = Vector2.Zero;
		playerPositionOnTopMap.X = zBackTopMarker.GlobalPosition.X + (zForwardTopMarker.GlobalPosition.X - zBackTopMarker.GlobalPosition.X) * playerPositionForMap.Z;
		playerPositionOnTopMap.Y = xLeftTopMarker.GlobalPosition.Y + (xRightTopMarker.GlobalPosition.Y - xLeftTopMarker.GlobalPosition.Y) * playerPositionForMap.X;
		
		LeftPlayerMarker.GlobalPosition = playerPositionOnLeftMap;
		TopPlayerMarker.GlobalPosition = playerPositionOnTopMap;
	}
	
	public void AddBrokenPart(RepairTrigger repairTrigger, Vector3 positionOnMap)
	{
		
		Vector2 playerPositionOnLeftMap = Vector2.Zero;
		playerPositionOnLeftMap.X = zBackLeftMarker.GlobalPosition.X + (zForwardLeftMarker.GlobalPosition.X - zBackLeftMarker.GlobalPosition.X) * positionOnMap.Z;
		playerPositionOnLeftMap.Y = yBottomLeftMarker.GlobalPosition.Y + (yTopLeftMarker.GlobalPosition.Y - yBottomLeftMarker.GlobalPosition.Y) * positionOnMap.Y;
		
		Control leftBrokenPartMarker = (Control)_leftBrokenPartTemplate.Duplicate();
		leftBrokenPartMarker.Visible = true;
		_leftBrokenPartTemplate.GetParent().AddChild(leftBrokenPartMarker);
		_brokenPartMarkers.Add(leftBrokenPartMarker);
		
		Vector2 playerPositionOnTopMap = Vector2.Zero;
		playerPositionOnTopMap.X = zBackTopMarker.GlobalPosition.X + (zForwardTopMarker.GlobalPosition.X - zBackTopMarker.GlobalPosition.X) * positionOnMap.Z;
		playerPositionOnTopMap.Y = xLeftTopMarker.GlobalPosition.Y + (xRightTopMarker.GlobalPosition.Y - xLeftTopMarker.GlobalPosition.Y) * positionOnMap.X;
		Control topBrokenPartMarker = (Control)_topBrokenPartTemplate.Duplicate();
		topBrokenPartMarker.Visible = true;
		_topBrokenPartTemplate.GetParent().AddChild(topBrokenPartMarker);
		_brokenPartMarkers.Add(topBrokenPartMarker);
		
		leftBrokenPartMarker.GlobalPosition = playerPositionOnLeftMap;
		topBrokenPartMarker.GlobalPosition = playerPositionOnTopMap;
		
		repairTrigger.OnPartRepaired += () =>
		{
			_brokenPartMarkers.Remove(leftBrokenPartMarker);
			leftBrokenPartMarker.QueueFree();
			_brokenPartMarkers.Remove(topBrokenPartMarker);
			topBrokenPartMarker.QueueFree();
		};
	}
	
}
