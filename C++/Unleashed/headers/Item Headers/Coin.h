#pragma once

#include "Item Headers/Item.h"

class Coin : public Item
{
public:
	virtual Item Buy(); // I like this function signature. Not sure if coins should control purchases though

	Coin(int amount); // Ensure to override item vars in Coin instructor
	Coin();
};