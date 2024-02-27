using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TarotReadingManager : MonoBehaviour
{

    #region Arcana_Refs
    // Major Arcana Refs
    [Header("Major Arcana Refs")]
    [Header("Slot One")]
    public Button majorArcanaSlotOneButton;
    private Image majorArcanaSlotOneImage;
    public TMP_Text majorArcanaSlotOneNameText;
    [Header("Slot Two")]
    public Button majorArcanaSlotTwoButton;
    private Image majorArcanaSlotTwoImage;
    public TMP_Text majorArcanaSlotTwoNameText;

    [Header("")]
    public GameObject inGamePanel;
    public GameObject confirmSelectionButton;
    public TMP_Text selectedMajorArcanaDescription;
    public TMP_Text selectedMajorArcanaNameText;
    public string[] dietyDescriptions;

    [Header("")]
    // Minor Arcana Refs
    [Header("Minor Arcana Refs")]
    [Header("Slot One")]
    public Button minorArcanaSlotOneButton;
    private Image minorArcanaSlotOneImage;
    public TMP_Text minorArcanaSlotOneText;
    [Header("Slot Two")]
    public Button minorArcanaSlotTwoButton;
    private Image minorArcanaSlotTwoImage;
    public TMP_Text minorArcanaSlotTwoText;
    [Header("Slot Three")]
    public Button minorArcanaSlotThreeButton;
    private Image minorArcanaSlotThreeImage;
    public TMP_Text minorArcanaSlotThreeText;
    [Header("")]
    public TMP_Text selectedMinorArcanaDescriptionText;

    [Header("Button Highlights")]
    public Image buttonHighlight1;
    public Image buttonHighlight2;
    public Image buttonHighlight3;
    public Image buttonHighlight4;
    public Image buttonHighlight5;
    public Image buttonHighlight6;
    public Image buttonHighlight7;
    public Image buttonHighlight8;

    private MajorCardSO selectedAbilityCard;
    private string selectedMinorCard1;
    private string selectedMinorCard2;
    private string lastSelectedMinorCardSlot;

    #endregion

    #region Grace_Refs
    [Header("Grace Refs")]
    [Header("Slot One")]
    public Button graceSlotOneButton;
    private Image graceSlotOneImage;
    public TextMeshProUGUI graceSlotOneName;
    public TextMeshProUGUI graceSlotOneDescription;
    private MajorCardSO graceOneMajorCard;

    [Header("Slot Two")]
    public Button graceSlotTwoButton;
    private Image graceSlotTwoImage;
    public TextMeshProUGUI graceSlotTwoName;
    public TextMeshProUGUI graceSlotTwoDescription;
    private MajorCardSO graceTwoMajorCard;

    [Header("Slot Three")]
    public Button graceSlotThreeButton;
    private Image graceSlotThreeImage;
    public TextMeshProUGUI graceSlotThreeName;
    public TextMeshProUGUI graceSlotThreeDescription;
    private MajorCardSO graceThreeMajorCard;
    [Header("")]

    private MajorCardSO slotOneCardOne;
    private MajorCardSO slotOneCardTwo;
    private MajorCardSO slotOneCardThree;
    private MajorCardSO slotTwoCardOne;
    private MajorCardSO slotTwoCardTwo;
    private MajorCardSO slotTwoCardThree;
    #endregion

    #region Possible_Cards_For_Slots
    // Major Arcana Current Possible Card
    List<MajorCardSO> slotOnePossibleCards = new List<MajorCardSO>();
    List<MajorCardSO> slotTwoPossibleCards = new List<MajorCardSO>();
    #endregion


    [Header("Confirm Selection Refs")]
    [SerializeField] GameObject confirmSelectionParent;
    [SerializeField] Button confirmSelectionNoButton;
    int minorCardsSelected = 0;
    int currentSelectedSlot = -1;

    //bool selectedMajorArcana = false;
    //bool selectedGrace = false;
    string minorCardSlotOne = "";
    string minorCardSlotTwo = "";
    string minorCardSlotThree = "";

    [Header("")]
    public GameObject openMenu;
    public GameObject tarotReadingMenu;
    public Image selectedGraceCard;
    public Sprite defaultSelection;
    public InventoryManager inventory;


    //Audio code
    AudioSource audioSource;
    public AudioClip minorcardSound;
    public AudioClip majorcardSound;
    public static bool pause; // Used by the audiolistener to determine whether the game is paused or not

    // Starts a tarot reading
    public void StartTarotReading()
    {
        if (GameManager.instance.isMenuOpen) return; // A menu is already open
        GameManager.instance.isMenuOpen = true;

        PauseTime();

        inGamePanel.SetActive(false);

        tarotReadingMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(majorArcanaSlotOneButton.gameObject);

        // Major ref getters
        majorArcanaSlotOneImage = majorArcanaSlotOneButton.GetComponent<Image>();
        majorArcanaSlotTwoImage = majorArcanaSlotTwoButton.GetComponent<Image>();

        // Minor ref getters
        minorArcanaSlotOneImage = minorArcanaSlotOneButton.GetComponent<Image>();
        minorArcanaSlotTwoImage = minorArcanaSlotTwoButton.GetComponent<Image>();
        minorArcanaSlotThreeImage = minorArcanaSlotThreeButton.GetComponent<Image>();

        // Grace ref getters
        graceSlotOneImage = graceSlotOneButton.GetComponent<Image>();
        graceSlotTwoImage = graceSlotTwoButton.GetComponent<Image>();
        graceSlotThreeImage = graceSlotThreeButton.GetComponent<Image>();

        PreDetermineCards();
    }

    // Pauses time
    private void PauseTime()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameManager.instance.player.GetComponent<PlayerController>().enabled = false;
        GameManager.instance.player.GetComponent<PlayerWeaponController>().enabled = false;

        Time.timeScale = 0;
        GameManager.instance.cinemachineCam.SetActive(false);

        AudioListener.pause = true; // Disables audio during tarot reading

        audioSource = GetComponent<AudioSource>(); // Audio source code
    }

    // Unpauses time and closes tarot reading
    private void CloseTarotReading()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inGamePanel.SetActive(true);

        GameManager.instance.player.GetComponent<PlayerController>().enabled = true;
        GameManager.instance.player.GetComponent<PlayerWeaponController>().enabled = true;

        Time.timeScale = 1f;
        GameManager.instance.cinemachineCam.SetActive(true);

        tarotReadingMenu.SetActive(false);

        GameManager.instance.currentHighPriestessAnimator.SetBool("Reading", false);
        GameManager.instance.currentHighPriestessAnimator = null;

        GameManager.instance.isMenuOpen = false;

        #region Reset_All_Refs
        // Lots of resets here

        slotOnePossibleCards = null;
        slotTwoPossibleCards = null;

        majorArcanaSlotOneImage.sprite = defaultSelection;
        majorArcanaSlotTwoImage.sprite = defaultSelection;
        minorArcanaSlotOneImage.sprite = defaultSelection;
        minorArcanaSlotTwoImage.sprite = defaultSelection;
        minorArcanaSlotThreeImage.sprite = defaultSelection;
        selectedGraceCard.sprite = defaultSelection;

        graceOneMajorCard = null; 
        graceTwoMajorCard = null; 
        graceThreeMajorCard = null;

        graceSlotOneName.text = "Empty";
        graceSlotTwoName.text = "Empty";
        graceSlotThreeName.text = "Empty";

        graceSlotOneDescription.text = "Empty";
        graceSlotTwoDescription.text = "Empty";
        graceSlotThreeDescription.text = "Empty";

        //selectedGrace = false;
        //selectedMajorArcana = false;

        minorCardSlotOne = "";
        minorCardSlotTwo = "";
        minorCardSlotThree = "";

        minorCardsSelected = 0;

        selectedAbilityCard = null;
        selectedMinorCard1 = null;
        selectedMinorCard2 = null;
        buttonHighlight1.gameObject.SetActive(false);
        buttonHighlight2.gameObject.SetActive(false);
        buttonHighlight3.gameObject.SetActive(false);
        buttonHighlight4.gameObject.SetActive(false);
        buttonHighlight5.gameObject.SetActive(false);
        buttonHighlight6.gameObject.SetActive(false);
        buttonHighlight7.gameObject.SetActive(false);
        buttonHighlight8.gameObject.SetActive(false);
        #endregion

        AudioListener.pause = false; // Allows audio to play after tarot reading
    }

    // Pre Determine All Random Cards
    void PreDetermineCards()
    {
        var zone = LevelManager.instance.GetCurrentZone() as TarotReadingZone; // Assuming we are in a tarot reading zone
        var deckManager = DeckManager.instance;

        var tarotPool = zone.TarotCardPool;
        List<MajorCardSO.MajorCardArcanaType> availableTypes = new List<MajorCardSO.MajorCardArcanaType>();

        foreach (var item in tarotPool)
        {
            if (!availableTypes.Contains(item.arcanaType))
            {
                availableTypes.Add(item.arcanaType);
            }
        }
        int slotOneRandom = UnityEngine.Random.Range(0, availableTypes.Count);
        int slotTwoRandom = -1;

        do
        {
            slotTwoRandom = UnityEngine.Random.Range(0, availableTypes.Count);
        } while (slotOneRandom == slotTwoRandom);

        // Switches that Determine the Major Arcana (The Devil, The Moon, etc.)
        #region Switch_One
        bool addThe = false;
        switch (availableTypes[slotOneRandom])
        {
            case MajorCardSO.MajorCardArcanaType.Strength:
                majorArcanaSlotOneNameText.text = "Strength";
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;

            case MajorCardSO.MajorCardArcanaType.Death:
                majorArcanaSlotOneImage.sprite = deckManager.deathCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;

            case MajorCardSO.MajorCardArcanaType.Devil:
                majorArcanaSlotOneImage.sprite = deckManager.devilCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        addThe = true;
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;

            case MajorCardSO.MajorCardArcanaType.Sun:
                majorArcanaSlotOneImage.sprite = deckManager.sunCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        addThe = true;
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;
            case MajorCardSO.MajorCardArcanaType.Moon:
                majorArcanaSlotOneImage.sprite = deckManager.moonCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        addThe = true;
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;
            case MajorCardSO.MajorCardArcanaType.World:
                majorArcanaSlotOneImage.sprite = deckManager.worldCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotOneRandom])
                    {
                        addThe = true;
                        slotOnePossibleCards = item.cards;
                        break;
                    }
                }
                break;
        }

        if (addThe)
        {
            majorArcanaSlotOneNameText.text = "The " + availableTypes[slotOneRandom].ToString();
        }
        else
        {
            majorArcanaSlotOneNameText.text = availableTypes[slotOneRandom].ToString();
        }
        addThe = false;
        #endregion

        #region Switch_Two
        switch (availableTypes[slotTwoRandom])
        {
            case MajorCardSO.MajorCardArcanaType.Strength:
                majorArcanaSlotTwoImage.sprite = deckManager.strengthCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break;

            case MajorCardSO.MajorCardArcanaType.Death:
                majorArcanaSlotTwoImage.sprite = deckManager.deathCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break;

            case MajorCardSO.MajorCardArcanaType.Devil:
                majorArcanaSlotTwoImage.sprite = deckManager.devilCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        addThe = true;
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break; ;

            case MajorCardSO.MajorCardArcanaType.Sun:
                majorArcanaSlotTwoImage.sprite = deckManager.sunCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        addThe = true;
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break;
            case MajorCardSO.MajorCardArcanaType.Moon:
                majorArcanaSlotTwoImage.sprite = deckManager.moonCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        addThe = true;
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break;
            case MajorCardSO.MajorCardArcanaType.World:
                majorArcanaSlotTwoImage.sprite = deckManager.worldCard;
                foreach (var item in tarotPool)
                {
                    if (item.arcanaType == availableTypes[slotTwoRandom])
                    {
                        addThe = true;
                        slotTwoPossibleCards = item.cards;
                        break;
                    }
                }
                break;
        }

        if (addThe)
        {
            majorArcanaSlotTwoNameText.text = "The " + availableTypes[slotTwoRandom].ToString();
        }
        else
        {
            majorArcanaSlotTwoNameText.text = availableTypes[slotTwoRandom].ToString();
        }
        addThe = false;
        #endregion

        // Updating Text Objects
        selectedMajorArcanaDescription.text = "";

        // Determining what abilities each major card can show
        int graceOneNum;
        int graceTwoNum;
        int graceThreeNum;

        // Major Arcana Slot 1 Abilities
        graceOneNum = UnityEngine.Random.Range(0, slotOnePossibleCards.Count);
        graceTwoNum = UnityEngine.Random.Range(0, slotOnePossibleCards.Count);
        graceThreeNum = UnityEngine.Random.Range(0, slotOnePossibleCards.Count);

        slotOneCardOne = slotOnePossibleCards[graceOneNum];
        slotOneCardTwo = slotOnePossibleCards[graceTwoNum];
        slotOneCardThree = slotOnePossibleCards[graceThreeNum];

        // Major Arcana Slot 2 Abilities
        graceOneNum = UnityEngine.Random.Range(0, slotTwoPossibleCards.Count);
        graceTwoNum = UnityEngine.Random.Range(0, slotTwoPossibleCards.Count);
        graceThreeNum = UnityEngine.Random.Range(0, slotTwoPossibleCards.Count);

        slotTwoCardOne = slotTwoPossibleCards[graceOneNum];
        slotTwoCardTwo = slotTwoPossibleCards[graceTwoNum];
        slotTwoCardThree = slotTwoPossibleCards[graceThreeNum];

        // Determining which Minor Arcana will be shown
        slotOneRandom = UnityEngine.Random.Range(1, 5);
        slotTwoRandom = -1;
        int slotThreeRandom = -1;

        do
        {
            slotTwoRandom = UnityEngine.Random.Range(1, 5);
        } while (slotOneRandom == slotTwoRandom);

        do
        {
            slotThreeRandom = UnityEngine.Random.Range(1, 5);
        } while (slotThreeRandom == slotTwoRandom || slotThreeRandom == slotOneRandom);

        // Minor Arcana options 1 - 3 in the switches below
        #region Switch_One
        if (minorCardSlotOne == "")
        {
            switch (slotOneRandom)
            {
                case 1:
                    minorCardSlotOne = "wands";
                    break;

                case 2:
                    minorCardSlotOne = "pentacles";
                    break;

                case 3:
                    minorCardSlotOne = "cups";
                    break;

                case 4:
                    minorCardSlotOne = "swords";
                    break;

                default:
                    Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                    minorArcanaSlotOneImage.sprite = deckManager.defaultCard;
                    break;
            }
        }
        #endregion

        #region Switch_Two
        if (minorCardSlotTwo == "")
        {
            switch (slotTwoRandom)
            {
                case 1:
                    minorCardSlotTwo = "wands";
                    break;

                case 2:
                    minorCardSlotTwo = "pentacles";
                    break;

                case 3:
                    minorCardSlotTwo = "cups";
                    break;

                case 4:
                    minorCardSlotTwo = "swords";
                    break;

                default:
                    Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                    minorArcanaSlotTwoImage.sprite = deckManager.defaultCard;
                    break;
            }
        }
        #endregion

        #region Switch_Three
        if (minorCardSlotThree == "")
        {
            switch (slotThreeRandom)
            {
                case 1:
                    minorCardSlotThree = "wands";
                    break;

                case 2:
                    minorCardSlotThree = "pentacles";
                    break;

                case 3:
                    minorCardSlotThree = "cups";
                    break;

                case 4:
                    minorCardSlotThree = "swords";
                    break;

                default:
                    Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                    minorArcanaSlotThreeImage.sprite = deckManager.defaultCard;
                    break;
            }
        }
        #endregion
    }

    // Sets graces when major arcana is selected;
    public void SelectMajorArcana(int slot)
    {
        //if (selectedMajorArcana) return; // Guard Clause

        // Possible to get dupes here
        if (slot == 1)
        {
            graceOneMajorCard = slotOneCardOne;
            graceTwoMajorCard = slotOneCardTwo;
            graceThreeMajorCard = slotOneCardThree;

            buttonHighlight1.gameObject.SetActive(true);
            buttonHighlight2.gameObject.SetActive(false);

            selectedGraceCard.sprite = graceOneMajorCard.cardImage;
            selectedMajorArcanaNameText.text = majorArcanaSlotOneNameText.text;

            #region description_one_switch
            switch (majorArcanaSlotOneNameText.text)
            {
                case "Strength":
                    selectedMajorArcanaDescription.text = dietyDescriptions[0];
                    break;
                case "Death":
                    selectedMajorArcanaDescription.text = dietyDescriptions[1];
                    break;
                case "The Devil":
                    selectedMajorArcanaDescription.text = dietyDescriptions[2];
                    break;
                case "The Sun":
                    selectedMajorArcanaDescription.text = dietyDescriptions[3];
                    break;
                case "The Moon":
                    selectedMajorArcanaDescription.text = dietyDescriptions[4];
                    break;
                case "The World":
                    selectedMajorArcanaDescription.text = dietyDescriptions[5];
                    break;
            }
            #endregion
        }
        else // Assume slot 2
        {
            graceOneMajorCard = slotTwoCardOne;
            graceTwoMajorCard = slotTwoCardTwo;
            graceThreeMajorCard = slotTwoCardThree;

            buttonHighlight1.gameObject.SetActive(false);
            buttonHighlight2.gameObject.SetActive(true);

            selectedGraceCard.sprite = graceTwoMajorCard.cardImage;
            selectedMajorArcanaNameText.text = majorArcanaSlotTwoNameText.text;

            #region description_two_switch
            switch (majorArcanaSlotTwoNameText.text)
            {
                case "Strength":
                    selectedMajorArcanaDescription.text = dietyDescriptions[0];
                    break;
                case "Death":
                    selectedMajorArcanaDescription.text = dietyDescriptions[1];
                    break;
                case "The Devil":
                    selectedMajorArcanaDescription.text = dietyDescriptions[2];
                    break;
                case "The Sun":
                    selectedMajorArcanaDescription.text = dietyDescriptions[3];
                    break;
                case "The Moon":
                    selectedMajorArcanaDescription.text = dietyDescriptions[4];
                    break;
                case "The World":
                    selectedMajorArcanaDescription.text = dietyDescriptions[5];
                    break;
            }
            #endregion
        }

        graceSlotOneName.text = graceOneMajorCard.name;
        graceSlotTwoName.text = graceTwoMajorCard.name;
        graceSlotThreeName.text = graceThreeMajorCard.name;

        graceSlotOneDescription.text = graceOneMajorCard.cardDescription;
        graceSlotTwoDescription.text = graceTwoMajorCard.cardDescription;
        graceSlotThreeDescription.text = graceThreeMajorCard.cardDescription;

        buttonHighlight6.gameObject.SetActive(false);
        buttonHighlight7.gameObject.SetActive(false);
        buttonHighlight8.gameObject.SetActive(false);
        selectedAbilityCard = null;

        //selectedMajorArcana = true;
    }

    // Selects grace
    public void SelectGrace(int slot)
    {
        //if (selectedGrace || selectedMajorArcana != true) return; // Guard clause

        print("Selected grace");

        if (slot == 1)
        {
            //inventory.AddCard(graceOneMajorCard);
            buttonHighlight6.gameObject.SetActive(true);
            buttonHighlight7.gameObject.SetActive(false);
            buttonHighlight8.gameObject.SetActive(false);
            selectedAbilityCard = graceOneMajorCard;

            PlaySound(majorcardSound);// Plays card obtained sound
        }
        else if (slot == 2)
        {
            //inventory.AddCard(graceTwoMajorCard);
            buttonHighlight6.gameObject.SetActive(false);
            buttonHighlight7.gameObject.SetActive(true);
            buttonHighlight8.gameObject.SetActive(false);
            selectedAbilityCard = graceTwoMajorCard;

            PlaySound(majorcardSound);// Plays card obtained sound
        }
        else // Assume 3
        {
            //inventory.AddCard(graceThreeMajorCard);
            buttonHighlight6.gameObject.SetActive(false);
            buttonHighlight7.gameObject.SetActive(false);
            buttonHighlight8.gameObject.SetActive(true);
            selectedAbilityCard = graceThreeMajorCard;

            PlaySound(majorcardSound);// Plays card obtained sound
        }

        ShowMinorArcana();

        //selectedGrace = true;
    }

    // Shows minor arcana
    private void ShowMinorArcana()
    {
        print("Showing Minor arcana");

        var deckManager = DeckManager.instance;

        #region Switch_One
        switch (minorCardSlotOne)
        {
            case "wands":
                minorArcanaSlotOneImage.sprite = deckManager.wandsCard;
                minorArcanaSlotOneText.text = DeckManager.instance.listOfMinorCards[1].cardName;
                break;

            case "pentacles":
                minorArcanaSlotOneImage.sprite = deckManager.pentaclesCard;
                minorArcanaSlotOneText.text = DeckManager.instance.listOfMinorCards[3].cardName;

                break;

            case "cups":
                minorArcanaSlotOneImage.sprite = deckManager.cupsCard;
                minorArcanaSlotOneText.text = DeckManager.instance.listOfMinorCards[2].cardName;
                break;

            case "swords":
                minorArcanaSlotOneImage.sprite = deckManager.swordsCard;
                minorArcanaSlotOneText.text = DeckManager.instance.listOfMinorCards[0].cardName;
                break;

            default:
                Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                minorArcanaSlotOneImage.sprite = deckManager.defaultCard;
                break;
        }
        #endregion

        #region Switch_Two
        switch (minorCardSlotTwo)
        {
            case "wands":
                minorArcanaSlotTwoImage.sprite = deckManager.wandsCard;
                minorArcanaSlotTwoText.text = DeckManager.instance.listOfMinorCards[1].cardName;
                break;

            case "pentacles":
                minorArcanaSlotTwoImage.sprite = deckManager.pentaclesCard;
                minorArcanaSlotTwoText.text = DeckManager.instance.listOfMinorCards[3].cardName;
                break;

            case "cups":
                minorArcanaSlotTwoImage.sprite = deckManager.cupsCard;
                minorArcanaSlotTwoText.text = DeckManager.instance.listOfMinorCards[2].cardName;
                break;

            case "swords":
                minorArcanaSlotTwoImage.sprite = deckManager.swordsCard;
                minorArcanaSlotTwoText.text = DeckManager.instance.listOfMinorCards[0].cardName;
                break;

            default:
                Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                minorArcanaSlotTwoImage.sprite = deckManager.defaultCard;
                break;
        }
        #endregion

        #region Switch_Three
        switch (minorCardSlotThree)
        {
            case "wands":
                minorArcanaSlotThreeImage.sprite = deckManager.wandsCard;
                minorArcanaSlotThreeText.text = DeckManager.instance.listOfMinorCards[1].cardName;
                break;

            case "pentacles":
                minorArcanaSlotThreeImage.sprite = deckManager.pentaclesCard;
                minorArcanaSlotThreeText.text = DeckManager.instance.listOfMinorCards[3].cardName;
                break;

            case "cups":
                minorArcanaSlotThreeImage.sprite = deckManager.cupsCard;
                minorArcanaSlotThreeText.text = DeckManager.instance.listOfMinorCards[2].cardName;
                break;

            case "swords":
                minorArcanaSlotThreeImage.sprite = deckManager.swordsCard;
                minorArcanaSlotThreeText.text = DeckManager.instance.listOfMinorCards[0].cardName;
                break;

            default:
                Debug.LogError("Tarot Reading Manager defaulted the image in slot one.");
                minorArcanaSlotThreeImage.sprite = deckManager.defaultCard;
                break;
        }
        #endregion

        selectedMinorArcanaDescriptionText.text = "";
    }

    // Opens confirmation menu and saves inserted slot for later referencing
    public void OpenConfirmMinorCard(int slot)
    {
        confirmSelectionParent.SetActive(true);

        currentSelectedSlot = slot;

        SetAllButtons(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(confirmSelectionNoButton.gameObject);
    }

    public void SelectMinorCard(int slot) 
    {
        if (selectedAbilityCard == null) return;
        //PlaySound(minorcardSound);

        switch (slot) 
        {
            case 1:
                lastSelectedMinorCardSlot = minorCardSlotOne;
                if (selectedMinorCard1 == minorCardSlotOne)
                {
                    selectedMinorCard1 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard2 == minorCardSlotOne)
                {
                    selectedMinorCard2 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard1 == null)
                {
                    selectedMinorCard1 = minorCardSlotOne;
                }
                else if (selectedMinorCard2 == null)
                {
                    selectedMinorCard2 = minorCardSlotOne;
                }
                else
                {
                    selectedMinorCard1 = selectedMinorCard2;
                    selectedMinorCard2 = minorCardSlotOne;
                }
                break;

            case 2:
                lastSelectedMinorCardSlot = minorCardSlotTwo;
                if (selectedMinorCard1 == minorCardSlotTwo)
                {
                    selectedMinorCard1 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard2 == minorCardSlotTwo)
                {
                    selectedMinorCard2 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard1 == null)
                {
                    selectedMinorCard1 = minorCardSlotTwo;
                }
                else if (selectedMinorCard2 == null)
                {
                    selectedMinorCard2 = minorCardSlotTwo;
                }
                else
                {
                    selectedMinorCard1 = selectedMinorCard2;
                    selectedMinorCard2 = minorCardSlotTwo;
                }
                break;

            case 3:
                lastSelectedMinorCardSlot = minorCardSlotThree;
                if (selectedMinorCard1 == minorCardSlotThree)
                {
                    selectedMinorCard1 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard2 == minorCardSlotThree)
                {
                    selectedMinorCard2 = null;
                    minorCardsSelected--;
                }
                else if (selectedMinorCard1 == null)
                {
                    selectedMinorCard1 = minorCardSlotThree;
                }
                else if (selectedMinorCard2 == null)
                {
                    selectedMinorCard2 = minorCardSlotThree;
                }
                else
                {
                    selectedMinorCard1 = selectedMinorCard2;
                    selectedMinorCard2 = minorCardSlotThree;
                }
                break;
        }

        if (lastSelectedMinorCardSlot == selectedMinorCard1 || lastSelectedMinorCardSlot == selectedMinorCard2)
        {
            lastSelectedMinorCardSlot.ToLower();
            switch (lastSelectedMinorCardSlot)
            {
                case "swords":
                    selectedMinorArcanaDescriptionText.text = DeckManager.instance.listOfMinorCards[0].cardDescription;
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "pentacles":
                    selectedMinorArcanaDescriptionText.text = DeckManager.instance.listOfMinorCards[3].cardDescription;
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "wands":
                    selectedMinorArcanaDescriptionText.text = DeckManager.instance.listOfMinorCards[1].cardDescription;
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "cups":
                    selectedMinorArcanaDescriptionText.text = DeckManager.instance.listOfMinorCards[2].cardDescription;
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;
            }
        }

        if (minorCardSlotOne == selectedMinorCard1 || minorCardSlotOne == selectedMinorCard2)
        {

            buttonHighlight3.gameObject.SetActive(true);
        }
        else
        {
            buttonHighlight3.gameObject.SetActive(false);
        }

        if (minorCardSlotTwo == selectedMinorCard1 || minorCardSlotTwo == selectedMinorCard2)
        {
            buttonHighlight4.gameObject.SetActive(true);
        }
        else
        {
            buttonHighlight4.gameObject.SetActive(false);
        }

        if (minorCardSlotThree == selectedMinorCard1 || minorCardSlotThree == selectedMinorCard2)
        {
            buttonHighlight5.gameObject.SetActive(true);
        }
        else
        {
            buttonHighlight5.gameObject.SetActive(false);
        }
    }

    // Selects minor card to upgrade
    public void SelectMinorCardToUpgrade()
    {
        minorCardsSelected++;

        var swordCardScript = inventory.GetComponentInChildren<SwordMinorCard>();
        var pentCardScript = inventory.GetComponentInChildren<PentaclesMinorCard>();
        var wandCardScript = inventory.GetComponentInChildren<WandsMinorCard>();
        var cupCardScript = inventory.GetComponentInChildren<CupsMinorCard>();

        if (currentSelectedSlot == 1)
        {
            minorCardSlotOne.ToLower();
            switch (minorCardSlotOne)
            {
                case "swords":
                    if (swordCardScript) swordCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "pentacles":
                    if (pentCardScript) pentCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "wands":
                    if (wandCardScript) wandCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "cups":
                    if (cupCardScript) cupCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;
            }
        }
        else if (currentSelectedSlot == 2)
        {
            minorCardSlotTwo.ToLower();
            switch (minorCardSlotTwo)
            {
                case "swords":
                    if (swordCardScript) swordCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "pentacles":
                    if (pentCardScript) pentCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "wands":
                    if (wandCardScript) wandCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "cups":
                    if (cupCardScript) cupCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;
            }
        }
        else // Assume 3
        {
            minorCardSlotThree.ToLower();
            switch (minorCardSlotThree)
            {
                case "swords":
                    if (swordCardScript) swordCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "pentacles":
                    if (pentCardScript) pentCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "wands":
                    if (wandCardScript) wandCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;

                case "cups":
                    if (cupCardScript) cupCardScript.Upgrade();
                    PlaySound(minorcardSound);// Plays card obtained sound
                    break;
            }
        }

        SetAllButtons(true);
        if (minorCardsSelected >= 2) CloseTarotReading();
    }

    // Sets all buttons to off or on based on passed in bool
    public void SetAllButtons(bool setTo)
    {
        majorArcanaSlotOneButton.enabled = setTo;
        majorArcanaSlotTwoButton.enabled = setTo;
        minorArcanaSlotOneButton.enabled = setTo;
        minorArcanaSlotTwoButton.enabled = setTo;
        minorArcanaSlotThreeButton.enabled = setTo;
        graceSlotOneButton.enabled = setTo;
        graceSlotTwoButton.enabled = setTo;
        graceSlotThreeButton.enabled = setTo;

        if (setTo == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(majorArcanaSlotOneButton.gameObject);
        }
    }

    private void Update()
    {
        if (confirmSelectionButton != null)
        {
            if (selectedAbilityCard != null && selectedMinorCard2 != null)
            {
                confirmSelectionButton.SetActive(true);
            }
            else
            {
                confirmSelectionButton.SetActive(false);
            }
        }
    }

    public void NextPanel(GameObject panelToOpen)
    {
        openMenu.SetActive(false);
        openMenu = panelToOpen;
        panelToOpen.SetActive(true);
    }

    public void ConfirmSelection()
    {
        var swordCardScript = inventory.GetComponentInChildren<SwordMinorCard>();
        var pentCardScript = inventory.GetComponentInChildren<PentaclesMinorCard>();
        var wandCardScript = inventory.GetComponentInChildren<WandsMinorCard>();
        var cupCardScript = inventory.GetComponentInChildren<CupsMinorCard>();

        inventory.AddCard(selectedAbilityCard);
        
        if (selectedMinorCard1 == "swords" || selectedMinorCard2 == "swords")
        {
            swordCardScript.Upgrade();
        }
        if (selectedMinorCard1 == "pentacles" || selectedMinorCard2 == "pentacles")
        {
            pentCardScript.Upgrade();
        }
        if (selectedMinorCard1 == "wands" || selectedMinorCard2 == "wands")
        {
            wandCardScript.Upgrade();
        }
        if (selectedMinorCard1 == "cups" || selectedMinorCard2 == "cups")
        {
            cupCardScript.Upgrade();
        }

        CloseTarotReading();
    }

    public void PlaySound(AudioClip clip) // Allows the tarot reading menu to play sounds
    {
        audioSource.PlayOneShot(clip);
        audioSource.ignoreListenerPause = true; // Allows tarot reading menu to play sounds even if the game is paused
    }
}
