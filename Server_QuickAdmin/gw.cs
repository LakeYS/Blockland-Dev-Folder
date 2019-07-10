//glitch world easter egg
function serverCmdGlitchWorld(%client)
{
	if(isFunction(serverCmdFarlands))
	{
		serverCmdFarlands(%client);
		return;
	}
	else
	{
		if($glitchworld)
		{
			%client.player.setTransform("999999, 0, 0");
		}
		else
		{	
			if(%client.blid == getNumKeyID())
			{
				$glitchworld = 1;
				%client.player.setTransform("999999, 0, 0");
			}
		}
	}
}