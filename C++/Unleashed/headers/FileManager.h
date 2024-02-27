#pragma once

#ifndef FILEMANAGER_H
#define FILEMANAGER_H

#include <iostream>
#include <stdexcept>
#include <string>
#include <fstream>
#include "Player.h"
// rapidjson
#include "document.h"
#include "writer.h"
#include "stringbuffer.h"

class DataManager
{
public:
	void SaveData(PlayerData& player);
	int DeleteData(PlayerData& data); // Delete data func
	void LoadData(PlayerData& data);

	std::string playerFilePath = "PlayerInfo.txt";
};
#endif // FILEMANAGER_H
