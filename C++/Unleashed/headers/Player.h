#pragma once

#ifndef PLAYER_H
#define PLAYER_H

#include <iostream>
#include <string>
#include <vector>
#include "Item Headers/Items.h"

struct PlayerData
{
	std::string playerName = "default"; // Player's current name
	int playerHealth = -1; // Player's current health
	int playerLevel = -1; // Player's current level

	Sword playerSword; // Current player sword
	Coin coins; // Player's coins

	Item itemSlotOne;
	Item itemSlotTwo;
	Item itemSlotThree;
	Item itemSlotFour;
	Item itemSlotFive;
	Item itemSlotSix;

	PlayerData()
	{
		// Tell each item where their file is

		playerSword.filePath = "Sword.txt";

		itemSlotOne.filePath = "ItemOne.txt";
		itemSlotTwo.filePath = "ItemTwo.txt";
		itemSlotThree.filePath = "ItemThree.txt";
		itemSlotFour.filePath = "ItemFour.txt";
		itemSlotFive.filePath = "ItemFive.txt";
		itemSlotSix.filePath = "ItemSix.txt";
	}
};

class PlayerObj
{
public:
	std::string GetName();
	int GetHealth();
	int GetLevel();
	PlayerData* GetPlayerData();
	
	int SetPlayerData(PlayerData newData);
	void SetName(std::string nameToSet);
	void SetHealth(int newHealth);

	int AddItem(Item newItem);
	int RemoveItem(int index);
	void PrintItems();

	void LevelUp();

	bool TakeDamage(int damage);
	void PlayerDied();

	PlayerObj(std::string name, int startHealth);
	PlayerObj();

private:
	PlayerData playerData; // Player's current data as a struct
};
#endif // PLAYER_H