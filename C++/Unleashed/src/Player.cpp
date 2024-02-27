//
// Player Implementation

#include "Player.h"
#include <windows.h>

// Data struct
extern struct PlayerData playerData;


// Player constructor
PlayerObj::PlayerObj(std::string name, int health)
{
	playerData.playerHealth = health;
	playerData.playerName = name;
}

PlayerObj::PlayerObj()
{
	playerData.playerName = "Phoenix";
	playerData.playerHealth = -1;
}

// Getters and Setters
int PlayerObj::GetHealth()
{
	return playerData.playerHealth;
}

std::string PlayerObj::GetName() 
{
	return playerData.playerName;
}

PlayerData* PlayerObj::GetPlayerData()
{
	return &playerData;
}

int PlayerObj::GetLevel()
{
	return playerData.playerLevel;
}


void PlayerObj::SetHealth(int newHealth)
{
	playerData.playerHealth = newHealth;
}

void PlayerObj::SetName(std::string newName)
{
	playerData.playerName = newName;
}

int PlayerObj::AddItem(Item newItem)
{
	// I love the lengths I have to go to keep the ability
	// To save and load files
	if (!playerData.itemSlotOne.IsValid())
	{
		playerData.itemSlotOne = newItem;
		playerData.itemSlotOne.filePath = "ItemOne.txt";
		return 1;
	}
	else if (!playerData.itemSlotTwo.IsValid())
	{
		playerData.itemSlotTwo = newItem;
		playerData.itemSlotTwo.filePath = "ItemTwo.txt";
		return 1;
	}
	else if (!playerData.itemSlotThree.IsValid())
	{
		playerData.itemSlotThree = newItem;
		playerData.itemSlotThree.filePath = "ItemThree.txt";
		return 1;
	}
	else if (!playerData.itemSlotFour.IsValid())
	{
		playerData.itemSlotFour = newItem;
		playerData.itemSlotFour.filePath = "ItemFour.txt";
		return 1;
	}
	else if (!playerData.itemSlotFive.IsValid())
	{
		playerData.itemSlotFive = newItem;
		playerData.itemSlotFive.filePath = "ItemFive.txt";
		return 1;
	}
	else if (!playerData.itemSlotSix.IsValid())
	{
		playerData.itemSlotSix = newItem;
		playerData.itemSlotSix.filePath = "ItemSix.txt";
		return 1;
	}
	else
	{
		return 0; // No slot available
	}
}

int PlayerObj::RemoveItem(int index)
{
	Item item;

	switch (index)
	{
		case 1:
		{
			playerData.itemSlotOne = item;
			return 1;
		}
		case 2:
		{
			playerData.itemSlotTwo = item;
			return 1;
		}
		case 3:
		{
			playerData.itemSlotThree = item;
			return 1;
		}
		case 4:
		{
			playerData.itemSlotFour = item;
			return 1;
		}
		case 5:
		{
			playerData.itemSlotFive = item;
			return 1;
		}
		case 6:
		{
			playerData.itemSlotSix = item;
			return 1;
		}
		default:
		{
			return 0; // No slot deleted
		}
	}
	return 0; // No slot deleted
}

void PlayerObj::PrintItems()
{
	std::cout << "===========================================\n";
	std::cout << playerData.playerName << "'s inventory\n\n";
	std::cout << "Health: " << playerData.playerHealth << "\n\n";
	std::cout << "Level: " << playerData.playerLevel << "\n\n";
	std::cout << "Current Sword : " << playerData.playerSword.GetItemName() << " which deals " << playerData.playerSword.damageOutput << " damage!\n";
	std::cout << "Gold Count : " << playerData.coins.GetItemCount() << "\n\n";

	std::cout << "Current Items" << "\n";
	bool foundAtLeastOneItem = false;
	if (playerData.itemSlotOne.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotOne.GetItemName() << " : " << playerData.itemSlotOne.GetItemCount() << " | " << playerData.itemSlotOne.GetItemDescription() << "\n";
	}
	if (playerData.itemSlotTwo.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotTwo.GetItemName() << " : " << playerData.itemSlotTwo.GetItemCount() << " | " << playerData.itemSlotTwo.GetItemDescription() << "\n";
	}
	if (playerData.itemSlotThree.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotThree.GetItemName() << " : " << playerData.itemSlotThree.GetItemCount() << " | " << playerData.itemSlotThree.GetItemDescription() << "\n";
	}
	if (playerData.itemSlotFour.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotFour.GetItemName() << " : " << playerData.itemSlotFour.GetItemCount() << " | " << playerData.itemSlotFour.GetItemDescription() << "\n";
	}
	if (playerData.itemSlotFive.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotFive.GetItemName() << " : " << playerData.itemSlotFive.GetItemCount() << " | " << playerData.itemSlotFive.GetItemDescription() << "\n";
	}
	if (playerData.itemSlotSix.IsValid())
	{
		foundAtLeastOneItem = true;
		std::cout << playerData.itemSlotSix.GetItemName() << " : " << playerData.itemSlotSix.GetItemCount() << " | " << playerData.itemSlotSix.GetItemDescription() << "\n";
	}

	if (!foundAtLeastOneItem) std::cout << "\nYou have not found any items yet!\n";
}

int PlayerObj::SetPlayerData(PlayerData newData)
{
	try
	{
		playerData = newData;
		return 1;
	}
	catch(...) 
	{
		return 0;
	}
}

// Levels up player by 1. No paramater to ensure linear progression
void PlayerObj::LevelUp()
{
	playerData.playerLevel++;
}

// When the player dies
void PlayerObj::PlayerDied()
{
	system("CLS"); // Clear console (Slow)
	std::cout << "===========================================\n";
	std::cout << "|            --=YOU DIED=--               |\n";
	std::cout << "==========================================="  ;

	Sleep(2500);

	PlayerData newPlayerData;
	playerData = newPlayerData;
}

// Player damage intake
// If player died from attack, return false
bool PlayerObj::TakeDamage(int damage)
{
	playerData.playerHealth -= damage;

	if (playerData.playerHealth <= 0)
	{
		//PlayerDied();
		return false;
	}
	return true;
}