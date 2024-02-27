#pragma once

#ifndef DUEL_H
#define DUEL_H

#include "Player.h"
#include "Enemy Headers/Enemies.h"
#include <stdlib.h>
#include <iostream>
#include <windows.h>


class Duel
{
public:
	Duel(PlayerObj* player, BaseEnemy* enemy);
	void StartDuel();
	void EndDuel();

	void PlayerTurn();
	void EnemyTurn();

	void PrintStats();

	bool playerWon = false;
	bool enemyWon = false;

	PlayerObj* player;
	BaseEnemy* enemy;
};
#endif
