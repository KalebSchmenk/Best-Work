//
// Base Dungeon Implementation

#include "Dungeon Headers/Dungeon.h"


DungeonDifficulty Dungeon::GetDifficulty() 
{
	return dungeonDifficulty;
}

bool Dungeon::StartDungeon(PlayerObj *player)
{
	throw std::invalid_argument("Base class used instead of derivitive to generate dungeon");
}