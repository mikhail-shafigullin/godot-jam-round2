using Godot;
using System;
using Godot.Collections;
using GodotJamRound2.gameplay;
using GodotJamRound2.mechas;
using Array = System.Array;

public partial class GameController : Node
{
	[Export]
	private Array<MechaPanel> _mechaPanels = new Array<MechaPanel>();
	
	private Globals _globals = null;
	private Array<HangarRes> _hangars = new Array<HangarRes>();
	
	private Array<GameSituationScript> _gameSituationScripts = new Array<GameSituationScript>();
	
	private Timer _timer;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<Globals>("/root/Globals");
		_globals.SetGameController(this);
		
		_hangars.Add(new HangarRes());
		_hangars.Add(new HangarRes());
		_hangars.Add(new HangarRes());
		
		if(_mechaPanels.Count != 3){
			GD.PrintErr("There should be 3 mecha panels");
		}
		
		_mechaPanels[0].SetHangarRes(_hangars[0]);
		
		_timer = new Timer();
		AddChild(_timer);

		CreateScripts();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void CreateScripts()
	{
		_gameSituationScripts = new Array<GameSituationScript>();
		var firstScript = new GameSituationScript();
		_gameSituationScripts.Add(firstScript);
		firstScript.AddGameSituation(new GameSituation(_hangars[0].createMechaSignal, 20));
		firstScript.AddGameSituation(new GameSituation(_hangars[1].createMechaSignal, 30));
		
		var secondScript = new GameSituationScript();
		_gameSituationScripts.Add(secondScript);
		
		var thirdScript = new GameSituationScript();
		_gameSituationScripts.Add(thirdScript);
	}

	private void RunGameSituationScripts()
	{
		
	}
	
	
}
