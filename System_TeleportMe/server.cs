//TeleportMe (Server end) v1

//test the prompts
//add the teleporting stuff (and the prompt)
//make a gui that looks similar to the trust invite dialog
//allow users to ignore others, similar to trust invites
//allow non-admins to find fully trusted users without permission
//add spam protection similar to the trust system

//test the handshake stuff
//TEST EVERYTHING
//TEST EVERYTHING

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

exec("./teleportPlayerToPlayer.cs");

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function serverCmdTeleportMeHandshake(%client)
{
	//this tells the server that you have the client script to teleportMe (for the fancy prompt gui)
	%client.hasTeleportMe = 1;
}

package TeleportMe
{
	//find
	function serverCmdFind(%client, %this)
	{
		if(%client.isAdmin)
		{
			Parent::ServerCmdFind(%client, %this); //they're admin. continue /find like normal.
			return;
		}
		
		if(%client.hasTeleportMe)
		{
			//fancy prompt stuff here
		}
		else
		{
			commandToClient(%this, 'messageBoxYesNo', "Confirm", %client.name @ " is requesting to FIND you.", 'TeleAccept'); //edit this to match the trust invite box as closely as possible
		}
	}
	
	//FETCH
	function serverCmdFetch(%client, %this)
	{
		if(%client.isAdmin)
		{
			Parent::ServerCmdFetch(%client, %this); //they're admin. continue /fetch like normal.
			return;
		}
		
		if(%client.hasTeleportMe)
		{
			//fancy prompt stuff here
		}
		else
		{
			commandToClient(%this,'messageBoxYesNo',"Confirm",%client.name @ " is requesting to FETCH you.",'TeleAccept'); //edit this to match the trust invite box as closely as possible
		}
	}
	
	function serverCmdTeleAccept(%client)
	{
		
	}
	
	//handshake
	function GameConnection::AutoAdminCheck(%this)
	{
		commandToClient('TeleportMeHandshake');
		Parent::AutoAdminCheck(%this);
	}
};

//i want to make this mirror the trust system but that might be somewhat difficult