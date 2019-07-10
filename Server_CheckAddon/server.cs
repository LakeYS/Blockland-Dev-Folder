//LakeYS

package CheckAddOn
{
	function serverCmdCheckAddOn(%client,%str)
	{
		%this = strUpr(%str);
		
		if(strstr(%this,"/") != -1 || strstr(%this,":") != -1 || strstr(%this,"*") != -1 || strstr(%this,"?") != -1 || strstr(%this,"\"") != -1 || strstr(%this,"<") != -1 || strstr(%this,">") != -1 || strstr(%this,"|") != -1 || strstr(%this,".") != -1 || strstr(%this,",") != -1 || strstr(%this,"(") != -1 || strstr(%this,")") != -1 || strstr(%this," ") != -1)
		{
			messageClient(%client,'MsgCheckAddOn',"Invalid filename. Please only include the filename, NOT the path. Also, do not include the \".zip\" file extension.");
			return;
		}
		
		if(strlen(%this) > 75)
		{
			messageClient(%client,'MsgCheckAddOn',"The filename you specified is too long.");
			return;
		}
		
		echo(%client.name @ " is checking for the add-on: " @ %this);
		if(isFile("Add-Ons/" @ %this @ ".zip"))
		{
			%space = strreplace(%this,"_"," ");
			%prefix = getWord(%space,0);
			%suffix = getWord(%space,1);
			%link = "dl.dropboxusercontent.com/u/70373565/The%20Great%20Add-On%20Dump/" @ %prefix @ "_/" @ %this @ ".zip";
			
			messageClient(%client,'MsgCheckAddOn',"The add-on \"" @ %this @ "\" exists! <a:" @ %link @ ">Click here to download it.</a> (The file name will be in all caps. Sorry about that.)");
		}
		else
		{
			messageClient(%client,'MsgCheckAddOn',"The file \"" @ %this @ "\".zip could not be found.");
		}
	}
};