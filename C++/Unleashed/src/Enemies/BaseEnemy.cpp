//
// Basic Enemy Implementation

#include "Enemy Headers/BaseEnemy.h"

int BaseEnemy::Attack(PlayerObj *player)
{
	// Do Damage to player
	return 0;
}

void BaseEnemy::TakeDamage(int damage)
{
	// Take damage from player attack
}

Item BaseEnemy::Die()
{
	// Die and drop item
	Item item;
	return item;
}

int BaseEnemy::GetHealth()
{
	return health;
}

std::string BaseEnemy::GetName()
{
	return name;
}