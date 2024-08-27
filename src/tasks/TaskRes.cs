using System;
using Godot;

namespace GodotJamRound2.gameplay;

public partial class TaskRes : Resource
{
    private String _name;
    private Signal _taskCompleteSignal;
    
    private bool _isComplete = false;
    
    
    public TaskRes(String name, Signal taskCompleteSignal)
    {
        _name = name;
        _taskCompleteSignal = taskCompleteSignal;

        Callable completeCallable = new Callable(this, "Complete");
        _taskCompleteSignal.Owner.Connect(_taskCompleteSignal.Name, completeCallable);
    }
    
    public void SetName(String name)
    {
        _name = name;
    }
    
    public String GetName()
    {
        return _name;
    }
    
    public bool IsComplete()
    {
        return _isComplete;
    }
    
    public void Complete()
    {
        _isComplete = true;
    }
    
    [Signal]
    public delegate void OnTaskCompleteEventHandler();
}