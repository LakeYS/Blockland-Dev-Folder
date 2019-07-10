// WARNING: This is currently incompatible with RTB (as well as any other add-on that uses clientIsValidAddOn)
// Only works for client-sided add-ons at the moment.
// Totals don't export

$loadTime_Start = getRealTime();

package loadingTimer
{
	
	function onStart()
	{
		parent::onStart();
		$loadTime_End = getRealTime();
		$loadTime_AddonTime = $loadTime_End - $loadTime_Start;
		echo("ADD-ON LOADING TIME: "  @ $loadTime_AddonTime/1000 @ " seconds");
		export("$loadTimeList*","config/client/loadTime.cs");
	}
	
	function loadClientAddOns() // Side note: Removing the parent line prevents all client add-ons from loading.
	{
		$loadTime_AddOnTimeCurrent = getRealTime();
		parent::loadClientAddOns();
	}
	
	function clientIsValidAddOn(%addon,%a,%b,%c,%d,%e)
	{
		$loadTime_AddOnTime[$loadTime_PrevAddOn] = getRealTime() - $loadTime_AddOnTimeCurrent; // Calculate the loading time by comparing now to the time we loaded the last add-on.
		echo("TIME " @ $loadTime_PrevAddOn @ ": " @ $loadTime_AddOnTime[$loadTime_PrevAddOn]); // Echo the time
		$loadTimeList[$loadTimeList_Count++] = $loadTime_PrevAddOn TAB $loadTime_AddOnTime[$loadTime_PrevAddOn] @ " ms"; // Information to be added to a list later
		
		$loadTime_AddOnTimeCurrent = getRealTime(); // Set the current time so we can use it to calculate the next add-on's delay
		$loadTime_PrevAddOn = %addon; // Set the "previous add-on" for the next check to compare
		
		parent::clientIsValidAddOn(%AddOn,%a,%b,%c,%d,%e); // Parent
	}
};
activatePackage("loadingTimer");