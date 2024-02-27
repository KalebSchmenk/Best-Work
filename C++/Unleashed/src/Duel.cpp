//
// Duel Implementation

#include "Duel.h"

Duel::Duel(PlayerObj* player, BaseEnemy* enemy)
{
	this->player = player;
	this->enemy = enemy;
}

void Duel::StartDuel()
{
	system("CLS"); // Clear console (Slow)
	std::cout << "You've come across a(n) " << enemy->GetName() << "!\n\n";
	std::cout << "You charge at them ready to fight!";

	Sleep(4500);
	PlayerTurn();

	EndDuel();
}

void Duel::EndDuel()
{

}

void Duel::PlayerTurn()
{
	bool goodSelection = false;
	do
	{
		system("CLS"); // Clear console (Slow)
		PrintStats();
		std::cout.flush();

		std::cout << "Options:\n\n";
		std::cout << "1. Attack with your sword\n";
		std::cout << "2. Run away!\n\n";
		
		int damage;

		// Ask to attack or run away
		std::string selectionString;
		std::cin >> selectionString;
		int selection;

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
				damage = player->GetPlayerData()->playerSword.Attack(enemy);

				std::cout << "\n" << "You dealt " << damage << " damage to the enemy!";

				goodSelection = true;

				Sleep(2500);

				break;

			case 2:
				// run away
				playerWon = false;
				enemyWon = false;
				goodSelection = true;
				return;

			default: // Loop
			{
				std::cout << "\n\nSorry, couldn't catch that. Please enter a menu option\n\n";
				Sleep(1750);
			}
		}		
	} while (!goodSelection);


	if (enemy->GetHealth() <= 0)
	{
		playerWon = true;
		return;
	}

	EnemyTurn();
}

void Duel::EnemyTurn()
{
	system("CLS"); // Clear console (Slow)
	PrintStats();

	int damage = enemy->Attack(player);

	std::cout << "The " << enemy->GetName() << " attacks you dealing " << damage << " damage!";

	Sleep(2500);

	if (player->GetHealth() <= 0)
	{
		enemyWon = true;
		return;
	}

	PlayerTurn();
}

void Duel::PrintStats()
{
	std::cout << "========================\n";
	std::cout << "BATTLE INFO\n\n\n";

	std::cout << player->GetName() << "'s health: " << player->GetHealth() << "\n\n";

	std::cout << enemy->GetName() << "'s health: " << enemy->GetHealth() << "\n\n\n";
}