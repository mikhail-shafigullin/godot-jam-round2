using Godot;
using Godot.Collections;
using GodotJamRound2.entites.ui;

namespace GodotJamRound2.mechas;

public partial class MechaRes: Resource
{
    private Dictionary<EMechaPartType, MechaPartRes> _mechaParts;
    
    public MechaRes()
    {
        _mechaParts = new Dictionary<EMechaPartType, MechaPartRes>();
    }
    
    public void AddMechaPart(MechaPartRes mechaPartRes, EMechaPartType mechaPartType)
    {
        _mechaParts[mechaPartType] = mechaPartRes;
        mechaPartRes.SetMechaPartType(mechaPartType);
    }
    
    public MechaPartRes GetMechaPart(EMechaPartType mechaPartType)
    {
        return _mechaParts[mechaPartType];
    }
}