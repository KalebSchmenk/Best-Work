//
// Skeleton Enemy Implementation

#include "Enemy Headers/SkeletonEnemy.h"

SkeletonEnemy::SkeletonEnemy(std::string name, int health, int damageOutput, Item item)
{
	this->health = health;
	this->damageOutput = damageOutput;
	this->itemToDrop = item;
	this->name = name;
}

int SkeletonEnemy::Attack(PlayerObj* player)
{
	// Do Damage to player
	player->TakeDamage(damageOutput);
	return damageOutput;
}

void SkeletonEnemy::TakeDamage(int damage)
{
	// Take damage from player attack
	this->health -= damage;

	if (this->health <= 0)
	{
		Die();
	}
}

Item SkeletonEnemy::Die()
{
	// Die and drop item
	return itemToDrop;
}