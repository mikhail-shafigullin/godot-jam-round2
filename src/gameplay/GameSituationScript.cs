using System;
using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class GameSituationScript: Resource
{
    Array<GameSituation> _gameSituations;
    private Timer _timer;
    private int eventIndex = 0;
    private bool _isRunning = false;
    private bool _isFinished = false;
    
    public GameSituationScript()
    {
        _gameSituations = new Array<GameSituation>();
    }
    
    public void AddGameSituation(GameSituation gameSituation)
    {
        _gameSituations.Add(gameSituation);
    }

    public void Run(Timer timer)
    {
        _timer = timer;
        _isRunning = true;
        RunNextSituation();
    }

    private void RunNextSituation()
    {
        if(eventIndex == 0)
        {
            _timer.Timeout += RunNextSituation;
        }
        if(eventIndex >= _gameSituations.Count)
        {
            _timer.Timeout -= RunNextSituation;
            _isRunning = false;
            _isFinished = true;
            return;
        }
        
        GameSituation currentSituation = _gameSituations[eventIndex];
        GD.Print("Game Situation Started ", currentSituation.signal.Name , " ", currentSituation.signal.Owner.ToString());
        _gameSituations[eventIndex].Run(_timer);
        eventIndex++;
    }

}