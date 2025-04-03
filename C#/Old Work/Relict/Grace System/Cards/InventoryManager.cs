using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Card data struct
    [Serializable]
    public struct MajorCard
    {
        public MajorCardSO cardSO;
        public MajorCardBase cardScript;
        public GameObject cardObj;

        // Sets null references and destroys card obj
        public void DestroyCard()
        {
            cardSO = null;
            cardScript = null;
            if (cardObj != null) Destroy(cardObj);
        }
    }

    // Major Cards
    public List<MajorCard> weaponMajorCards = new List<MajorCard>();
    public List<MajorCard> movementMajorCards = new List<MajorCard>();
    public List<MajorCard> specialMajorCards = new List<MajorCard>();
    public List<MajorCard> supportMajorCards = new List<MajorCard>();
    public List<MajorCard> ultimateMajorCards = new List<MajorCard>();
    public List<MajorCard> passiveMajorCards = new List<MajorCard>();

    // Minor Cards
    public MinorCardBase[] minorCards = new MinorCardBase[4];
    public MinorCardSO[] minorCardSOs = new MinorCardSO[4];

    // Reference to Debug UI Manager
    public DebugUIManager debugUIManager;

    // Player Stats Reference
    public PlayerStats playerStats;

    // Flag for if inventory has generated minor cards
    [NonSerialized] public bool generatedMinorCards = false;

    // Flag for if player has fool cards
    [NonSerialized] public static bool ownsFoolCards = false;

    #region Event Subscriptions
    // Add coins on enemy kill func subscription
    private void OnEnable()
    {
        PlayerEvents.OnEnemyKill += AddCoins;
    }

    // Add coins on enemy kill func unsubscription
    private void OnDisable()
    {
        PlayerEvents.OnEnemyKill -= AddCoins;
    }
    #endregion

    private void Start()
    {
        if (!generatedMinorCards) CreateMinorCards();
        if (!ownsFoolCards) AddFoolCards();
    }

    // Creates all minor cards and adds to inventory
    public void CreateMinorCards()
    {
        generatedMinorCards = true;
        for (int i = 0; i < minorCards.Length; i++)
        {
            MinorCardSO cardSO = DeckManager.instance.listOfMinorCards[i];
            GameObject cardObj = Instantiate(cardSO.cardPrefab);
            cardObj.name = cardSO.name;
            cardObj.transform.parent = this.transform;
            minorCardSOs[i] = cardSO;
            minorCards[i] = cardObj.GetComponent<MinorCardBase>();
        }
    }

    // Adds all fool cards
    public void AddFoolCards()
    {
        var foolCards = DeckManager.instance.GetAllCardsOfArcanaType(MajorCardSO.MajorCardArcanaType.Fool);

        foreach (var foolCard in foolCards)
        {
            AddCard(foolCard);
        }

        ownsFoolCards = true;
    }

    // Adds a major card passed in
    public int AddCard(MajorCardSO card)
    {
        print("In add card");

        MajorCard buildACard; // Card to be built

        switch (card.cardType)
        {
            case MajorCardSO.MajorCardType.Weapon:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) weaponMajorCards.Add(buildACard);
                if (weaponMajorCards.Count == 1) weaponMajorCards[0].cardScript.OnAdd(); // If first card, call on add func
                return 1;

            case MajorCardSO.MajorCardType.Movement:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) movementMajorCards.Add(buildACard);
                if (movementMajorCards.Count == 1) movementMajorCards[0].cardScript.OnAdd(); // If first card, call on add func
                return 1;

            case MajorCardSO.MajorCardType.Special:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) specialMajorCards.Add(buildACard);
                if (specialMajorCards.Count == 1) specialMajorCards[0].cardScript.OnAdd(); // If first card, call on add func
                return 1;

            case MajorCardSO.MajorCardType.Support:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) supportMajorCards.Add(buildACard);
                if (supportMajorCards.Count == 1) supportMajorCards[0].cardScript.OnAdd(); // If first card, call on add func
                return 1;

            case MajorCardSO.MajorCardType.Ultimate:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) ultimateMajorCards.Add(buildACard);
                if (ultimateMajorCards.Count == 1) ultimateMajorCards[0].cardScript.OnAdd(); // If first card, call on add func
                return 1;

            case MajorCardSO.MajorCardType.Passive:
                buildACard = BuildMajorCard(card);
                if (buildACard.cardObj != null) passiveMajorCards.Add(buildACard);
                if (passiveMajorCards.Count == 1) passiveMajorCards[0].cardScript.OnAdd();
                if (passiveMajorCards.Count == 2) passiveMajorCards[1].cardScript.OnAdd();
                return 1;
        }

        return 0;
    }

    // Add card by ID
    public int AddCard(int id)
    {
        if (id == -1)
        {
            Debug.LogWarning("Add card by ID was passed a -1. If this was intentional, disregard");
            return 0;
        }

        var majorCards = DeckManager.instance.listOfMajorCards;
        foreach (var card in majorCards)
        {
            if (card.cardID == id)
            {
                AddCard(card);
                return 1;
            }
        }

        Debug.LogError("Tried to add card by ID but couldn't find a matching card ID. Passed in ID was " + id);
        return 0;
    }

    // Destroys a card by type and index
    public void DestroyCard(MajorCardSO card)
    {
        switch (card.cardType)
        {
            case MajorCardSO.MajorCardType.Weapon:
                for (int i = 0; i < weaponMajorCards.Count; i++)
                {
                    if (card == weaponMajorCards[i].cardSO)
                    {
                        weaponMajorCards[i].DestroyCard();
                        weaponMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the card that is active is turned on. If not, turn it on
                        if (weaponMajorCards.Count > 0 && weaponMajorCards[0].cardScript.isActive == false) weaponMajorCards[0].cardScript.OnAdd();
                    }
                }
                break;

            case MajorCardSO.MajorCardType.Movement:
                for (int i = 0; i < movementMajorCards.Count; i++)
                {
                    if (card == movementMajorCards[i].cardSO)
                    {
                        movementMajorCards[i].DestroyCard();
                        movementMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the card that is active is turned on. If not, turn it on
                        if (movementMajorCards.Count > 0 && movementMajorCards[0].cardScript.isActive == false) movementMajorCards[0].cardScript.OnAdd();
                    }
                }
                break;

            case MajorCardSO.MajorCardType.Special:
                for (int i = 0; i < specialMajorCards.Count; i++)
                {
                    if (card == specialMajorCards[i].cardSO)
                    {
                        specialMajorCards[i].DestroyCard();
                        specialMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the card that is active is turned on. If not, turn it on
                        if (specialMajorCards.Count > 0 && specialMajorCards[0].cardScript.isActive == false) specialMajorCards[0].cardScript.OnAdd();
                    }
                }
                break;

            case MajorCardSO.MajorCardType.Support:
                for (int i = 0; i < supportMajorCards.Count; i++)
                {
                    if (card == supportMajorCards[i].cardSO)
                    {
                        supportMajorCards[i].DestroyCard();
                        supportMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the card that is active is turned on. If not, turn it on
                        if (supportMajorCards.Count > 0 && supportMajorCards[0].cardScript.isActive == false) supportMajorCards[0].cardScript.OnAdd();
                    }
                }
                break;

            case MajorCardSO.MajorCardType.Ultimate:
                for (int i = 0; i < ultimateMajorCards.Count; i++)
                {
                    if (card == ultimateMajorCards[i].cardSO)
                    {
                        ultimateMajorCards[i].DestroyCard();
                        ultimateMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the card that is active is turned on. If not, turn it on
                        if (ultimateMajorCards.Count > 0 && ultimateMajorCards[0].cardScript.isActive == false) ultimateMajorCards[0].cardScript.OnAdd();
                    }
                }
                break;

            case MajorCardSO.MajorCardType.Passive:
                for (int i = 0; i < passiveMajorCards.Count; i++)
                {
                    if (card == passiveMajorCards[i].cardSO)
                    {
                        passiveMajorCards[i].DestroyCard();
                        passiveMajorCards.RemoveAt(i);

                        // After removing the card, we need to check if the cards that are active is turned on. If not, turn it/them on
                        // For index 0 and 1 for passives
                        if (passiveMajorCards.Count > 0 && passiveMajorCards[0].cardScript.isActive == false) passiveMajorCards[0].cardScript.OnAdd();
                        if (passiveMajorCards.Count > 1 && passiveMajorCards[1].cardScript.isActive == false) passiveMajorCards[1].cardScript.OnAdd();
                    }
                }
                break;
        }
    }

    // Builds a given Major card and inserts it into the struct container and returns it
    private MajorCard BuildMajorCard(MajorCardSO card)
    {
        print("In build-a-card");
        var cardRef = new MajorCard();

        if (card.cardPrefab == null) // Guard clause
        {
            Debug.LogWarning("Card prefab was null. Build-a-card failed. Exiting...");
            return cardRef;
        }

        cardRef.cardSO = card;
        cardRef.cardObj = Instantiate(cardRef.cardSO.cardPrefab);
        cardRef.cardObj.transform.position = this.transform.position;
        cardRef.cardScript = cardRef.cardObj.GetComponent<MajorCardBase>();
        cardRef.cardObj.name = cardRef.cardSO.name;
        cardRef.cardObj.transform.parent = this.transform;

        print("Added " + cardRef.cardSO.name + " to the player's inventory");
        return cardRef;
    }
    
    // Destroys card refs and gameobjects
    // We do this by reference and not by value to ensure we are editing the correct card
    public void DestroyMajorCard(ref MajorCard cardToDestroy)
    {
        cardToDestroy.cardSO = null;
        cardToDestroy.cardScript = null;
        if (cardToDestroy.cardObj != null) Destroy(cardToDestroy.cardObj);
    }

    // Returns a major card at index
    public MajorCard GetMajorCard(MajorCardSO.MajorCardType cardType, int index)
    {
        switch (cardType)
        {
            case MajorCardSO.MajorCardType.Weapon:
                if (weaponMajorCards.Count > index)
                {
                    return weaponMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            case MajorCardSO.MajorCardType.Movement:
                if (movementMajorCards.Count > index)
                {
                    return movementMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            case MajorCardSO.MajorCardType.Special:
                if (specialMajorCards.Count > index)
                {
                    return specialMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            case MajorCardSO.MajorCardType.Support:
                if (supportMajorCards.Count > index)
                {
                    return supportMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            case MajorCardSO.MajorCardType.Ultimate:
                if (ultimateMajorCards.Count > index)
                {
                    return ultimateMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            case MajorCardSO.MajorCardType.Passive:
                if (passiveMajorCards.Count > index)
                {
                    return passiveMajorCards[index];
                }
                else
                {
                    return new MajorCard();
                }

            default:
                return new MajorCard();
        }
    }

    // Returns list of major cards
    public List<MajorCard> GetMajorCards(MajorCardSO.MajorCardType cardType)
    {
        switch (cardType)
        {
            case MajorCardSO.MajorCardType.Weapon:
                return weaponMajorCards;

            case MajorCardSO.MajorCardType.Movement:
                return movementMajorCards;

            case MajorCardSO.MajorCardType.Special:
                return specialMajorCards;

            case MajorCardSO.MajorCardType.Support:
                return supportMajorCards;

            case MajorCardSO.MajorCardType.Ultimate:
                return ultimateMajorCards;

            case MajorCardSO.MajorCardType.Passive:
                return passiveMajorCards;

            default:
                return new List<MajorCard>();
        }
    }

    // Clears all major cards from the inventory
    public void ClearMajorCards()
    {
        for (int i = 0; i < weaponMajorCards.Count; i++)
        {
            weaponMajorCards[i].DestroyCard();
        }
        weaponMajorCards.Clear();

        for (int i = 0; i < movementMajorCards.Count; i++)
        {
            movementMajorCards[i].DestroyCard();
        }
        movementMajorCards.Clear();

        for (int i = 0; i < specialMajorCards.Count; i++)
        {
            specialMajorCards[i].DestroyCard();
        }
        specialMajorCards.Clear();

        for (int i = 0; i < supportMajorCards.Count; i++)
        {
            supportMajorCards[i].DestroyCard();
        }
        supportMajorCards.Clear();

        for (int i = 0; i < ultimateMajorCards.Count; i++)
        {
            ultimateMajorCards[i].DestroyCard();
        }
        ultimateMajorCards.Clear();

        for (int i = 0; i < passiveMajorCards.Count; i++)
        {
            passiveMajorCards[i].DestroyCard();
        }
        passiveMajorCards.Clear();
    }  

    // Increase given card type index
    // Puts current card at back of list
    // Tells cards they are being added or removed
    public void ChangeCurrentCardIndex(int cardType)
    {
        MajorCard tempCard;

        //Weapon = 0,
        //Movement = 1,
        //Special = 2,
        //Support = 3,
        //Ultimate = 4,
        //Passive card 1 = 5
        //Passive card 2 = 6
        // Why dont they just support enums man

        switch (cardType)
        {
            case (int)MajorCardSO.MajorCardType.Weapon:
                if (weaponMajorCards.Count <= 1) return; // Don't change if player only has 1 card or no cards

                weaponMajorCards[0].cardScript.OnRemove();
                tempCard = weaponMajorCards[0];
                weaponMajorCards.RemoveAt(0);
                weaponMajorCards[0].cardScript.OnAdd();
                weaponMajorCards.Add(tempCard);
                break;

            case (int)MajorCardSO.MajorCardType.Movement:
                if (movementMajorCards.Count <= 1) return; // Don't change if player only has 1 card or no cards

                movementMajorCards[0].cardScript.OnRemove();
                tempCard = movementMajorCards[0];
                movementMajorCards.RemoveAt(0);
                movementMajorCards[0].cardScript.OnAdd();
                movementMajorCards.Add(tempCard);
                break;

            case (int)MajorCardSO.MajorCardType.Special:
                if (specialMajorCards.Count <= 1) return; // Don't change if player only has 1 card or no cards

                specialMajorCards[0].cardScript.OnRemove();
                tempCard = specialMajorCards[0];
                specialMajorCards.RemoveAt(0);
                specialMajorCards[0].cardScript.OnAdd();
                specialMajorCards.Add(tempCard);
                break;

            case (int)MajorCardSO.MajorCardType.Support:
                if (supportMajorCards.Count <= 1) return; // Don't change if player only has 1 card or no cards

                supportMajorCards[0].cardScript.OnRemove();
                tempCard = supportMajorCards[0];
                supportMajorCards.RemoveAt(0);
                supportMajorCards[0].cardScript.OnAdd();
                supportMajorCards.Add(tempCard);
                break;

            case (int)MajorCardSO.MajorCardType.Ultimate:
                if (ultimateMajorCards.Count <= 1) return; // Don't change if player only has 1 card or no cards

                ultimateMajorCards[0].cardScript.OnRemove();
                tempCard = ultimateMajorCards[0];
                ultimateMajorCards.RemoveAt(0);
                ultimateMajorCards[0].cardScript.OnAdd();
                ultimateMajorCards.Add(tempCard);
                break;

            case (int)MajorCardSO.MajorCardType.Passive: // First passive card
                if (passiveMajorCards.Count <= 1 || passiveMajorCards.Count == 2) return; // Don't change if player only has 1 or 2 cards or no cards
                MajorCard passiveTemp = new MajorCard();
                
                // Tell first card to prepare for deactivation
                passiveMajorCards[0].cardScript.OnRemove();

                // Save first card and third card
                tempCard = passiveMajorCards[0];
                passiveTemp = passiveMajorCards[2];

                // Set first card to third card
                passiveMajorCards[0] = passiveTemp;

                // Set new first card active
                passiveMajorCards[0].cardScript.OnAdd(); // Tell the passive that it is being activated

                // Pop old third card (now located at index 0)
                passiveMajorCards.RemoveAt(2);

                // Add old first card to back of list
                passiveMajorCards.Add(tempCard);
                break;

            case 6: // Second passive card
                if (passiveMajorCards.Count <= 1 || passiveMajorCards.Count == 2) return; // Don't change if player only has 1 or 2 cards
                MajorCard passiveTempTwo = new MajorCard();

                // Tell second card to prepare for deactivation
                passiveMajorCards[1].cardScript.OnRemove();

                // Save second card and third card
                tempCard = passiveMajorCards[1];
                passiveTempTwo = passiveMajorCards[2];

                // Set second card to third card
                passiveMajorCards[1] = passiveTempTwo;

                // Set new second card active
                passiveMajorCards[1].cardScript.OnAdd(); // Tell the passive that it is being activated

                // Pop old third card (now located at index 1)
                passiveMajorCards.RemoveAt(2);

                // Add old first card to back of list
                passiveMajorCards.Add(tempCard);
                break;
        }
    }

    // Money Adder
    public void AddCoins(int amountToAdd)
    {
        playerStats.Cash += amountToAdd;
    }
}