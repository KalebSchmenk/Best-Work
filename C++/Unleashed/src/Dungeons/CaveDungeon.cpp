//
// Cave Dungeon Implementation

#include "Dungeon Headers/CaveDungeon.h"
#include "Item Headers/Items.h"
#include <windows.h>


// Constructor
CaveDungeon::CaveDungeon(DungeonDifficulty difficulty)
{
	dungeonDifficulty = difficulty;
}

DungeonDifficulty CaveDungeon::GetDifficulty()
{
	return dungeonDifficulty;
}


bool CaveDungeon::StartDungeon(PlayerObj *player)
{
	system("CLS"); // Clear console (Slow)
	std::cout << "Welcome to the CAVE DUNGEON!\n\n";

	// lol
	switch(dungeonDifficulty)
	{
		case Easy:
		{
			Sleep(1250);

			std::cout << "\nYou're difficulty level is: EASY\n\n";
			std::cout << "YOU TOOK 1 DAMAGE!\n\n";
			player->TakeDamage(1);

			Sleep(1250);

			std::cout << "You found 5 gold and a sword!";
			Coin coins(5);
			Sword sword;
			sword.SetItemName("Sword");
			player->AddItem(coins);
			player->AddItem(sword);
			break;
		}
		case Medium:
		{
			Sleep(1250);

			std::cout << "\nYou're difficulty level is: MEDIUM\n\n";
			std::cout << "YOU TOOK 5 DAMAGE!\n\n";
			player->TakeDamage(5);

			Sleep(1250);

			std::cout << "You found 5 gold and a sword!";
			Coin coins(5);
			Sword sword;
			sword.SetItemName("Sword");
			player->AddItem(coins);
			player->AddItem(sword);
			break;
		}	
		case Hard:
		{
			Sleep(1250);

			std::cout << "\nYou're difficulty level is: HARD\n\n";
			std::cout << "YOU TOOK 10 DAMAGE!\n\n";
			bool alive = player->TakeDamage(10);
			if (!alive) return false;

			Sleep(1250);

			std::cout << "You found 5 gold and a sword!\n\n";
			Sword sword;
			sword.SetItemName("Iron Sword");
			player->GetPlayerData()->playerSword = sword;
			player->GetPlayerData()->coins.ChangeItemCount(5);

			Sleep(1250);

			std::cout << "You also found a mysterious black orb!\n\n";
			Item item;
			item.SetItemName("Black Orb");
			player->AddItem(item);
			break;
		}
		default:
			break;
	}

	Sleep(2500);
	return true; // Player survived
}
