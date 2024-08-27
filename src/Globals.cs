using Godot;
using System;
using GodotJamRound2.gameplay;

public partial class Globals : Node
{
	private BasePlayerUI _ui;
	private GameController _gameController;
	private MissionManager _missionManager;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_missionManager = new MissionManager();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetPlayerUI(BasePlayerUI ui)
	{
		_ui = ui;
	}
	
	public BasePlayerUI GetPlayerUI()
	{
		return _ui;
	}
	
	public void SetGameController(GameController gameController)
	{
		_gameController = gameController;
	}
	
	public GameController GetGameController()
	{
		return _gameController;
	}
	
	public MissionManager GetMissionManager()
	{
		return _missionManager;
	}
	
}
