using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{
    // Singleton instance reference
    public static DeckManager instance;

    // Arcana Sprite Refs
    [Header("Major Card Sprite Refs")]
    public Sprite strengthCard;
    public Sprite deathCard;
    public Sprite devilCard;
    public Sprite worldCard;
    public Sprite sunCard;
    public Sprite moonCard;
    public Sprite defaultCard;

    [Header("Minor Card Sprite Refs")]
    public Sprite wandsCard;
    public Sprite pentaclesCard;
    public Sprite cupsCard;
    public Sprite swordsCard;

    [Header("Minor Card Drop Refs")]
    public GameObject swordsDrop;
    public GameObject wandsDrop;
    public GameObject pentaclesDrop;
    public GameObject cupsDrop;


    private void Awake()
    {
        HandleSingleton();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        HandleSingleton();
    }

    private void HandleSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [Header("Card Refs")]
    // Lists of all scriptable object for both major and minor cards
    public List<MajorCardSO> listOfMajorCards = new List<MajorCardSO>();
    public List<MinorCardSO> listOfMinorCards = new List<MinorCardSO>();


    // Returns all cards store in list of given type
    public List<MajorCardSO> GetAllCardsOfArcanaType(MajorCardSO.MajorCardArcanaType type)
    {
        List<MajorCardSO> cardsToReturn = new List<MajorCardSO>();

        foreach (MajorCardSO card in listOfMajorCards)
        {
            if (card.arcanaType == type)
            {
                cardsToReturn.Add(card);
            }
        }

        return cardsToReturn;
    }
}
