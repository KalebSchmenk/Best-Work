#pragma once

#ifndef CAVE_DUNGEON_H
#define CAVE_DUNGEON_H

#include "Player.h"
#include "Dungeon.h"
#include <stdlib.h>
#include <iostream>

class CaveDungeon : public Dungeon
{
public:
	DungeonDifficulty GetDifficulty() override;
	bool StartDungeon(PlayerObj *player) override;
	bool EndDungeon() override;

	CaveDungeon(DungeonDifficulty difficulty);

};
#endif // CAVE_DUNGEON_H