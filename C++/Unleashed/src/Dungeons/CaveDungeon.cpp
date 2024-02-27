//
// Cave Dungeon Implementation

#include "Dungeon Headers/CaveDungeon.h"
#include "Item Headers/Items.h"
#include "Enemy Headers/Enemies.h"
#include "Duel.h"
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
	Sleep(2500);

	SelectDifficulty();

	std::cout << "\n\nYou begin to wander the cave...\n\n";
	Sleep(2500);

	this->player = player;
	Duel* duel;

	switch(dungeonDifficulty)
	{
		case Easy:
		{
			Item bone = Item();
			bone.SetItemName("Bone");
			bone.SetItemDescription("A skeletal bone");

			SkeletonEnemy* enemy = new SkeletonEnemy("Skeleton",10, 1, bone);

			duel = new Duel(player, enemy);
			duel->StartDuel();

			system("CLS"); // Clear console (Slow)

			if (duel->playerWon == false && duel->enemyWon == false)
			{
				// Run away
				std::cout << "You run away!\n\n";
				Sleep(2500);
				EndDungeon();
			}
			else if (duel->playerWon == true && duel->enemyWon == false)
			{
				// Player won
				std::cout << "You beat the " << enemy->GetName() << "!\n\n";

				Sleep(2500);
				player->AddItem(enemy->Die());
				int coins = 3;
				player->GetPlayerData()->coins.ChangeItemCount(coins);

				std::cout << "You got " << enemy->Die().GetItemCount() << " " << enemy->Die().GetItemName() << "(s)";
				std::cout << "\n\nand\n\n" << coins << " coins!";

				player->LevelUp();

				Sleep(2500);

				EndDungeon();
			}
			else // Assume enemy won
			{
				// Enemy won
				player->PlayerDied();
				EndDungeon();
				return false;
			}
			break;
		}
		case Medium:
		{
			Sword sword = Sword();
			sword.SetItemName("Steel Sword");
			sword.SetItemDescription("A sword made of steel");
			sword.damageOutput = 8;

			SkeletonEnemy* enemy = new SkeletonEnemy("Hardened Skeleton", 15, 2, sword);

			duel = new Duel(player, enemy);
			duel->StartDuel();

			system("CLS"); // Clear console (Slow)

			if (duel->playerWon == false && duel->enemyWon == false)
			{
				// Run away
				std::cout << "You run away!\n\n";
				Sleep(2500);
				EndDungeon();
			}
			else if (duel->playerWon == true && duel->enemyWon == false)
			{
				// Player won
				std::cout << "You beat the " << enemy->GetName() << "!\n\n";

				Sleep(2500);
				player->GetPlayerData()->playerSword = sword;
				int coins = 8;
				player->GetPlayerData()->coins.ChangeItemCount(coins);

				std::cout << "You got " << enemy->Die().GetItemCount() << " " << enemy->Die().GetItemName() + "(s)";
				std::cout << "\n\nand\n\n";
				std::cout << coins << " coins!";

				player->LevelUp();

				Sleep(2500);

				EndDungeon();
			}
			else // Assume enemy won
			{
				// Enemy won
				player->PlayerDied();
				EndDungeon();
				return false;
			}
			break;
		}	
		case Hard:
		{
			Item item = Item();
			item.SetItemName("Legendary Black Orb");
			item.SetItemDescription("A strange eniminating black orb...");

			SkeletonEnemy* enemy = new SkeletonEnemy("Armored Skeleton", 20, 4, item);

			duel = new Duel(player, enemy);
			duel->StartDuel();

			system("CLS"); // Clear console (Slow)

			if (duel->playerWon == false && duel->enemyWon == false)
			{
				// Run away
				std::cout << "You run away!\n\n";
				Sleep(2500);
				EndDungeon();
			}
			else if (duel->playerWon == true && duel->enemyWon == false)
			{
				// Player won
				std::cout << "You beat the " << enemy->GetName() << "!\n\n";

				Sleep(2500);
				player->AddItem(enemy->Die());
				int coins = 15;
				player->GetPlayerData()->coins.ChangeItemCount(coins);

				std::cout << "You got " << enemy->Die().GetItemCount() << " " << enemy->Die().GetItemName() << "(s)";
				std::cout << "\n\nand\n\n" << coins << " coins!";

				std::cout << "\n\n======Thanks for playing my demo!======\n\n";

				player->LevelUp();

				Sleep(2500);

				EndDungeon();
			}
			else // Assume enemy won
			{
				// Enemy won
				player->PlayerDied();
				EndDungeon();
				return false;
			}
			break;
		}
		default:
			break;
	}

	Sleep(2500);
	return true; // Player survived
}

bool CaveDungeon::EndDungeon()
{
	return (player->GetHealth() <= 0) ? true : false;
}
