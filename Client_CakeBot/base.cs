//remove annoying stuff, make chat only respond to self

$NoteDelay = 15000;

function cakeEcho(%msg)
{
	if($CakeID::AnnounceEcho)
	{
		cakeSay(%msg);
	}
	echo(%msg);
}

// [CODE REDACTED]

function CakeBot_NoteLoop()
{
	$CurrentNote++;

	if($CurrentNote > $TotalNotes)
	{
		$CurrentNote = 1;
	}

	cakeSay($Note[$CurrentNote]);

	$CakeBot::NoteLoop = schedule($NoteDelay,0,CakeBot_NoteLoop);
}



package CakeBot_Chat
{
	function clientCmdChatMessage(%a,%b,%c,%fmsg,%cp,%name,%cs,%msg)
	{
		Parent::clientCmdChatMessage(%a,%b,%c,%fmsg,%cp,%name,%cs,%msg);

		if(%name != $pref::Player::NetName && $CakeChatLogging == 1)
		{
			cakeEcho(%name @ ": " @ %msg);
		}

		//number game stuff
		if($CakeBot::NumberGame::Enabled)
		{
			if(getWord(%msg,0) $= $CakeBot::NumberGame::Number) //number is equal to the answer
			{
				CakeBot_NumberGameCorrect(%name);
			}

			if(getWord(%msg,0) $= $CakeBot::NumberGame::Number-1) //number is one under the answer
			{
				CakeBot_NumberGameVeryClose(%name);
			}

			if(getWord(%msg,0) $= $CakeBot::NumberGame::Number+1) //number is one above the answer
			{
				CakeBot_NumberGameVeryClose(%name);
			}

			if(getWord(%msg,0) $= $CakeBot::NumberGame::Number-2) //number is two under the answer
			{
				CakeBot_NumberGameVeryClose(%name);
			}

			if(getWord(%msg,0) $= $CakeBot::NumberGame::Number+2) //number is two above the answer
			{
				CakeBot_NumberGameVeryClose(%name);
			}
		}

		//word game stuff
		if($CakeBot::WordGame::Enabled)
		{
			if(getWord(%msg,0) $= $CakeBot::WordGame::Word) //word is equal to the answer
			{
				CakeBot_WordGameCorrect(%name);
			}
		}

		//commands
		if(getWord(%msg,0) $= "@CAKE" && %name $= "Lake") //to do: add a permission list
		{
			%str = getWords(%msg,1);

			//@CAKE DEFINE
			if(getWord(%str,0) $= "DEFINE")
			{
				%str1 = strUpr(getWords(%str,1));
				%word = CakeBot_GetDefinition(%str1);

				if(%word $= "")
				{
					cakeSay("I can't find the definition for that word. Sorry!");
				}
				else
				{
					cakeSay(%str1 @ ": " @ %word);
				}
			}

			//@CAKE REGWORD
			if(getWord(%str,0) $= "REGWORD")
			{
				%str1 = getWord(%str,1);

				if(%str1 $= "")
				{
					cakeSay("Invalid or blank input!");
					return;
				}

				if(strLen(%str1) < 4)
				{
					cakeSay("The word must have at least four letters in it.");
					return;
				}

				%str2 = getWords(%str,2);

				%wordexists = CakeBot_GetDefinition(%str1);

				if(!%wordexists $= "")
				{
					cakeSay("That word already exists.");
					return;
				}

				CakeBot_RegisterDefinition(%str1,%str2);

				%word = CakeBot_GetDefinition(%str1);

				if(%word $= "")
				{
					cakeSay("I can't find the definition for that word. Sorry!");
				}
				else
				{
					cakeSay(strUpr(%str1) @ ": " @ %word);
				}
			}

			//@CAKE ADDNOTE
			if(getWord(%str,0) $= "ADDNOTE")
			{
				%str1 = getWords(%str,1);

				if(%str1 $= "")
				{
					cakeSay("Invalid or blank input!");
					return;
				}

				if(strLen(%str1) < 4)
				{
					cakeSay("The note must have at least four letters in it.");
					return;
				}

				$TotalNotes++;

				cakeSay(%str1);
				$Note[$TotalNotes] = %str1;
			}

			//@CAKE REMNOTE
			if(getWord(%str,0) $= "REMNOTE")
			{
				%str1 = getWords(%str,1);

				if(%str1 $= "")
				{
					cakeSay("Invalid or blank input!");
					return;
				}

				if(strLen(%str1) < 4)
				{
					cakeSay("The note must have at least four letters in it.");
					return;
				}

				cakeSay("The following note has been removed:");
				cakeSay($Note[$TotalNotes]);
				$TotalNotes--;
			}

			//@CAKE SETNOTEDELAY
			if(getWord(%str,0) $= "SETNOTEDELAY")
			{
				%str1 = getWords(%str,1);

				if(%str1 > 0 || %str1 < 999999)
				{
					$NoteDelay = %str1;
					return;
				}

				cakeSay("Invalid or blank input!");
			}

			//@CAKE STARTNOTES
			if(getWord(%str,0) $= "STARTNOTES")
			{
				CakeBot_NoteLoop();
			}

			//@CAKE ENDNOTES
			if(getWord(%str,0) $= "ENDNOTES")
			{
				cakeSay("Notes will no-longer display.");
				cancel($CakeBot::NoteLoop);
			}

			//@CAKE STARTNUMBERGAME
			if(getWord(%str,0) $= "STARTNUMBERGAME")
			{
				CakeBot_StartNumberGame();
			}

			//@CAKE STARTMINEBOT
			if(getWord(%str,0) $= "STARTMINEBOT")
			{
				CakeMine_Init();
			}

			//@CAKE ENDMINEBOT
			if(getWord(%str,0) $= "ENDMINEBOT")
			{
				CakeMine_Cancel();
			}

			//@CAKE ENDGAME
			if(getWord(%str,0) $= "ENDGAME")
			{
				CakeBot_EndNumberGame();
				CakeBot_EndWordGame();
				cakeSay("Game ended.");
			}

			//@CAKE QUIT
			if(getWord(%str,0) $= "QUIT")
			{
				quit();
			}

			//@CAKE DISCONNECT
			if(getWord(%str,0) $= "DISCONNECT")
			{
				disconnect();
			}

			//@CAKE SUICIDE
			if(getWord(%str,0) $= "SUICIDE")
			{
				commandToServer('suicide');
			}

			//@CAKE EVAL
			if(getWord(%str,0) $= "EVAL" && !$CakeBot::DisableEval)
			{
				//%str1 = getWords(%str,1);
				//%eval = eval(%str1);
				//cakeSay(%eval);
				cakeSay("Eval is disabled.");
			}

			//@CAKE BACKUPDEFINITIONS
			if(getWord(%str,0) $= "BACKUPDEFINITIONS")
			{
				cakeEcho("Backing up definitions file...");
				%str1 = getWords(%str,1);

				if(!%str1)
				{
					cakeEcho("No input. Using default file path...");
					%path = "config/client/cakebot/archive/definitions_latestBackup.cs";
				}
				else
				{
					%file = %str1;
				}

				cakeEcho("Creating backup at " @ %file);
				cakeSay("Creating backup at " @ %file);
				copyTextFile($CakeBot_DefList,%file);

				cakeEcho("Done, testing for file...");
				discoverFile(%file);
				if(!isFile(%file))
				{
					cakeSay("Failed to create a backup.");
					return;
				}
				cakeEcho("Backup created successfully.");
				cakeSay("Backup created successfully.");
			}

			//@CAKE STARTWORDGAME
			if(getWord(%str,0) $= "STARTWORDGAME")
			{
				CakeBot_StartWordGame();
			}

			//a
		}
	}
};
deactivatePackage("CakeBot_Chat");
activatePackage("CakeBot_Chat");
