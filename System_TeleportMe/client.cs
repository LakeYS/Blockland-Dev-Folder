//TeleportMe (Client end) v1

//add a gui
//(IMPORTANT) make sure that clients without teleportMe can still use all of the functions properly
//make sure that the button doesn't overwrite the client-sided mute button
//add the client end of the teleportMe handshake

//TEST THE HANDSHAKE FUNCTIONALITY

function clientCmdTeleportMeHandshake()
{
	commandToServer('TeleportMeHandshake'); //we have teleportMe, reply back to the server
}