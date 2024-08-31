class_name ShipRes
extends Resource

var _brokenPartGroups = {};

func GetBrokenParts(group: String) -> Array:
	return _brokenPartGroups[group];

func CreateBrokenPartResForMission(missionKey: String) -> BrokenPartRes:
	if (!_brokenPartGroups.has(missionKey)):
		var brokenParts = [];
		_brokenPartGroups[missionKey] = brokenParts;
		
	var brokenPartsFromMap: Array = _brokenPartGroups[missionKey];
	var brokenPartRes = BrokenPartRes.new();
	
	
	brokenPartsFromMap.push_back(brokenPartRes);
	return brokenPartRes;
