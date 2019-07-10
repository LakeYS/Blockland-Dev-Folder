package AntiFetchAll
{
	//This is to block the client-sided 'fetch all' add-on.
	//If an admin tries to fetch someone, there will be a delay. (32ms by default)
	//If they try to fetch someone during the delay time, it will be 'blocked' (aka nothing will happen)
	//This is probably the best workaround you'll get without a noticeable effect when using the fetch command. 
	//If you really don't care, you can change $fetchCheckDelay to something higher.
	//Note: 32ms is very short. Manually fetching someone repeatedly will NOT trigger this. You won't notice the delay, either.
	//Note2: Yes, I know that idiots can easily bypass this check by adding a delay for fetchAll. If it matters that much, raising $fetchCheckDelay is an option.
	function serverCmdFetch(%client,%target)
	{
		//talk(%client @ " FETCH " @ %target);
		
		if($DisableFetchCheck)
		{
			//talk("DEAD END; fetch check is disabled. finished without checking");
			Parent::ServerCmdFetch(%client,%target);
			return;
		}
		
		if(!%client.isAdmin || !%target)
		{
			//talk("DEAD END; no admin");
			return;
		}
		
		if(%client.canFetchAll)
		{
			//talk("client has permission, ignoring...");
			Parent::ServerCmdFetch(%client,%target);
			return;
		}
		
		//this shouldn't be needed. why did i add this in?
		//%victim = findClientByName(%target);
		//if(!%victim)
		//{
			//talk("DEAD END; nonexistent target");
		//	return;
		//}
		
		if(%client.fetchAllCmd)
		{
			//talk("rapid fetching, extend schedule...");
			cancel(%client.fetchSchedule);
			%client.fetchSchedule = schedule(1000,0,eval,%client @ ".fetchAll = 0;");
			return;
		}
		
		if(%client.fetchReady)
		{
			%client.fetchReady = 0;
			Parent::ServerCmdFetch(%client,%target);
			//talk("DEAD END; finished");
			return;
		}
		
		if(%client.noFetch)
		{
			%client.noFetch = 0;
			%client.fetchAll = 1;
			cancel(%client.fetchSchedule);
			%client.fetchSchedule = schedule(1000,0,eval,%client @ ".fetchAll = 0;");
			//talk("attempting to fetch twice in 32ms!");
			return;
		}
		
		if(!%client.noFetch)
		{
			%client.noFetch = 1;
			%client.fetchSchedule = schedule($FetchCheckDelay,0,eval,%client @ ".noFetch = 0; " @ %client @ ".fetchReady = 1; serverCmdFetch(" @ %client @ "," @ %target @ ");");
			//talk("Client will be fetched in " @ $FetchCheckDelay @ "ms...");
			return;
		}
	}
};
activatePackage("AntiFetchAll");
$FetchCheckDelay = 32;