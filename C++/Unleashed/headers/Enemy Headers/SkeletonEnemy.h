#pragma once

#include "Enemy Headers/BaseEnemy.h"

class SkeletonEnemy : public BaseEnemy
{
public:
	int Attack(PlayerObj *player) override;
	void TakeDamage(int damage) override;
	Item Die() override; 
	SkeletonEnemy(std::string name, int health, int damageOutput, Item item);
};
