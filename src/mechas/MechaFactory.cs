using Godot;
using GodotJamRound2.entites.ui;

namespace GodotJamRound2.mechas;

public partial class MechaFactory: Resource
{
    public MechaRes createMecha()
    {
        MechaRes mechaRes = new MechaRes();
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.RIGHT_ARM);
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.LEFT_ARM);
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.RIGHT_LEG);
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.LEFT_LEG);
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.TORSO);
        mechaRes.AddMechaPart(new MechaPartRes(), EMechaPartType.HEAD);
        return mechaRes;
    }
}