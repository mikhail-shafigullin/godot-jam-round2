using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class MissionManager : Resource
{
    
    private MissionRes _currentMissionRes;
    private Array<MissionRes> previousMissions = new Array<MissionRes>();
    private bool isHidden = false;
    
    public MissionRes GetCurrentMission()
    {
        return _currentMissionRes;
    }
    
    
    public bool IsMissionComplete()
    {
        return _currentMissionRes.IsComplete();
    }
    
    public void OnTaskComplete()
    {
        EmitSignal(nameof(OnCurrentMissionChanged));
        if(IsMissionComplete())
        {
            previousMissions.Add(_currentMissionRes);
        }
    }

    public void StartMission(MissionRes missionRes)
    {
        _currentMissionRes = missionRes;
        missionRes.SetMissionManager(this);
        EmitSignal(nameof(OnCurrentMissionChanged));
    }

    [Signal]
    public delegate void OnCurrentMissionChangedEventHandler();

    [Signal]
    public delegate void OnChangeVisibilityEventHandler(bool hidden);
    
    public void SetHidden(bool hidden)
    {
        isHidden = hidden;
        EmitSignal(nameof(OnChangeVisibility), hidden);
    }
    
    
    
}