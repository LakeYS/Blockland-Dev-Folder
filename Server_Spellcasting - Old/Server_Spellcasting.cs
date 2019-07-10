////////////////////Lake's Spellcasting Mod 1.4.0; 8/5/2014)////////////////////
//This add-on uses Semnatic Versioning 2.0.0: http://semver.org/spec/v2.0.0.html
//MAJOR version when you make incompatible API changes,
//MINOR version when you add functionality in a backwards-compatible manner, and
//PATCH version when you make backwards-compatible bug fixes.

//Note: This add-on is still in early development. Expect updates that will completely BREAK stuff. Also expect bugs, including ones that may CRASH YOUR SERVER.
//Note 2: This currently relies on player persistence. So far it seems to work good but I haven't done extensive testing to make sure nothing goes wrong. I'll get around to that eventually.

//TO DO RIGHT NOW: MOVE OVER TO OUR OWN PERSISTENCE SYSTEM. PLAYER PERSISTENCE DOESN'T WORK WELL HERE.
//TO DO: CUSTOM SPELLS
//TO DO: HASSPAWNEDONCE STUFF

//persistence vars
if(!$Spellcasting::HasRunOnce)
{
	registerPersistenceVar("spellcasting_spellPoints",false,""); //total amount of spell points
	registerPersistenceVar("spellcasting_isSpellBanned",false,""); //is the player banned from spellcasting
	registerPersistenceVar("spellcasting_hasreceivedhelp",false,""); //did they type /spellhelp
}


//finally, we need to actually give points to people
function giveSpellPoints(%newClient)
{
	if(isObject(%newClient.player))
	{
		%pointAmount = getRandom(5,15);
		%newClient.spellcasting_spellPoints += %pointAmount;
		
		messageClient(%newClient,'',"\c6You've been given \c3" @ %pointAmount @ "\c6 spell points for playing on the server!");
	}
}

function startSpellCooldown(%client)
{
	%client.spellcasting_isCoolingDown = 1;
	schedule(5000,0,endSpellCooldown,%client);
}

function endSpellCooldown(%client)
{
	%client.spellcasting_isCoolingDown = 0;
	messageClient(%client,'',"\c6Your spell powers have cooled down. You can cast another spell now!");
}

package serverspellcasting
{
	//functions - spellTick1
	function spellTick1()
	{
		echo("Spellcasting (CONSOLE): Distributing spell points to players...");
		if($Spellcasting::Debug) { talk("Distributing spell points to players..."); }
		
		for(%i=0;%i<ClientGroup.getCount();%i++) 
		{
			giveSpellPoints(ClientGroup.getObject(%i));
		}
		
		$spellTick = schedule(300000,0,spellTick1); //300000
	}
	
	//server commands - /cast
	//for the current state of testing, we're going to use three hard-coded spells. once everything is working better i'll switch over to a much cleaner custom spell system.
	function servercmdCast(%client,%spellname,%spelltarget)
	{
		echo("Spellcasting (" @ %client.name @ "): /cast " @ %spellname @ " " @ %spelltarget);
		
		%spell = strUpr(%spellname);
		%target = findClientByName(%spelltarget);
		
		if(%client.spellcasting_isSpellBanned) //before we do anything, make sure they aren't banned from spellcasting.
		{
			messageClient(%client,'',"You are banned from spellcasting. For more info, type /spellhelp banned");
			return;
		}

		if(%client.spellcasting_isCoolingDown) //if they're still cooling down, don't even bother to do any other checks.
		{
			messageClient(%client,'',"Your powers are currently cooling down. Be patient!");
			return;
		}
		
		startSpellCooldown(%client); //start the cooldown IMMEDIATELY; this is to prevent the command from being spammed.
		
		//are the arguments valid?
		if(strlen(%spellname) == 0 || strlen(%spelltarget) == 0)
		{
			messageClient(%client,'',"Wrong number of arguments! Usage: /cast spell target");
		}
		else
		{
			//is it a real spell?
			%client.hasValidSpellName = 0;
			
			switch$(%spell)
			{
				case "TORCH":
				%client.hasValidSpellName = 1;
					
				case "KILL":
				%client.hasValidSpellName = 1;
				
				case "LAUNCH":
				%client.hasValidSpellName = 1;
			}
			
			//does the target exist?
			if(isObject(%target))
			{
				%client.hasValidSpellTarget = 1;
			}
			else
			{
				%client.hasValidSpellTarget = 0;
			}
			
			//does the target have a player object?
			if(isObject(%target.player))
			{
				%client.hasAliveSpellTarget = 1;
			}
			else
			{
				%client.hasAliveSpellTarget = 0;
			}
			
			//get the required spell, along with the necesary information (spell id, points needed to purchase, etc.)
			//can this check be merged with the above one for efficiency? i'm not going to bother because it isn't a huge problem. it probably isn't worth the effort.
			switch$(%spell)
			{
				case "TORCH":
				%client.spellNum = 0;
				%client.requiredPoints = 50;
				%client.hasAdminSpell = 0;
				
				case "KILL":
				%client.spellNum = 1;
				%client.requiredPoints = 0;
				%client.hasAdminSpell = 1;  //it works!
				
				case "LAUNCH":
				%client.spellNum = 2;
				%client.requiredPoints = 25;
			}
			
			//does the caster have enough points?
			if(%client.spellcasting_spellPoints >= %client.requiredPoints)
			{
				%client.hasSufficientPoints = 1;
			}
			else
			{
				%client.hasSufficientPoints = 0;
			}
			
			//admin stuff
			//if it's an admin spell, is the caster an admin?
			if(%client.hasAdminSpell == 0)
			{
				%client.hasValidAdmin = 1; //it's not an admin spell, so we'll just say that it's valid
			}
			else
			{
				//it's an admin spell, so we'll need to do some extra checks first
				if(%client.isAdmin == 1)
				{
					%client.hasValidAdmin = 1;
				}
				else
				{
					%client.hasValidAdmin = 0;
				}
				
			}
			
			//is everything OK to run the spell?
			if(%client.hasValidSpellName == 1 && %client.hasValidSpellTarget == 1 && %client.hasAliveSpellTarget == 1 && %client.hasSufficientPoints == 1 && %client.hasValidAdmin)
			{
				//all set! the user's request is completely valid
				//now, we can get around to actually executing the spell.
				
				%client.spellcasting_spellPoints -= %client.requiredPoints;
				
				if(%client.spellNum == 0) //torch
				{
					messageAll('',"\c3" @ %client.name @ "\c6 has casted a \c3Torch\c6 spell on \c3" @ %target.name @ "\c6!");
					%target.player.burnPlayer(0); 
					%target.player.addVelocity("0 0 8");
					%target.player.addHealth(getRandom(-50,-100)); //take 50-100 health from the player
				}
				
				if(%client.spellNum == 1) //kill
				{
					messageAll('',"\c3" @ %client.name @ "\c6 has casted a \c3Kill\c6 spell on \c3" @ %target.name @ "\c6!");
					%target.player.kill();
				}
				
				if(%client.spellNum == 2) //launch
				{
					messageAll('',"\c3" @ %client.name @ "\c6 has casted a \c3Launch\c6 spell on \c3" @ %target.name @ "\c6!");
					%target.player.addVelocity("0 0 25");
				}
					
			}		
			else
			{
				//instead of showing the first problem we find, we want to show all of the problems.
				//this is rarely used in blockland server commands, but i feel it's necessary here.
				
				messageClient(%client,'',"Your spell has failed!");
				if(%client.hasValidSpellName == 0)
				{
					messageClient(%client,'',"\c6The spell \"" @ %spellname @ "\" does not exist.");
				}
					
				if(%client.hasValidSpellTarget == 0)
				{
						messageClient(%client,'',"\c6I could not locate anybody named \"" @ %spelltarget @ "\". ");
				}
					
				if(%client.hasAliveSpellTarget == 0 && %client.hasValidSpellTarget == 1)
				{
						messageClient(%client,'',"\c6Your target, \"" @ %spelltarget @ "\", is already dead or still loading."); //make an alternate check for players who are still loading
				}
					
				if(%client.hasSufficientPoints == 0 && %client.hasValidSpellName == 1)
				{
					messageClient(%client,'',"\c6You don't have enough points! (Required: \c3" @ %client.requiredPoints @ "\c6; Available: \c3" @ %client.spellcasting_spellPoints @ "\c6)");
				}
				
				
				if(%client.hasValidAdmin == 0)
				{
					messageClient(%client,'',"\c6Only admins have the ability to cast that spell.");
				}
			}
		}
	}
	
	//server commands - /spellHelp
	function serverCmdSpellHelp(%client,%input)
	{
		%client.spellcasting_hasreceivedhelp = 1;
		
		switch$(strUpr(%input))
		{
			case "": //main
			messageClient(%client,'',"\c6Welcome to Lake's Spellcasting Mod!");
			messageClient(%client,'',"\c6To cast a spell, type /cast (spell) (player)");
			messageClient(%client,'',"\c6I can't tell you the names of any spells. You have to figure them out on your own!");
			messageClient(%client,'',"\c6Be careful, though. Once you cast a spell, there's no refund for the points.");
			messageClient(%client,'',"\c6Also, don't mess up! After casting, you have to wait for the cooldown even if the spell fails!");
			messageClient(%client,'',"\c6If you want to see how many points you have, type \c3/spellPoints");
			
			case "BANNED":
			messageClient(%client,'',"\c6Admins can ban you from casting spells.");
			messageClient(%client,'',"\c6When you are banned, you still collect spell points. However, you won't be able to use them.");
			messageClient(%client,'',"\c6The ban lasts forever unless an admin un-bans you!");
			
			case "TORCH":
			messageClient(%client,'',"\c6The torch spell costs \c350\c6 points. It will burn another player.");
			messageClient(%client,'',"\c6It might not kill them, but it sure will leave a mark!");
			
			case "LAUNCH":
			messageClient(%client,'',"\c6The launch spell costs \c325\c6 points. It sends them flying upwards.");
			messageClient(%client,'',"\c6I believe I can fl-OH NO I'M FALLING AHHHHHH");
		}
	}

	//server commands - spellPoints
	function serverCmdSpellPoints(%client)
	{
		messageClient(%client,'',"\c6You currently have \c3" @ %client.spellcasting_spellPoints @ "\c6 points.");
	}
};

activatePackage(serverspellcasting);
$spellTick = schedule(60000,0,spellTick1);
$Spellcasting::HasRunOnce = 1;