#pragma once

#ifndef DUNGEON_GENERATOR_H
#define DUNGEON_GENERATOR_H

#include "Player.h"
#include <windows.h>
#include <stdexcept>

static enum DungeonDifficulty
{
	Easy = 1,
	Medium = 2,
	Hard = 3
};

class Dungeon
{
public:	
	virtual DungeonDifficulty GetDifficulty();
	virtual bool StartDungeon(PlayerObj *player);
	virtual bool EndDungeon();
	void SelectDifficulty();

protected:
	DungeonDifficulty dungeonDifficulty;
	PlayerObj* player;
};
#endif // DUNGEON_GENERATOR_H