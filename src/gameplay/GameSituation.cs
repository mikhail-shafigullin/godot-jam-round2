using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class GameSituation: Resource
{
    public Signal signal;
    public float time;

    public GameSituation(Signal signal, float time)
    {
        this.signal = signal;
        this.time = time;
    }
}