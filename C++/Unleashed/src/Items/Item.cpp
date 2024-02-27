//
// Base Item Implementation

#include "Item Headers/Item.h"


std::string Item::GetItemName()
{
	return itemName;
}

std::string Item::GetItemDescription()
{
	return itemDescription;
}

int Item::GetItemCount()
{
	return itemCount;
}

void Item::SetItemName(std::string newName)
{
	itemName = newName;
	validItem = true; // If we've set the name, consider the object no longer pseudo
}

void Item::SetItemDescription(std::string newDescription)
{
	itemDescription = newDescription;
}

void Item::SetItemCount(int setTo)
{
	itemCount = setTo;

	if (itemCount < 0) itemCount = 0; // An "Item" count should never be negative
}

void Item::ChangeItemCount(int changeBy)
{
	itemCount += changeBy;

	if (itemCount < 0) itemCount = 0; // An "Item" count should never be negative
}

bool Item::IsValid()
{
	return validItem ? true : false;
}

void Item::SetValid(bool set)
{
	validItem = set;
}

void Item::SaveItem()
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

		writer.Key("itemFilePath");
		writer.String(this->filePath.c_str());

		writer.Key("itemCount");
		writer.Int(this->itemCount);

		writer.Key("itemIsValid");
		writer.Bool(this->IsValid());

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

Item* Item::LoadItem()
{
	Item* item = new Item();

	std::ifstream fileStream(this->filePath, std::ios::in);

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
		const rapidjson::Value& itemNameValue = document["itemName"];
		const rapidjson::Value& itemDescriptionValue = document["itemDescription"];
		const rapidjson::Value& itemCount = document["itemCount"];
		const rapidjson::Value& itemIsValid = document["itemIsValid"];
		const rapidjson::Value& itemFilePath = document["itemFilePath"];

		if (itemNameValue.IsString() && itemDescriptionValue.IsString() && itemFilePath.IsString()) {
			item->itemName = itemNameValue.GetString();
			item->itemDescription = itemDescriptionValue.GetString();
			item->filePath = itemFilePath.GetString();
		}
		else 
		{
			throw std::runtime_error("Invalid JSON format");
		}

		if (itemCount.IsInt())
		{
			item->itemCount = itemCount.GetInt();
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

void Item::DeleteItem()
{
	try
	{
		std::remove(this->filePath.c_str());
	}
	catch (...)
	{
	}
}