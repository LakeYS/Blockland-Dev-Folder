function serverCmdFindBrick(%client,%blid,%colorID,%colorFxID)
{
	if(!%client.isSuperAdmin)
			return;
	
	%group = "BrickGroup_" @ %blid;
	if(!isObject(%group))
		return;
	
	if(!%client.brickSearch)
		%client.brickSearch = 0;
		
	if(%colorID $= "" || %colorFxID $= "")
	{
		if(!%client.colorID || !%client.colorFxID)
			return;
		
		%colorID = %client.colorID;
		%colorFxID = %client.colorFxID;
	}
	else //if ids are specified, the counter must be reset
	{
		%client.brickSearch = 0;
		%client.colorID = %colorID;
		%client.colorFxID = %colorFxID;
	}

	//	if(%client.brickSearch > %group.getCount())
	//		return;
	//necessary?

	for(%i=%client.bricksearch;%i<%group.getcount();%i++)
	{
		%brick = %group.getobject(%i);
		
		//we're only interested in bricks...
		if(!(%brick.getType() & $TypeMasks::FxBrickAlwaysObjectType))
			continue;
		
		//talk("check" SPC %brick.colorID SPC %brick.colorFxID);
		//talk("target" SPC %colorID SPC %colorFxID);
		if(%brick.colorID == %colorID && %brick.colorFxID == %colorFxID)
		{
			%client.player.setTransform(%brick.position);
			%client.bricksearch = %i+1;
			//talk("search" SPC %client.bricksearch);
			break;
		}
	}
}