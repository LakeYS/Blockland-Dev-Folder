//people can use client-sided crap to rapidly activate things, which is annoying and can screw up events
//this stops clients from rapidly clicking. otherwise, it shouldn't affect gameplay
//weapons are not affected by this script at all

//note: this won't completely break an auto-clicker but it will slow them down so it isn't as fast (the limit can be modified below, i might make an rtb pref for it)

package AntiRapidActivate
{
	function Player::ActivateStuff(%this)
	{
		if(%this.activatePause)
		{
			return;
		}
		
		%this.activatePause = 1;
		schedule(128,0,eval,%this @ ".activatePause = 0;");
		
		Parent::ActivateStuff(%this);
	}
};
activatePackage("AntiRapidActivate");