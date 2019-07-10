$CakeBot_DefList = "config/client/CakeBot/Definitions.cs";
if(isFile($CakeBot_DefList))
{
	echo("Loading definition list");
	exec($CakeBot_DefList);
	echo("Successfully loaded " @ $CakeBot_TotalDefinitions-1 @ " definitions");
	echo("Last modified: " @ $DefList_LastModified);
	
	copyTextFile($CakeBot_DefList,"config/client/cakebot/archive/definitions_autoBackup.cs");
}
else
{
	echo("No definitions file!");
	$CakeBot_TotalDefinitions = 0;
}

function CakeBot_RegisterDefinition(%name,%description)
{
	echo("Registering definition...");
	
	if(strLen(%name) < 4)
	{
		echo("ERROR: Word must have < 4 characters!");
		return;
	}
	
	echo("$DefinitionName[" @ $CakeBot_TotalDefinitions @ "] = " @ strUpr(%name));
	echo("$DefinitionDesc[" @ $CakeBot_TotalDefinitions @ "] = " @ %description);
	$DefinitionName[$CakeBot_TotalDefinitions] = strUpr(%name);
	$DefinitionDesc[$CakeBot_TotalDefinitions] = %description;

	$CakeBot_TotalDefinitions++;
	$DefList_LastModified = getDateTime();

	echo("Exporting definitions");
	export("$DefList_LastModified",$CakeBot_DefList,0);
	export("$CakeBot_TotalDefinitions",$CakeBot_DefList,1); 
	export("$DefinitionName*",$CakeBot_DefList,1); 
	export("$DefinitionDesc*",$CakeBot_DefList,1); 
}

function CakeBot_GetDefinition(%name)
{
	//echo("Searching for definition...");
	//echo("Total definitions: " @ $CakeBot_TotalDefinitions);
	
	for(%i = 0; %i < $CakeBot_TotalDefinitions; %i++)
	{
		//echo(%i);
		//echo(strUpr(%name) @ " = " @ $DefinitionName[%i]);
		if(strUpr(%name) $= $DefinitionName[%i])
		{
			//echo("MATCH");
			return $DefinitionDesc[%i];
		}
	}
}