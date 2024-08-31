using System;
using Godot;
using Godot.Collections;

namespace GodotJamRound2.ship;

public partial class ShipRes: Resource
{
    private Dictionary<String, Array<BrokenPartRes>> _brokenPartGroups = new Dictionary<String, Array<BrokenPartRes>>();
    
    public ShipRes()
    {
        _brokenPartGroups = new Dictionary<String, Array<BrokenPartRes>>();
        
        // var brokenParts1 = new Array<BrokenPartRes>();
        // brokenParts1.Add(new BrokenPartRes());
        // brokenParts1.Add(new BrokenPartRes());
        // _brokenPartGroups.Add("firstMission", brokenParts1);
        //
        // var brokenParts2 = new Array<BrokenPartRes>();
        // brokenParts2.Add(new BrokenPartRes());
        // brokenParts2.Add(new BrokenPartRes());
        // _brokenPartGroups.Add("secondMission", brokenParts2);
        //
        // var brokenParts3 = new Array<BrokenPartRes>();
        // brokenParts3.Add(new BrokenPartRes());
        // brokenParts3.Add(new BrokenPartRes());
        // brokenParts3.Add(new BrokenPartRes());
        // _brokenPartGroups.Add("secondMission", brokenParts3);
    }
    
    public Array<BrokenPartRes> GetBrokenParts(String group)
    {
        return _brokenPartGroups[group];
    }

    public BrokenPartRes CreateBrokenPartResForMission(String missionKey)
    {
        if (!_brokenPartGroups.ContainsKey(missionKey))
        {
            Array<BrokenPartRes> brokenParts = new Array<BrokenPartRes>();
            _brokenPartGroups.Add(missionKey, brokenParts);
        }
        Array<BrokenPartRes> brokenPartsFromMap = _brokenPartGroups[missionKey];
        BrokenPartRes brokenPartRes = new BrokenPartRes();
        brokenPartsFromMap.Add(brokenPartRes);
        return brokenPartRes;
    }
    
}