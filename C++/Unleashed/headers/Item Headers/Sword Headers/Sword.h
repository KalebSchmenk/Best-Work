#pragma once

#ifndef SWORD_H
#define SWORD_H
#include "Item Headers/Item.h"
class BaseEnemy;

// TODO 
class Sword : public Item
{
public:
	virtual int Attack(BaseEnemy* enemy); 

	virtual void SaveItem() override;
	virtual Item* LoadItem() override;

	Sword();

	int damageOutput = 3;
};
#endif