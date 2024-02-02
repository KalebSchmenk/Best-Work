#pragma once

#ifndef DUNGEON_GENERATOR_H
#define DUNGEON_GENERATOR_H

#include "Player.h"
#include <stdexcept>

static enum DungeonDifficulty
{
	Easy,
	Medium,
	Hard
};

class Dungeon
{
public:	
	virtual DungeonDifficulty GetDifficulty();
	virtual bool StartDungeon(PlayerObj *player);

protected:
	DungeonDifficulty dungeonDifficulty;
};
#endif // DUNGEON_GENERATOR_H