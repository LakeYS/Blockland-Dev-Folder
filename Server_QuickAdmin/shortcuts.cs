function serverCmdW(%client)
{
	serverCmdWarp(%client);
}

function serverCmdFe(%client,%input)
{
	serverCmdFetch(%client,%input);
}

function serverCmdFi(%client,%input)
{
	serverCmdFind(%client,%input);
}

function serverCmdTS(%client,%input)
{
	serverCmdTimeScale(%client,%input);
}

function serverCmdTp(%client,%target1,%target2)
{
	serverCmdTeleportPlayerToPlayer(%client,%target1,%target2);
}
