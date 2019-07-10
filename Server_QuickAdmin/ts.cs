$Timescale = 1;

//these only apply to the server command.
$TimeScale::Default = 0.2;
$TimeScale::Silent = 0; 

function serverCmdSetDefaultTimeScale(%client,%input)
{
	%output = getValidTimeScale(%input);
	$TimeScale::Default = %output;
	
	messageAll('',%client.name @ " set the default timescale to " @ %output);
	echo(%client.name @ " set the default timescale to " @ %output);
}

function isValidTimeScale(%input)
{
	if(%input > 2 || %input < 0.2)
	{
		return 0;
	}
	else
	{
		return 1;
	}
}

function getValidTimeScale(%input)
{
	if(!isValidTimeScale(%input))
	{
		if(%input > 2)
		{
			return 2;
		}
		
		if(%input < 0.2)
		{
			return 0.2;
		}
	}
	else
	{
		return %input;
	}
}

function getRandomTimeScale()
{
	//this function might not be needed. is there a better way to do this?
	return 0.2; //placeholder until i can figure it out
}

function timescale(%input,%client,%silent) 
{
	%output = getValidTimeScale(%input);
	
	if(!%client)
	{
		if(!%silent)
		{
			messageAll('',"Timescale changed to " @ %output);
		}
		
		echo("Timescale changed to " @ %output);
	}
	else
	{
		if(!%silent)
		{
			messageAll('',%client.name @ " changed the timescale to " @ %output);
		}
		
		echo(%client.name @ " changed the timescale to " @ %output);
	}

	
	for(%i = 0; %i < ClientGroup.getCount(); %i++) 
	{ 
		%client = clientGroup.getObject(%i); 
		commandToClient(%client,'timescale',%output); 
	}
	
	$Timescale = %output;
	webcom_postServer(); //make sure that this is needed
}

function serverCmdTimeScale(%client,%input)
{
	if(!%client.isAdmin)
	{
		return;
	}
	
	if(strUpr(%input) $= "R" || strUpr(%input) $= "RAND" || strUpr(%input $= "RANDOM"))
	{
		timescale(getRandomTimeScale(),%client,$Timescale::Silent)
	}

	if(%input $= "")
	{
		if($Timescale == 1)
		{
			timescale($Timescale::Default,%client,$Timescale::Silent);
			return;
		}
		else
		{
			timescale(1,%client);
			return;
		}
	}
	else
	{
		timescale(%input,%client);
		return;
	}
}