//
// Coin Implementation

#include "Item Headers/Coin.h"

Coin::Coin(int amount)
{
	SetItemName("Coin");
	SetItemCount(amount);
}
Coin::Coin()
{
	SetItemName("Coin");
	SetItemCount(0);
}

// TODO
Item Coin::Buy()
{
	std::cout << "You bought something! TODO";
	Coin coin(10);
	return coin;
}