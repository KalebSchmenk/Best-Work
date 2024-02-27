//
// Base Dungeon Implementation

#include "Dungeon Headers/Dungeon.h"


DungeonDifficulty Dungeon::GetDifficulty() 
{
	return dungeonDifficulty;
}

bool Dungeon::StartDungeon(PlayerObj *player)
{
	throw std::invalid_argument("Base class used instead of derivitive to generate dungeon");
}

bool Dungeon::EndDungeon()
{
	return true;
}

void Dungeon::SelectDifficulty()
{
	bool goodSelection = false;
	std::string selectionString;
	int selection;
	do
	{
		system("CLS"); // Clear console (Slow)

		std::cout << "What difficulty do you want this dungeon?\n\n";

		std::cout << "Options:\n";
		std::cout << "1. Easy\n";
		std::cout << "2. Medium\n";
		std::cout << "3. Hard\n\n";

		std::cin >> selectionString;

		try
		{
			selection = std::stoi(selectionString);
		}
		catch (...)
		{
			selection = 10; // Not accepted input
		}

		switch (selection)
		{
			case 1:
				std::cout << "\nYou chose Easy!";

				goodSelection = true;

				Sleep(1750);

				break;

			case 2:
				std::cout << "\nYou chose Medium!";

				goodSelection = true;

				Sleep(1750);

				break;

			case 3:
				std::cout << "\nYou chose Hard!";

				goodSelection = true;

				Sleep(1750);

				break;

			default: // Loop
			{
				std::cout << "\n\nSorry, couldn't catch that. Please enter a menu option\n\n";
				Sleep(1750);
			}
		}
	} while (!goodSelection);	

	this->dungeonDifficulty = (DungeonDifficulty)selection;
}