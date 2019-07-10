
// [CODE REDACTED]

package CakeBot_Commands
{
	function clientCmdChatMessage(%a,%b,%c,%fmsg,%cp,%name,%cs,%msg)
	{
		Parent::clientCmdChatMessage(%a,%b,%c,%fmsg,%cp,%name,%cs,%msg);

		if(!%name == $pref::Player::NetName)
		{
			echo(%name @ ": " @ %msg);
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
		if(getWord(%msg,0) $= "@CAKE" && %name $= "LakeYS") //to do: add a permission list
		{
			%str = getWords(%msg,1);

			//@CAKE DEFINE
			if(getWord(%str,0) $= "DEFINE")
			{
				%str1 = strUpr(getWords(%str,1));
				%word = CakeBot_GetDefinition(%str1);

				if(%word $= "")
				{
					commandToServer('messagesent',"I can't find the definition for that word. Sorry!");
				}
				else
				{
					commandToServer('messagesent',%str1 @ ": " @ %word);
				}
			}

			//@CAKE REGWORD
			if(getWord(%str,0) $= "REGWORD")
			{
				%str1 = getWord(%str,1);

				if(%str1 $= "")
				{
					commandToServer('messagesent',"Invalid or blank input!");
					return;
				}

				if(strLen(%str1) < 4)
				{
					commandToServer('messagesent',"The word must have at least four letters in it.");
					return;
				}

				%str2 = getWords(%str,2);

				%wordexists = CakeBot_GetDefinition(%str1);

				if(!%wordexists $= "")
				{
					commandToServer('messagesent',"That word already exists.");
					return;
				}

				CakeBot_RegisterDefinition(%str1,%str2);

				%word = CakeBot_GetDefinition(%str1);

				if(%word $= "")
				{
					commandToServer('messagesent',"I can't find the definition for that word. Sorry!");
				}
				else
				{
					commandToServer('messagesent',strUpr(%str1) @ ": " @ %word);
				}
			}

			//@CAKE STARTNUMBERGAME
			if(getWord(%str,0) $= "STARTNUMBERGAME")
			{
				CakeBot_StartNumberGame();
			}

			//@CAKE ENDGAME
			if(getWord(%str,0) $= "ENDGAME")
			{
				CakeBot_EndNumberGame();
				CakeBot_EndWordGame();
				commandToServer('messagesent',"Game ended.");
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

			//@CAKE EVAL
			if(getWord(%str,0) $= "EVAL")
			{
				%str1 = strUpr(getWords(%str,1));
				%eval = eval(%str1);
				commandToServer('messagesent',%eval);
			}

			//@CAKE BACKUPDEFINITIONS
			if(getWord(%str,0) $= "BACKUPDEFINITIONS")
			{
				echo("Backing up definitions file...");
				%str1 = getWords(%str,1);

				if(!%str1)
				{
					echo("No input. Using default file path...");
					%path = "config/client/cakebot/archive/definitions_latestBackup.cs";
				}
				else
				{
					%file = %str1;
				}

				echo("Creating backup at " @ %file);
				commandToServer('messagesent',"Creating backup at " @ %file);
				copyTextFile($CakeBot_DefList,%file);

				echo("Done, testing for file...");
				discoverFile(%file);
				if(!isFile(%file))
				{
					commandToServer('messagesent',"Failed to create a backup.");
					return;
				}
				echo("Backup created successfully.");
				commandToServer('messagesent',"Backup created successfully.");
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
activatePackage("CakeBot_Commands");
