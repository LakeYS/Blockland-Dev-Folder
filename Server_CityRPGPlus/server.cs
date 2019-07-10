// This is a script that enhances the CityRPG game-mode.
// Developed by Lake Y.

// This has been tested with:
// File Name						CRC				Latest Commit Tested
// Gamemode_CityRPG.zip				1504460900		b67413a
// Gamemode_TysCityRPG.zip			467599692		b67413a

package CityRPGPlus
{
	function CityRPGPoliceBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
	{
		//// HELP SECTION ////

		//if(%triggerStatus $= "" && strReplace(%text, "?", "") !$= %text)
		//	messageClient(%client,'',"\c6This is the police .");
		//else // Execute the rest of the function...
		//{
			//// DEMERIT COST CHANGER////
			// We want to make the cost of demerits scale with the player's education level.

			// We're going to use a little trick to avoid overwriting the function.
			%defaultCost = $CityRPG::pref::demerits::demeritCost;
			%education = CityRPGData.getData(%client.bl_id).valueEducation;

			// demeritCost = demerits*(multiplier+educationLevel*1.2)
			$CityRPG::pref::demerits::demeritCost = %defaultCost+%education; // We'll temporarily change the pref to our desired value...
			Parent::parseData(%this, %brick, %client, %triggerStatus, %text); // Then call the function...
			$CityRPG::pref::demerits::demeritCost = %defaultCost; // Then reset the pref to its original value.

			//if(%triggerStatus) // Help section message
			//	messageClient(%client,'',"\c3Type '?' in chat for help.");
		//}
	}

	//// DISABLE SCALE EFFECT ////
	// This disables the hunger scaling effect.
	function player::setScale(%this, %scale, %client)
	{
		%valueHunger = CityRPGData.getData(%this.client.bl_id).valueHunger;

		CityRPGData.getData(%this.client.bl_id).valueHunger = 4; // Temporarily set hunger to 4. This tricks the game-mode into setting our scale to the normal "1 1 1"
		Parent::setScale(%this, %scale, %client);
		CityRPGData.getData(%this.client.bl_id).valueHunger = %valueHunger; // Reset the player's hunger to its original value.
	}

	//// DEFINE GETSALARY ////
	function GameConnection::GetSalary(%client)
	{
		return 0;
	}
};
deactivatePackage(CityRPGPlus);
activatePackage(CityRPGPlus);
