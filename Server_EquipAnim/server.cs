package equipAnim 
{ 
	function ServerCmdUseTool(%client,%a,%b) 
	{ 
		%client.player.playThread(0,shiftAway); Parent::ServerCmdUseTool(%client,%a,%b); 
	}
		
	function ServerCmdUnUseTool(%client,%a,%b) 
	{ 
		%client.player.playThread(0,shiftTo); Parent::ServerCmdUnUseTool(%client,%a,%b);
	}
};
activatePackage("equipAnim");