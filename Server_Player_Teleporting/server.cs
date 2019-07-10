package PlayerTele
{
	function serverCmdTP(%client,%target1,%target2)
	{
		//this should be harmless--pm me if there's a problem
		if(!%client.isAdmin)
		{
			return;
		}
		
		%victim1 = findClientByName(%target1);
		%victim2 = findClientByName(%target2);
		
		%admin = %victim1.isAdmin; 

		%victim1.isAdmin = 1;
		serverCmdFind(%victim1,%victim2.name);
		%victim1.isAdmin = %admin;
	}
};