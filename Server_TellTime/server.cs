package TellTime
{
	function serverCmdMissionStartPhase3Ack(%client, %bool)
	{
		messageClient(%client,'MsgTellTime',"\c2Hey there! It is currently " @ getDateTime());
		Parent::ServerCmdMissionStartPhase3Ack(%client, %bool);
	}
};
activatePackage("TellTime");