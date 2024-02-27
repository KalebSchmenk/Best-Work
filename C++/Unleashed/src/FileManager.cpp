//
// Player data file manager implementation

#include "FileManager.h"
#include <filesystem>


// Saves player data to .dat file
void DataManager::SaveData(PlayerData& data)
{
	std::remove(playerFilePath.c_str()); // Clear path for new file

	std::ofstream f(playerFilePath, std::ios::out);

	//_______________
	// from rapidJSON
	//
	if (f.is_open())
	{
		rapidjson::StringBuffer s;
		rapidjson::Writer<rapidjson::StringBuffer> writer(s);

		writer.StartObject();

		writer.Key("playerName");
		writer.String(data.playerName.c_str());

		writer.Key("playerLevel");
		writer.Int(data.playerLevel);

		writer.Key("playerHealth");
		writer.Int(data.playerHealth);

		writer.Key("playerCoins");
		writer.Int(data.coins.GetItemCount());

		writer.EndObject();

		f << s.GetString() << std::endl;
		f.close();
	}
	else
	{
		throw std::invalid_argument("Couldn't open a new file");
	}
	//
	// from rapidJSON
	//_______________

	data.playerSword.SaveItem();
	data.itemSlotOne.SaveItem();
	data.itemSlotTwo.SaveItem();
	data.itemSlotThree.SaveItem();
	data.itemSlotFour.SaveItem();
	data.itemSlotFive.SaveItem();
	data.itemSlotSix.SaveItem();
}

// Deletes player .dat data file
int DataManager::DeleteData(PlayerData& data)
{
	try
	{
		std::remove(playerFilePath.c_str());
		data.playerSword.DeleteItem();
		data.itemSlotOne.DeleteItem();
		data.itemSlotTwo.DeleteItem();
		data.itemSlotThree.DeleteItem();
		data.itemSlotFour.DeleteItem();
		data.itemSlotFive.DeleteItem();
		data.itemSlotSix.DeleteItem();

		data.playerName = "null";
		return 1;
	}
	catch (...)
	{
		return 0;
	}
}

// Loads data
void DataManager::LoadData(PlayerData& data)
{
	std::ifstream fileStream(playerFilePath, std::ios::in);

	//_______________
	// from rapidJSON
	//
	if (!fileStream.is_open())
	{
		throw std::invalid_argument("Couldn't open the file for reading");
	}

	// Get the file size
	fileStream.seekg(0, std::ios::end);
	std::streamsize fileSize = fileStream.tellg();
	fileStream.seekg(0, std::ios::beg);

	// Read the entire file into a buffer
	std::vector<char> buffer(static_cast<size_t>(fileSize));
	fileStream.read(buffer.data(), fileSize);

	// Close the file
	fileStream.close();

	// Parse the JSON data
	rapidjson::Document document;
	document.Parse(buffer.data());

	if (document.HasParseError())
	{
		throw std::runtime_error("Error parsing JSON");
	}

	// Extract values from the JSON document
	if (document.IsObject()) {
		const rapidjson::Value& playerName = document["playerName"];
		const rapidjson::Value& playerLevel = document["playerLevel"];
		const rapidjson::Value& playerHealth = document["playerHealth"];
		const rapidjson::Value& playerCoins = document["playerCoins"];

		if (playerName.IsString()) {
			data.playerName = playerName.GetString();
		}
		else
		{
			throw std::runtime_error("Invalid JSON format");
		}

		if (playerLevel.IsInt() && playerHealth.IsInt() && playerCoins.IsInt())
		{
			data.playerLevel = playerLevel.GetInt();
			data.playerHealth = playerHealth.GetInt();
			data.coins.SetItemCount(playerCoins.GetInt());
		}
		else
		{
			throw std::runtime_error("Invalid JSON format");
		}
	}
	else
	{
		throw std::runtime_error("Invalid JSON format");
	}
	//
	// from rapidJSON
	//_______________


	// Save Inventory
	Item* sword = data.playerSword.LoadItem();
	Sword* newSword = dynamic_cast<Sword*>(sword);
	data.playerSword = *newSword;

	data.itemSlotOne = *data.itemSlotOne.LoadItem();
	data.itemSlotTwo = *data.itemSlotTwo.LoadItem();
	data.itemSlotThree = *data.itemSlotThree.LoadItem();
	data.itemSlotFour = *data.itemSlotFour.LoadItem();
	data.itemSlotFive = *data.itemSlotFive.LoadItem();
	data.itemSlotSix = *data.itemSlotSix.LoadItem();


	// If the name or health are invalid arguments, default to these values.
	// Does it have to be a try catch? No, but it's interesting to have this be considered an exception
	try
	{
		if (data.playerName.compare("") == 0) throw std::invalid_argument("Name was empty");
	}
	catch (const std::invalid_argument& e)
	{
		data.playerName = "Phoenix";
	}
	try
	{
		if (data.playerHealth == -1) throw std::invalid_argument("Health was never overriden");
	}
	catch (const std::invalid_argument& e)
	{
		data.playerHealth = 15;
	}
}


