using System;
using Godot;
using GodotJamRound2.mechas;

namespace GodotJamRound2.ship;

public partial class BrokenPartRes: Resource
{
    private Vector3 _position;
    private float repairProgress = 0;
    private float maxRepairProgress = 100;
    private bool isRepaired = false;
    
    public void SetPosition(Vector3 position)
    {
        _position = position;
    }
    
    public Vector3 GetPosition()
    {
        return _position;
    }
    
    public void SetRepairProgress(float progress)
    {
        repairProgress = progress;
        EmitSignal(nameof(OnRepairChanged));
    }
    
    [Signal]
    public delegate void OnPartRepairedEventHandler();
    
    [Signal]
    public delegate void OnRepairChangedEventHandler();
}