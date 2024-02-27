#pragma once

#ifndef ITEM_H
#define ITEM_H

#include <iostream>
#include <fstream>
#include <string>
#include <filesystem>

// rapidjson
#include "document.h"
#include "writer.h"
#include "stringbuffer.h"

// All collectibles are considered an Item
// Classes can inherit from this base to expand item functionality
class Item
{
public:
	virtual std::string GetItemName(); // Returns item name
	virtual std::string GetItemDescription(); // Returns item description
	virtual int GetItemCount(); // Returns how many of this "item" is within this class object. (A "Coin" item may contain more than 1 coin in the class)

	virtual void SetItemName(std::string newName);// Sets new item name
	virtual void SetItemDescription(std::string newDescription);// Sets new item description
	virtual void SetItemCount(int setTo); // Sets count to this number
	void SetValid(bool set); // Sets item validity
	virtual void ChangeItemCount(int changeBy); // Changes count of item(s) within this object

	virtual void SaveItem();
	virtual Item* LoadItem();
	virtual void DeleteItem();

	bool IsValid(); // Returns validItem

	std::string filePath = ""; // Save/Load filepath

protected:
	std::string itemName = "null"; // Default null item (An "Item" class should never be instantiated) 
	std::string itemDescription = "null"; // Default null description
	int itemCount = 1; // Default 1

	bool validItem = false; // Flag for pseudo items tagged as not for use
};

#endif // ITEM_H