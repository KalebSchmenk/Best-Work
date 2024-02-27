#pragma once

#ifndef BASE_ENEMY_H
#define BASE_ENEMY_H

#include "Player.h"
#include "Item Headers\Items.h"
#include <stdlib.h>
#include <iostream>

class BaseEnemy
{
public:
	virtual int Attack(PlayerObj *player);
	virtual void TakeDamage(int damage);
	int GetHealth();
	std::string GetName();
	virtual Item Die();

protected:
	std::string name;
	int health;
	int damageOutput;
	Item itemToDrop;

};
#endif // BASE_ENEMY_H