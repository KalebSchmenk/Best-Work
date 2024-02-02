#pragma once

#ifndef ITEM_H
#define ITEM_H

#include <iostream>
#include <fstream>
#include <string>

// All collectibles are considered an Item
// Classes can inherit from this base to expand item functionality
class Item
{
public:
	virtual std::string GetItemName(); // Returns item name
	virtual int GetItemCount(); // Returns how many of this "item" is within this class object. (A "Coin" item may contain more than 1 coin in the class)

	virtual void SetItemName(std::string newName);// Sets new item name
	virtual void SetItemCount(int setTo); // Sets count to this number
	virtual void ChangeItemCount(int changeBy); // Changes count of item(s) within this object

	bool IsValid(); // Returns validItem

protected:
	std::string itemName = "null"; // Default null item (An "Item" class should never be instantiated) 
	int itemCount = 1; // Default 1

	bool validItem = false; // Flag for pseudo items tagged as not for use
};

#endif // ITEM_H