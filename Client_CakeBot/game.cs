/////////////////////////////////////NUMBER GAME/////////////////////////////////////
$CakeBot::NumberGame::Msg:GameStart[0] = "Who's ready to guess more numbers?";
$CakeBot::NumberGame::Msg:GameStart[1] = "Let's play the number game!";
$CakeBot::NumberGame::Msg:GameStart[2] = "Anybody want to pointlessly guess numbers all day? Let's begin!";
$CakeBot::NumberGame::Msg:GameStart[3] = "It's time for another round of the number game!";

$CakeBot::NumberGame::Msg:NewGame[0] = "Start guessing.";
$CakeBot::NumberGame::Msg:NewGame[1] = "What is it?";
$CakeBot::NumberGame::Msg:NewGame[2] = "Begin.";
$CakeBot::NumberGame::Msg:NewGame[3] = "Figure it out.";
$CakeBot::NumberGame::Msg:NewGame[4] = "Start.";

$CakeBot::NumberGame::Msg:Milestone1[0] = "C'mon guys, you can figure this out!";
$CakeBot::NumberGame::Msg:Milestone1[1] = "It can't be THAT hard, can it? Keep guessing!";
$CakeBot::NumberGame::Msg:Milestone1[2] = "Are you guys gonna let a bot stump you? Guess harder!";
$CakeBot::NumberGame::Msg:Milestone1[3] = "You guys can do it, I know you can! It's just a number!";
$CakeBot::NumberGame::Msg:Milestone1[4] = "The game is still going. Don't give up yet!";
$CakeBot::NumberGame::Msg:Milestone1[5] = "You know you want to figure out what the number is. Keep going!";

$CakeBot::NumberGame::Msg:Milestone2[0] = "Still nothing? Fine, I'll give you a hint: ";
$CakeBot::NumberGame::Msg:Milestone2[1] = "This must be harder than I thought. Here's a hint: ";
$CakeBot::NumberGame::Msg:Milestone2[2] = "Am I really that good at this? I guess I'll have to give a hint. ";
$CakeBot::NumberGame::Msg:Milestone2[3] = "Hint: ";
$CakeBot::NumberGame::Msg:Milestone2[4] = "You guys must be having a lot of trouble with this. ";

$CakeBot::NumberGame::Msg:VeryClose[0] = ", you are really close.";
$CakeBot::NumberGame::Msg:VeryClose[1] = " is super close to the answer!";
$CakeBot::NumberGame::Msg:VeryClose[2] = " almost got it right.";
$CakeBot::NumberGame::Msg:VeryClose[3] = " was so close!";

function CakeBot_StartNumberGame()
{
	cancel($CakeBot::NumberGame::Schedule);
	cancel($CakeBot::WordGame::Schedule);
	
	echo("Number game starting...");
	
	cakeSay($CakeBot::NumberGame::Msg:GameStart[getRandom(0,3)]);
	
	CakeBot_NumberGameLoop();
}

function CakeBot_NumberGameLoop()
{
	echo("New round");
	
	$CakeBot::NumberGame::NumberRange = getRandom(10,100);
	echo("Number range is 1-" @ $CakeBot::NumberGame::NumberRange);
	
	$CakeBot::NumberGame::Number = getRandom(1,$CakeBot::NumberGame::NumberRange);
	echo("The number is " @ $CakeBot::NumberGame::Number);
	
	cakeSay("I'm thinking of a number between 1 and " @ $CakeBot::NumberGame::NumberRange @ ". " @ $CakeBot::NumberGame::Msg:NewGame[getRandom(0,4)]);

	$CakeBot::NumberGame::VeryCloseCheck = 0;
	$CakeBot::NumberGame::Enabled = 1;
	
	$CakeBot::NumberGame::Schedule = schedule(30000,0,CakeBot_NumberGameMilestone1);
}

function CakeBot_NumberGameMilestone1()
{
	cakeSay($CakeBot::NumberGame::Msg:Milestone1[getRandom(0,5)]);
	$CakeBot::NumberGame::Schedule = schedule(15000,0,CakeBot_NumberGameMilestone2);
}

function CakeBot_NumberGameMilestone2()
{
	
	if($CakeBot::NumberGame::Number == 42)
	{
		cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "Life, the universe, and everything.");
		$CakeBot::NumberGame::Schedule = schedule(30000,0,CakeBot_NumberGameMilestone3);
		return;
	}
	
	%hint = getRandom(0,2);
	
	if(%hint == 0)
	{
		cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "The number is GREATER THAN " @ getRandom(1,$CakeBot::NumberGame::Number-1));
		$CakeBot::NumberGame::Schedule = schedule(30000,0,CakeBot_NumberGameMilestone3);
		return;
	}
	
	if(%hint == 1)
	{
		if($CakeBot::NumberGame::Number < 100 && $CakeBot::NumberGame::Number > 9)
		{
			cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "It's a two-digit number.");
			$CakeBot::NumberGame::Schedule = schedule(15000,0,CakeBot_NumberGameMilestone3);
			return;
		}
		
		if($CakeBot::NumberGame::Number < 10)
		{
			cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "It's a small number, only a single digit!");
			$CakeBot::NumberGame::Schedule = schedule(15000,0,CakeBot_NumberGameMilestone3);
			return;
		}
		
		if($CakeBot::NumberGame::Number > 99)
		{
			cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "It's a three-digit number.");
			$CakeBot::NumberGame::Schedule = schedule(15000,0,CakeBot_NumberGameMilestone3);
			return;
		}
	}
	
	if(%hint == 2)
	{
		cakeSay($CakeBot::NumberGame::Msg:Milestone2[getRandom(0,4)] @ "The number is LESS THAN " @ getRandom($CakeBot::NumberGame::Number+1,$CakeBot::NumberGame::NumberRange));
	}
	
	$CakeBot::NumberGame::Schedule = schedule(15000,0,CakeBot_NumberGameMilestone3);
}

function CakeBot_NumberGameMilestone3()
{
	cakeSay("Time up. I win! The number was " @ $CakeBot::NumberGame::Number);
	$CakeBot::NumberGame::Enabled = 0;
	
	if(getRandom(1,8) == 1 && $CakeBot::CycleGames)
	{
		$CakeBot::NumberGame::Schedule = schedule(5000,0,CakeBot_StartWordGame);
		return;
	}
	
	$CakeBot::NumberGame::Schedule = schedule(5000,0,CakeBot_NumberGameLoop);
}

function CakeBot_NumberGameCorrect(%name)
{
	cakeSay("Correct! " @ strUpr(%name) @ " is the winner!");
	
	$CakeBot::NumberGame::TotalWins++;
	
	echo("User \"" @ %name @ "\" guessed correctly");
	echo("Total games won: " @ $CakeBot::NumberGame::TotalWins
	);
	commandToServer('donate',%name,getRandom(250000,1000000));
	$CakeBot::NumberGame::Enabled = 0;	
	
	cancel($CakeBot::NumberGame::Schedule);
	
	if(getRandom(1,16) == 1 && $CakeBot::CycleGames)
	{
		$CakeBot::NumberGame::Schedule = schedule(5000,0,CakeBot_StartWordGame);
		return;
	}
	
	$CakeBot::NumberGame::Schedule = schedule(5000,0,CakeBot_NumberGameLoop);
}

function CakeBot_NumberGameVeryClose(%name)
{
	if(!$CakeBot::NumberGame::VeryCloseCheck)
	{
		cakeSay(strUpr(%name) @ $CakeBot::NumberGame::Msg:VeryClose[getRandom(0,3)]);
		$CakeBot::NumberGame::VeryCloseCheck = 1;
		$CakeBot::NumberGame::VeryClose++;
	}
}

function CakeBot_EndNumberGame()
{
	cancel($CakeBot::NumberGame::Schedule);
	$CakeBot::NumberGame::Enabled = 0;
	//cakeSay("Game ended.");
}


/////////////////////////////////////WORD GAME/////////////////////////////////////
$CakeBot::WordGame::Msg:GameStart[0] = "Who wants to guess more words?";
$CakeBot::WordGame::Msg:GameStart[1] = "Let's play the word game!";
$CakeBot::WordGame::Msg:GameStart[2] = "Anybody want to pointlessly guess words all day? Let's begin!";
$CakeBot::WordGame::Msg:GameStart[3] = "It's time for another round of the word game!";

$CakeBot::WordGame::Msg:Milestone1[0] = "C'mon guys, you can figure this out!";
$CakeBot::WordGame::Msg:Milestone1[1] = "It can't be THAT hard, can it? Keep guessing!";
$CakeBot::WordGame::Msg:Milestone1[2] = "Are you guys gonna let a bot stump you? Guess harder!";
$CakeBot::WordGame::Msg:Milestone1[3] = "You guys can do it, I know you can! It's just a word!";
$CakeBot::WordGame::Msg:Milestone1[4] = "The game is still going. Don't give up yet!";
$CakeBot::WordGame::Msg:Milestone1[5] = "You know you want to figure out what the word is. Keep going!";

$CakeBot::WordGame::Msg:Milestone2[0] = "Still nothing? Fine, I'll give you a hint: ";
$CakeBot::WordGame::Msg:Milestone2[1] = "This must be harder than I thought. Here's a hint: ";
$CakeBot::WordGame::Msg:Milestone2[2] = "Am I really that good at this? I guess I'll have to give a hint. ";
$CakeBot::WordGame::Msg:Milestone2[3] = "Hint: ";
$CakeBot::WordGame::Msg:Milestone2[4] = "You guys must be having a lot of trouble with this. ";

function CakeBot_StartWordGame()
{
	if($CakeBot_TotalDefinitions < 5)
	{
		echo("ERROR: Total definitions must be < 5 to start the word game");
		cakeSay("The current dictionary doesn't have enough words to start the game!");
		return;
	}
	
	cancel($CakeBot::NumberGame::Schedule);
	cancel($CakeBot::WordGame::Schedule);
	
	echo("Word game starting...");
	
	cakeSay($CakeBot::WordGame::Msg:GameStart[getRandom(0,3)]);
	
	CakeBot_WordGameLoop();
}

function CakeBot_WordGameLoop()
{
	echo("New round");
	
	echo("Word range is 0-" @ $CakeBot_TotalDefinitions-1);
	$CakeBot::WordGame::WordID = getRandom(0,$CakeBot_TotalDefinitions-1);
	
	echo("Word ID is " @ $CakeBOT::WordGame::WordID);
	
	$CakeBot::WordGame::Word = $DefinitionName[$CakeBot::WordGame::WordID];
	$CakeBot::WordGame::Definition = $DefinitionDesc[$CakeBot::WordGame::WordID];
	
	echo("The definition is " @ $CakeBot::WordGame::Definition);
	echo("The word is " @ $CakeBot::WordGame::Word);

	
	cakeSay($CakeBot::WordGame::Definition);

	$CakeBot::WordGame::Enabled = 1;
	
	$CakeBot::WordGame::Schedule = schedule(30000,0,CakeBot_WordGameMilestone1);
}

function CakeBot_WordGameMilestone1()
{
	cakeSay($CakeBot::WordGame::Msg:Milestone1[getRandom(0,5)]);
	$CakeBot::WordGame::Schedule = schedule(15000,0,CakeBot_WordGameMilestone2);
}

function CakeBot_WordGameMilestone2()
{
	%hint = getRandom(0,1);
	
	if(%hint == 0)
	{
		cakeSay($CakeBot::WordGame::Msg:Milestone2[getRandom(0,4)] @ "The first letter of the word is " @ strUpr(getSubStr($CakeBot::WordGame::Word,0,1)) );
	}
	
	if(%hint == 1)
	{
		cakeSay($CakeBot::WordGame::Msg:Milestone2[getRandom(0,4)] @ "The last letter of the word is " @ strUpr(getSubStr($CakeBot::WordGame::Word,strLen($CakeBot::WordGame::Word)-1,1)) );
	}
	
	$CakeBot::WordGame::Schedule = schedule(30000,0,CakeBot_WordGameMilestone3);
}

function CakeBot_WordGameMilestone3()
{
	%hint = getRandom(0,1);
	
	if(%hint == 0)
	{
		cakeSay($CakeBot::WordGame::Msg:Milestone2[getRandom(0,4)] @ "The word starts with " @ strUpr(getSubStr($CakeBot::WordGame::Word,0,getRandom(2,3))) );
	}
	
	if(%hint == 1)
	{
		cakeSay($CakeBot::WordGame::Msg:Milestone2[getRandom(0,4)] @ "The word ends with " @ strUpr(getSubStr($CakeBot::WordGame::Word,strLen($CakeBot::WordGame::Word)-2,2)) );
	}
	
	$CakeBot::WordGame::Schedule = schedule(15000,0,CakeBot_WordGameMilestone4);
}

function CakeBot_WordGameMilestone4()
{
	cakeSay("Time up. I win! The word was " @ $CakeBot::WordGame::Word);
	$CakeBot::WordGame::Enabled = 0;
	
	if(getRandom(1,2) == 1 && $CakeBot::CycleGames)
	{
		$CakeBot::WordGame::Schedule = schedule(5000,0,CakeBot_StartNumberGame);
		return;
	}
	
	$CakeBot::WordGame::Schedule = schedule(5000,0,CakeBot_WordGameLoop);
}

function CakeBot_WordGameCorrect(%name)
{
	//users have shown much less engagement with the word game, so it doesn't cycle often and usually only lasts a few rounds
	cakeSay("Correct! " @ strUpr(%name) @ " is the winner!");
	
	$CakeBot::WordGame::TotalWins++;
	
	echo("User \"" @ %name @ "\" guessed correctly");
	echo("Total games won: " @ $CakeBot::WordGame::TotalWins
	);
	commandToServer('donate',%name,getRandom(500000,2500000));
	$CakeBot::WordGame::Enabled = 0;
	cancel($CakeBot::WordGame::Schedule);
	
	if(getRandom(1,4) == 1 && $CakeBot::CycleGames)
	{
		$CakeBot::WordGame::Schedule = schedule(5000,0,CakeBot_StartNumberGame);
		return;
	}
	
	$CakeBot::WordGame::Schedule = schedule(5000,0,CakeBot_WordGameLoop);
}

function CakeBot_EndWordGame()
{
	cancel($CakeBot::WordGame::Schedule);
	$CakeBot::WordGame::Enabled = 0;
	//cakeSay("Game ended.");
}