package JumpControl
{
	function Player::SmoothUp(%obj,%this)
	{
		for(%i = 0; %i < %this+1; %i++) 
		{ 
			schedule(%i*$jumpSmoothing,0,eval,%obj @ ".addVelocity(\"0 0 -" @ %i @ "\");" @ %obj @ ".addVelocity(\"0 0 " @ %i+1 @ "\");"); 
		}
	}
	
	//ToggleJC
	function serverCmdToggleJC(%client)
	{
		if(%client.isAdmin)
		{
			if($JumpControl::Enabled)
			{
				$JumpControl::Enabled = 0;
				messageAll('MsgJumpControl',%client.name @ " disabled global jump control");
				return;
			}			
			
			if(!$JumpControl::Enabled)
			{
				$JumpControl::Enabled = 1;
				messageAll('MsgJumpControl',%client.name @ " enabled global jump controls");
			}
		}
	}
	
	//SetJCVelocity
	function serverCmdSetJCVelocity(%client,%val1,%val2)
	{
		if(%client.isAdmin)
		{
			if(!%val1 || !%val2)
			{
				%output1 = 8;
				%output2 = 12;
			}
			else
			{
				%output1 = %val1;
				%output2 = %val2;
			}
			
			$jumpVelocity1 = %output1;
			$jumpVelocity2 = %output2;
			
			messageAll('MsgJumpControl',%client.name @ " changed the jump velocity to (" @ %output1 @ "," @ %output2 @ ")");
		}
	}
	
	//SetJCTimeout
	function serverCmdSetJCTimeout(%client,%val1,%val2)
	{
		if(%client.isAdmin)
		{
			if(!%val1 || !%val2)
			{
				%output1 = 500;
				%output2 = 750;
			}
			else
			{
				%output1 = %val1;
				%output2 = %val2;
			}
			
			$jumpTimeout1 = %output1;
			$jumpTimeout2 = %output2;
			
			messageAll('MsgJumpControl',%client.name @ " changed the jump timeout to (" @ %output1 @ "," @ %output2 @ ")");
		}
	}
	
	//SetJCSmoothing
	function serverCmdSetJCSmoothing(%client,%this)
	{
		if(%client.isAdmin)
		{
			if(!%this || %this > 50 || %this < 0)
			{
				%output = 1;
			}
			else
			{
				%output = %this;
			}
			
			$jumpSmoothing = %output;
			
			messageAll('MsgJumpControl',%client.name @ " changed the jump smoothing to " @ %output);
		}
	}
	
	//physics code
	function Armor::onTrigger(%data,%obj,%slot,%val) 
	{
		Parent::onTrigger(%data,%obj,%slot,%val); 
		if(%slot == 2 && %val) 
		{
			if($jumpAirAnim)
			{
				%obj.playThread(0,jump);
			}
			//talk("jump " @ %obj);
			
			//if one or more of the following conditions are met, the player will have 'ocean floor' physics applied to them:
			//global physics are enabled ($JumpControl::Enabled)
			//obj.isJumpControlPlayer is set to "true" on the player object
			if($JumpControl::Enabled || %obj.isJumpControlPlayer)
			{
				if(!%obj.isJumpTimeout)
				{
					%jumpVelocity = getRandom($jumpVelocity1,$jumpVelocity2);
					%obj.smoothUp(%jumpVelocity);
					//talk("addvelocity " @ %JumpVelocity @ " " @ %obj);
				}
				
				cancel(%obj.timeout);
				%obj.isJumpTimeout = 1;
				
				%timeout = getRandom($jumpTimeout1,$jumpTimeout2);
				//talk("timeout " @ %timeout @ " " @ %obj);
				//echo("obj.isJumpControlPlayer: " @ %obj.isJumpControlPlayer);
				%obj.timeout = schedule(%timeout,0,eval,%obj @ ".isJumpTimeout = 0; ");
			}
		} 
	}
};
activatePackage(JumpControl);