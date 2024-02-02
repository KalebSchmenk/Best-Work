#pragma once

#ifndef FILEMANAGER_H
#define FILEMANAGER_H

#include <iostream>
#include <stdexcept>
#include <string>
#include <fstream>
#include "Player.h"

class DataManager
{
public:
	void SaveData(PlayerData* playerData); // Save data func
	int DeleteData(); // Delete data func
	void LoadData(PlayerData* data); // Load data func
};
#endif // FILEMANAGER_H
