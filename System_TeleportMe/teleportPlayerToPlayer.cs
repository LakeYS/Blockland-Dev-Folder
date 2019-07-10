//ripped from quickTele

function teleportPlayerToPlayer(%victim1,%victim2)
{
	//this is a bit of a hack but i'll explain it so people don't freak out
	//we're going to utilize the default '/find' servercmd instead of trying to mess around with setTransform stuff
	//we give victim1 admin and use their client to /find victim2
	//then, we IMMEDIATELY reset victim1's admin status
	//this should be harmless but if it causes any problems i will fix it
	
	%admin = %victim1.isAdmin; 

	%victim1.isAdmin = 1;
	serverCmdFind(%victim1,%victim2.name);
	%victim1.isAdmin = %admin;
}