using System;
using Godot;
using Godot.Collections;

namespace GodotJamRound2.gameplay;

public partial class MissionRes: Resource
{
    private String _missionDescription;
    
    private Array<TaskRes> tasks = new Array<TaskRes>();
    
    private bool _isComplete = false;
    
    public MissionRes(String missionDescription)
    {
        _missionDescription = missionDescription;
    }
    
    public void AddTask(TaskRes taskRes)
    {
        tasks.Add(taskRes);
        taskRes.OnTaskComplete += CheckTasks; 
    }
    
    public void CheckTasks()
    {
        bool allTasksComplete = true;
        foreach(TaskRes task in tasks)
        {
            if(!task.IsComplete())
            {
                allTasksComplete = false;
                break;
            }
        }
        
        if(allTasksComplete)
        {
            _isComplete = true;
        }
    }
    
    public bool IsComplete()
    {
        return _isComplete;
    }

    public string GetMissionDescription()
    {
        return _missionDescription;
    }
    
    public Array<TaskRes> GetTasks()
    {
        return tasks;
    }
}