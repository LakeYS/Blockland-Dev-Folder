function teleportPlayerToPlayer(%victim1,%victim2)
{
	%admin = %victim1.isAdmin;

	%victim1.isAdmin = 1;
	serverCmdFind(%victim1,%victim2);
	%victim1.isAdmin = %admin;
}

function serverCmdTeleportPlayerToPlayer(%client,%target1,%target2)
{
	if(!%client.isAdmin)
	{
		return;
	}
	
	%victim1 = findClientByName(%target1);
	%victim2 = findClientByName(%target1);
	
	teleportPlayerToPlayer(%client,%target1,%target2);
}

function serverCmdFetchAll(%client)
{
	if(!%client.isSuperAdmin)
	{
		return;
	}
	
	%client.fetchAll = 1;
	
	for(%i = 0; %i < ClientGroup.getCount(); %i++) 
	{ 
		%victim = clientGroup.getObject(%i); 
		serverCmdFetch(%client,%victim);
	}
	
	$client.fetchAll = 0;
}

//to do: block the client-sided fetch-all mod