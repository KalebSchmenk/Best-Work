// Main starting point

#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <windows.h>

#include "Player.h" // Player
#include "FileManager.h" // File Management
#include "Dungeon Headers/Dungeons.h" // All Dungeons
#include "Item Headers/Items.h" // All Items


void PrintTitle();
void TryLoadFile();
void EnterMenuLoop();
void AskToDeleteFile();
bool AskForConfirmation();


// Game Related Data
DataManager data;
PlayerData* playerData = new PlayerData;
PlayerObj player;
bool playGame = true;



	// Enter Intro
	// Get Player Info
	// Create init player obj
	// Save init player obj
	// Open Main Menu
	// Only allow first level tutorial selection
	// Enter tutorial version of game loop
	// Exit to main menu
	// Begin tutorial version of main menu loop
	// Let player begin journey 

int main()
{
	while (playGame)
	{
		PrintTitle();
		TryLoadFile();

		//_______________________ 
		// Game
		EnterMenuLoop();
		//_______________________
	}
	AskToDeleteFile();
	return 1;
}

void TryLoadFile()
{
	// If player data file exists use it, if not create new player data
	try
	{
		data.LoadData(playerData);	// LoadData() Throws an exception if it could not load a player data file which is why
		// we have it in a try-catch. Not neccessary, but an interesting implementation

		// If load successful
		player.SetPlayerData(*playerData); // Handles name and health setting internally

		std::cout << "Hello again, " << playerData->playerName << "\n\n";

		player.PrintItems();

		// Sleep for title screen. Otherwise it disappears too quickly
		Sleep(7500);
	}
	catch (...)
	{
		// If load unsuccessful
		std::string name;

		std::cout << "Hello new player!\n\n";
		std::cout << "Please type a name for your character...\n\n";

		std::cin >> name;

		playerData->playerName = name;
		playerData->playerHealth = 15; // Default
		playerData->playerLevel = -1; // Default

		player.SetPlayerData(*playerData); // Handles name and health setting internally
		player.LevelUp();
		player.LevelUp();
	}
}

void PrintTitle()
{
	system("CLS"); // Clear console (Slow)
	std::cout << "===================================================================================================\n\n";
	std::cout << "                                ______                   _______               ______    ___        \n";
	std::cout << "|      |   |     |   |         |              /\\        |          |      |   |         |   \\     \n";
	std::cout << "|      |   |\\    |   |         |             /  \\       |          |      |   |         |    \\   \n";
	std::cout << "|      |   | \\   |   |         |            /    \\      |______    |      |   |         |     \\  \n";
	std::cout << "|      |   |  \\  |   |         |------     /______\\            |   |------|   |------   |     /   \n";
	std::cout << "|      |   |   \\ |   |         |          /        \\           |   |      |   |         |    /    \n";
	std::cout << "|______|   |    \\|   |______   |______   /          \\   _______|   |      |   |______   |___/     \n";
	std::cout << "\n\n===================================================================================================\n\n";
}

void EnterMenuLoop()
{
	bool goodSelect = false;
	int menuSelect;

	// Menu select loop
	do
	{
		system("CLS"); // Clear console (Slow)

		std::cout << "|--=Unleashed=--|\n\n\n";
		std::cout << "-MAIN MENU-\n\n";
		std::cout << "Please select an option";
		std::cout << "\n1. Enter A Dungeon";
		std::cout << "\n2. View Inventory";
		std::cout << "\n3. Delete Character";
		std::cout << "\n4. Quit\n\n";
		std::cin >> menuSelect;

		switch (menuSelect)
		{
			case 1:
			{
				// Entering a dungeon
				DungeonDifficulty difficulty = Hard;
				Dungeon* enteredDungeon = new CaveDungeon(difficulty);

				bool alive = enteredDungeon->StartDungeon(&player);
			
				if (!alive) 
				{
					data.DeleteData();
					goodSelect = true;
					break;
				}

				player.LevelUp();
				break;
			}
			case 2:
			{
				system("CLS"); // Clear console (Slow)
				player.PrintItems();
				Sleep(3500);
				break;
			}
			case 3:
			{
				bool answer = AskForConfirmation();
				if (answer) 
				{
					player.PlayerDied();
					data.DeleteData();
					goodSelect = true;
				}
				break;
			}
			case 4:
			{
				data.SaveData(player.GetPlayerData());
				playGame = false;
				goodSelect = true;
				break;
			}
			default:
			{
				break;
			}
		}
	} while (goodSelect != true);
}

void AskToDeleteFile()
{
	std::cout << "\n\nDelete saved file? (y/n)\n";

	char answer;
	std::cin >> answer;

	while (answer != 'y' && answer != 'n')
	{
		std::cout << "\n\nSorry, didn't catch that. Delete saved file? (y/n)\n";
		std::cin >> answer;
	}
	if (answer == 'y') data.DeleteData();
}

bool AskForConfirmation()
{
	std::cout << "\n\nDelete character? (y/n)\n";

	char answer;
	std::cin >> answer;

	while (answer != 'y' && answer != 'n')
	{
		std::cout << "\n\nSorry, didn't catch that. Delete character? (y/n)\n";
		std::cin >> answer;
	}

	if (answer == 'y') return true;

	return false;
}