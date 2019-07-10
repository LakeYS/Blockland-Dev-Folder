//to do: handleYourSpawn checker for server resets
//to do: auto-reconnect?

function CakeMine_Cancel()
{
	cancel($CakeMine_SchedMainLoop);
	cancel($CakeMine_SchedFixLoop);
	cancel($CakeMine_SchedUpgradeLoop);
	cancel($CakeMine_MoveSched);
	cancel($CakeMine_TempSched);
	
	$MouseLoop = 0;
	
	moveForward(0);
	moveRight(0);
	moveBackward(0);
	moveLeft(0);
	
	mouseFire(0);
	useTools(0);
	crouch(0);
	jump(0);
	jet(0);
	
	$CakeMine_AutoRespawnFixEnabled = 0;
	$CakeMine_IsMiningDown = 0;
	
	CakeSay("Mining bot stopped");
}

function CakeMine_Move()
{
	%this = getRandom(1,2);
	
	if(%this == 1)
	{
		moveLeft(1);
		$CakeMine_TempSched = schedule(300,0,moveLeft,0);
	}
	
	if(%this == 2)
	{
		moveRight(1);
		$CakeMine_TempSched = schedule(300,0,moveRight,0);
	}
}

function CakeMine_FixLoop()
{
	$CakeMine_FixLoopCount++;
	cakeEcho("CakeMine fix loop #" @ $CakeMine_FixLoopCount);
	
	commandToServer('suicide'); //this will trigger the suicide check
	
	$CakeMine_SchedFixLoop = schedule(2700000,0,"CakeMine_FixLoop");
}

function CakeMine_Fix()
{
	cakeEcho("CakeMine FIX");
	
	cancel($CakeMine_SchedFixB);
	
	moveForward(0);
	moveRight(0);
	moveBackward(0);
	moveLeft(0);
	
	mouseFire(0);
	useTools(0);
	crouch(0);
	jump(0);
	jet(0);
	
	$CakeMine_SchedFixB = schedule(12000,0,CakeMine_FixB);
}

function CakeMine_FixB()
{
	cancel($CakeMine_SchedMainLoop);
	
	crouch(1);
	moveForward(1);
	useTools(1);
	
	CakeMine_Move();
	$CakeMine_TempSched = schedule(750,0,CakeMine_MainLoop); 
}

function CakeMine_MainLoop()
{
	$CakeMine_AutoRespawnFixEnabled = 1;
	crouch(1);
	moveForward(1);
	$mouseLoop = 0;
	toggleMouseLoop();
	
	%this = getRandom(0,6);
	$CakeMine_LoopCount++;
	cakeEcho("CakeMine switch loop #" @ $CakeMine_LoopCount @ ": rolled " @ %this);
	
	if(%this == 0) //do nothing at all
	{
		cakeEcho("rolled 0, do nothing...");
	}
	
	if(%this == 1) //turn left/right
	{ 
		if(!$CakeMine_IsMiningDirectlyDown)
		{
			moveBackward(0); crouch(1); moveForward(1);
			yaw(410);
		}
		else
		{
			cakeEcho("Can't turn left/right while mining directly down");
		}
	} 
	
	if(%this == 2) //turn left/right
	{
		if(!$CakeMine_IsMiningDirectlyDown)
		{
			moveBackward(0); crouch(1); moveForward(1);
			yaw(-410);
		}
		else
		{
			cakeEcho("Can't turn left/right while mining directly down");
		}
	}	
	
	if(%this == 3) //turn left/right (extra chance)
	{
		if(!$CakeMine_IsMiningDirectlyDown)
		{
			moveBackward(0); crouch(1); moveForward(1);
			yaw(410);
		}
		else
		{
			cakeEcho("Can't turn left/right while mining directly down");
		}
	} 
	
	if(%this == 4) //turn left/right (extra chance)
	{
		if(!$CakeMine_IsMiningDirectlyDown)
		{
			moveBackward(0); crouch(1); moveForward(1);
			yaw(-410);
		}
		else
		{
			cakeEcho("Can't turn left/right while mining directly down");
		}
	}
	
	if(%this == 5) //mine down
	{
		if(!$CakeMine_IsMiningDown)
		{
			$CakeMine_IsMiningDown = 1;
			
			moveBackward(0); crouch(1); moveForward(1); 
			pitch(250); 
			
			$CakeMine_MoveSched = schedule(getRandom(15000,60000),0,eval,"pitch(-250); $CakeMine_IsMiningDown = 0;");
		}
		else
		{
			cakeEcho("Already mining down");
		}
	}
	
	if(%this == 6) //mine directly down
	{
		if(!$CakeMine_IsMiningDown)
		{
			$CakeMine_IsMiningDown = 1;
			$CakeMine_IsMiningDirectlyDown = 1;
			
			moveBackward(0); crouch(1); moveForward(1); 
			pitch(400); 
			
			$CakeMine_MoveSched = schedule(getRandom(15000,60000),0,eval,"pitch(-400); $CakeMine_IsMiningDown = 0; $CakeMine_IsMiningDirectlyDown = 0;");
		}
		else
		{
			cakeEcho("Already mining down");
		}
	}
	
	$CakeMine_SchedMainLoop = schedule(30000,0,"CakeMine_MainLoop");
}

function CakeMine_UpgradeLoop()
{
	%this = getRandom(0,8);
	$CakeMine_UpgradeLoopCount++;
	cakeEcho("CakeMine upgrade loop #" @ $CakeMine_UpgradeLoopCount @ ": rolled " @ %this);
	
	if(%this == 0) //do nothing
	{
		cakeEcho("rolled 0, do nothing...");
	}

	if(%this == 1) //upgradeall
	{
		commandToServer('upgradeall');
	}
	
	if(%this == 2) //upgradeall (extra chance)
	{
		commandToServer('upgradeall');
	}	
	
	if(%this == 3) //upgradeall (extra chance)
	{
		commandToServer('upgradeall');
	}	
	
	if(%this == 4) //upgradeall (extra chance)
	{
		commandToServer('upgradeall');
	}	
	
	if(%this == 5) //upgradeall (extra chance)
	{
		commandToServer('upgradeall');
	}
	
	if(%this == 6) //upgradeall (extra chance)
	{
		commandToServer('upgradeall');
	}
	
	if(%this == 7) //drill
	{
		cakeEcho("rolled 7, drill");
		commandToServer('drill',999);
	}
	
	if(%this == 8) //buy heatsuit
	{
		cakeEcho("rolled 8, buy heatsuit");
		commandToServer('buy',"heatsuit");
	}
	
	$CakeMine_SchedUpgradeLoop = schedule(300000,0,CakeMine_UpgradeLoop);
}

function CakeMine_Init()
{
	CakeSay("Mining bot started");
	toggleMouseLoop();
	CakeMine_Fix();
	
	//$CakeMine_SchedFixLoop = schedule(2700000,0,"CakeMine_FixLoop");
	$CakeMine_SchedFixLoop = schedule(30000,0,"CakeMine_FixLoop");
}