using Godot;

namespace GodotJamRound2.mechas;

public partial class HangarRes: Resource
{
    private MechaRes _mechaRes = null;
    public Signal createMechaSignal = new Signal();
    public Signal launchMechaSignal = new Signal();
    
    public void SetMecha(MechaRes mechaRes)
    {
        _mechaRes = mechaRes;
    }
    
    public MechaRes GetMecha()
    {
        return _mechaRes;
    }
}