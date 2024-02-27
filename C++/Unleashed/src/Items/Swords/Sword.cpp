//
// Base Sword Implementation

#include "Item Headers/Sword Headers/Sword.h"
#include "Enemy Headers/BaseEnemy.h"

Sword::Sword()
{
	SetItemName("Wooden Sword");
	this->filePath = "Sword.txt";
}

int Sword::Attack(BaseEnemy* enemy)
{
	enemy->TakeDamage(damageOutput);
	return damageOutput;
}

void Sword::SaveItem()
{
	DeleteItem(); // Clear path for new file


	std::ofstream f(this->filePath.c_str(), std::ios::out);
	//_______________
	// from rapidJSON
	//
	if (f.is_open())
	{
		rapidjson::StringBuffer s;
		rapidjson::Writer<rapidjson::StringBuffer> writer(s);

		writer.StartObject();

		writer.Key("itemName");
		writer.String(this->itemName.c_str());

		writer.Key("itemDescription");
		writer.String(this->itemDescription.c_str());

		writer.Key("itemCount");
		writer.Int(this->itemCount);

		writer.Key("itemIsValid");
		writer.Bool(this->IsValid());

		writer.Key("itemFilePath");
		writer.String(this->filePath.c_str());

		writer.Key("swordDamageOutput");
		writer.Int(this->damageOutput);

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
}

Item* Sword::LoadItem()
{
	Sword* item = new Sword();

	std::ifstream fileStream(this->filePath, std::ios::in);
	
	//_______________
	// from rapidJSON
	//
	if (!fileStream.is_open()) {
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

	if (document.HasParseError()) {
		throw std::runtime_error("Error parsing JSON");
	}

	// Extract values from the JSON document
	if (document.IsObject()) {
		const rapidjson::Value& itemNameValue = document["itemName"];
		const rapidjson::Value& itemDescriptionValue = document["itemDescription"];
		const rapidjson::Value& itemCount = document["itemCount"];
		const rapidjson::Value& itemIsValid = document["itemIsValid"];
		const rapidjson::Value& itemFilePath = document["itemFilePath"];
		const rapidjson::Value& swordDamageOutput = document["swordDamageOutput"];

		if (itemNameValue.IsString() && itemDescriptionValue.IsString() && itemFilePath.IsString()) {
			item->SetItemName(itemNameValue.GetString());
			item->SetItemDescription(itemDescriptionValue.GetString());
			item->filePath = itemFilePath.GetString();
		}
		else
		{
			throw std::runtime_error("Invalid JSON format");
		}

		if (itemCount.IsInt())
		{
			item->SetItemCount(itemCount.GetInt());
		}
		else
		{
			throw std::runtime_error("Invalid JSON format");
		}

		if (itemIsValid.IsBool())
		{
			item->SetValid(itemIsValid.GetBool());
		}
		else
		{
			throw std::runtime_error("Invalid JSON format");
		}

		if (swordDamageOutput.IsInt())
		{
			item->damageOutput = swordDamageOutput.GetInt();
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

	return item;
}
