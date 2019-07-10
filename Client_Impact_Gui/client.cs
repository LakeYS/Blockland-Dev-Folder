package guiProfileStuff 
{
	function GuiControlProfile::UpdateFont(%profile) 
	{
		if(%profile.edit)
		{
			%profile.edit = 0;
			return;
		}
		
		%name = %profile.getName();
		echo(%name);
		
		if(%name $= "ImpactCheckProfile" || %name $= "ImpactRadioProfile")
		{
			%profile.fontSize = %profile.fontSize/1.8;
		}
		
		Parent::UpdateFont(%profile);
	}
	
	function customGameGui::onRender(%gui)
	{
		Parent::onRender(%gui);
		
		for(%i = 1; %i <= 3; %i++) // Add-Ons, Music, Color Sets
		{
			switch(%i)
			{
				
				case 1: %box = CustomGameGui_AddOnsBox;
				case 2: %box = CustomGameGui_MusicBox;
				case 3: %box = CustomGameGui_ColorsetSwatch;
			}
			
			for(%iB = 0; %iB <= %box.getCount()-1; %iB++)
			{
				%obj = %box.getObject(%iB);

				if(%prevPos $= "")
					%prevPos = -45;
				
				%obj.resize(getWord(%obj.getPosition(),0), %prevPos+40, getWord(%obj.getExtent(),0), getWord(%obj.getExtent(),1));
				%prevPos = %prevPos+40;
			}
			
			%prevPos = "";
		}
		
		for(%i = 0; %i <= CustomGameGui_AdvancedBox.getCount()-1; %i++) // Advanced
		{
			%obj = CustomGameGui_AdvancedBox.getObject(%i);
			
			if(%prevPos $= "")
				%prevPos = -45;
			
			%class = %obj.getClassName();
			
			//if(%class $= "GuiMLTextCtrl")
			//	%obj.resize(getWord(%obj.getPosition(),0), %prevPos+40, getWord(%obj.getExtent(),0), getWord(%obj.getExtent(),1));
			//else if(%class $= "GuiTextEditCtrl")
			//{
			//	%obj.resize(getWord(%obj.getPosition(),0), %prevPos+40, getWord(%obj.getExtent(),0), getWord(%obj.getExtent(),1));
			//	%prevPos = %prevPos+40;
			//}
			//else if(%class $= "GuiSwatchCtrl")
			//{
			//	%obj.resize(getWord(%obj.getPosition(),0), %prevPos+40, getWord(%obj.getExtent(),0), getWord(%obj.getExtent(),1));
			//	%prevPos = %prevPos+40;
			//}
		}
		
		CustomGameGui_AdvancedBox.lineHeight = 10;
	}
};
deactivatepackage(guiProfileStuff);
activatepackage(guiProfileStuff);