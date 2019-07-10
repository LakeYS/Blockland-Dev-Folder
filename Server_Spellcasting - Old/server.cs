$Spellcasting::Debug = 1;
$Spellcasting::OldVersion = $Spellcasting::Version;

$Spellcasting::Version = "1.4.0";

if($Spellcasting::Debug)
{
	talk("Server_Spellcasting " @ $Spellcasting::Version @ " INIT");
	echo("Server_Spellcasting " @ $Spellcasting::Version @ " INIT");
	
	if($Spellcasting::HasRunOnce)
	{
		talk("The add-on has been reloaded from v" @ $Spellcasting::OldVersion @ " to v" @ $Spellcasting::Version);
		echo("The add-on has been reloaded from v" @ $Spellcasting::OldVersion @ " to v" @ $Spellcasting::Version);
		cancel($spelltick);
		deactivatePackage("serverspellcasting");
	}
}

exec("./Server_Spellcasting.cs");