using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;


// GameManager
// Will be expanded in future
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public GameObject mainCamera;
    public GameObject cinemachineCam;

    public TMP_FontAsset damageNumberFont;
    public Material damageNumberMat;
    public StatusEffectData dazedEffectData;

    private bool loadSave = false;
    private bool loadCheckpoint = false;

    private CritChanceController critChanceSystem;

    private TextMeshProUGUI objectiveUI;
    [HideInInspector] public string currentObjective;


    [SerializeField] private GameObject critChanceSystemObj;

    [HideInInspector] public TarotReadingManager tarotReadingManager;

    [HideInInspector]
    public Checkpoint currentActiveCheckpoint;

    [HideInInspector]
    public Animator currentHighPriestessAnimator;

    private void Awake()
    {
        HandleSingleton();
        HandleCritSystemReference();

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Update()
    {
        //if (player != null) print("Player pos: " + player.transform.position);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;

#if UNITY_EDITOR
        File.Delete(Application.dataPath + "/GameSaveFile.json");
        File.Delete(Application.dataPath + "/GameSaveFile.json.meta");
#endif
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        HandleSingleton();
        HandleCritSystemReference();
        GetRefs();

        if (loadCheckpoint)
        {
            Time.timeScale = 1.0f;
            LoadCheckpoint();
            loadCheckpoint = false;
        }

        if (loadSave)
        {
            Time.timeScale = 1.0f;
            LoadData();
            loadSave = false;
        }
    }

    // Gets singleton refs
    private void GetRefs()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cinemachineCam = GameObject.FindGameObjectWithTag("FreeCam");
        //objectiveUI = GameObject.Find("Objective").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        if (tarotReadingManager == null && player != null) 
            tarotReadingManager = player.transform.parent.transform.GetComponentInChildren<TarotReadingManager>(); // wow
    }

    private void HandleCritSystemReference()
    {
        if (critChanceSystem == null)
        {
            var critObj = Instantiate(critChanceSystemObj);
            critObj.transform.parent = this.transform;
        }
    }

    public void UpdateObjective(string newObjective)
    {
        currentObjective = newObjective;
        objectiveUI.text = currentObjective;
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

    // Starts tarot reading
    public void BeginTarotReading()
    {
        tarotReadingManager.StartTarotReading();
    }

    // Saves game data
    public void SaveGameData()
    {
        print("Saving game data");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        InventoryManager inventory = player.GetComponentInChildren<InventoryManager>();
        SaveData newData = new SaveData();

        if (playerStats == null ||  inventory == null)
        {
            Debug.LogError("Couldn't find player stats or inventory. Not saving game data");
            return;
        }

        if (currentActiveCheckpoint == null)
        {
            Debug.LogWarning("Couldn't find a player checkpoint! Defaulting to 0,0,0...");
            newData.checkpoint = Vector3.zero;
        }
        else
        {
            newData.checkpoint = currentActiveCheckpoint.transform.position;
        }
        
        newData.currency = playerStats.Cash;

        // Weapon card saving
        List<int> weaponCardIDs = new List<int>();
        var weaponCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Weapon);
        if (weaponCards.Any())
        {
            foreach ( var card in weaponCards )
            {
                weaponCardIDs.Add(card.cardSO.cardID);
            }
            newData.weaponCardIds = weaponCardIDs;
        } 
        else 
        {
            weaponCardIDs.Add(-1);
            newData.weaponCardIds = weaponCardIDs; 
        }
        //---------------------------------------------------------------------------

        // Movement card saving
        List<int> movementCardIDs = new List<int>();
        var movementCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Movement);
        if (movementCards.Any())
        {
            foreach (var card in movementCards)
            {
                movementCardIDs.Add(card.cardSO.cardID);
            }
            newData.movementCardIds = movementCardIDs;
        } 
        else
        {
            movementCardIDs.Add(-1);
            newData.movementCardIds = movementCardIDs;
        }
        //---------------------------------------------------------------------------

        // Special card saving
        List<int> specialCardIDs = new List<int>();
        var specialCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Special);
        if (specialCards.Any())
        {
            foreach (var card in specialCards)
            {
                specialCardIDs.Add(card.cardSO.cardID);
            }
            newData.specialCardIds = specialCardIDs;
        }
        else
        {
            specialCardIDs.Add(-1);
            newData.specialCardIds = specialCardIDs;
        }
        //---------------------------------------------------------------------------

        // Support card saving
        List<int> supportCardIDs = new List<int>();
        var supportCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Support);
        if (supportCards.Any())
        {
            foreach (var card in supportCards)
            {
                supportCardIDs.Add(card.cardSO.cardID);
            }
            newData.supportCardIds = supportCardIDs;
        } 
        else
        {
            supportCardIDs.Add(-1);
            newData.supportCardIds = supportCardIDs;
        }
        //---------------------------------------------------------------------------

        // Ultimate card saving
        List<int> ultimateCardIDs = new List<int>();
        var ultimateCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Ultimate);
        if (ultimateCards.Any())
        {
            foreach (var card in ultimateCards)
            {
                ultimateCardIDs.Add(card.cardSO.cardID);
            }
            newData.ultimateCardIds = ultimateCardIDs;
        } 
        else
        {
            ultimateCardIDs.Add(-1);
            newData.ultimateCardIds = ultimateCardIDs;
        }
        //---------------------------------------------------------------------------

        // Passive card saving
        List<int> passiveCardIDs = new List<int>();
        var passiveCards = inventory.GetMajorCards(MajorCardSO.MajorCardType.Passive);
        if (passiveCards.Any())
        {
            foreach (var card in passiveCards)
            {
                passiveCardIDs.Add(card.cardSO.cardID);
            }
            newData.passiveCardIds = passiveCardIDs;
        }
        else
        {
            passiveCardIDs.Add(-1);
            newData.passiveCardIds = passiveCardIDs;
        }
        //---------------------------------------------------------------------------


        newData.cupsTier = inventory.GetComponentInChildren<CupsMinorCard>().currentTier;
        newData.wandsTier = inventory.GetComponentInChildren<WandsMinorCard>().currentTier;
        newData.swordsTier = inventory.GetComponentInChildren<SwordMinorCard>().currentTier;
        newData.pentaclesTier = inventory.GetComponentInChildren<PentaclesMinorCard>().currentTier;
        newData.Objective = this.currentObjective;
        string jsonString = JsonUtility.ToJson(newData, true);

#if UNITY_EDITOR
        File.WriteAllText(Application.persistentDataPath + "/GameSaveFile.json", jsonString);
        print(Application.persistentDataPath);
#else
        File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/GameSaveFile.json", jsonString);
#endif
    }

    public void LoadDataOnNextSceneLoad()
    {
        print("Loading data on next scene load");
        loadSave = true;
    }

    public void LoadCheckpointOnRespawn()
    {
        print("Loading data on respawn");
        loadCheckpoint = true;
    }

    private void LoadData()
    {
        print("Loading game data...");

        GetRefs();

        string fromJson;

#if UNITY_EDITOR
        try
        {
            fromJson = File.ReadAllText(Application.persistentDataPath + "/GameSaveFile.json");
        }
        catch
        {
            Debug.LogError("Couldn't find the editor save file. Failed to load data");
            return;
        }

#else
        try
        {
            fromJson = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/GameSaveFile.json");
        }
        catch
        {
            Debug.LogError("Couldn't find the game save file. Failed to load data");
            return;
        }     
#endif

        SaveData data = JsonUtility.FromJson<SaveData>(fromJson);

        var playerStats = player.GetComponent<PlayerStats>();
        var inventory = player.GetComponentInChildren<InventoryManager>();

        playerStats.Cash = data.currency; // Sets currency data

        // Sets Major Card data
        for (int i = 0; i < data.weaponCardIds.Count; i++)
        {
            inventory.AddCard(data.weaponCardIds[i]);
        }
        for (int i = 0; i < data.movementCardIds.Count; i++)
        {
            inventory.AddCard(data.movementCardIds[i]);
        }
        for (int i = 0; i < data.specialCardIds.Count; i++)
        {
            inventory.AddCard(data.specialCardIds[i]);
        }
        for (int i = 0; i < data.supportCardIds.Count; i++)
        {
            inventory.AddCard(data.supportCardIds[i]);
        }
        for (int i = 0; i < data.ultimateCardIds.Count; i++)
        {
            inventory.AddCard(data.ultimateCardIds[i]);
        }
        for (int i = 0; i < data.passiveCardIds.Count; i++)
        {
            inventory.AddCard(data.passiveCardIds[i]);
        }

        // Sets Minor Card data
        inventory.CreateMinorCards();
        var cupsCard = inventory.GetComponentInChildren<CupsMinorCard>();
        while (cupsCard.currentTier < data.cupsTier)
        {
            cupsCard.Upgrade();
        }
        var wandsCard = inventory.GetComponentInChildren<WandsMinorCard>();
        while (wandsCard.currentTier < data.wandsTier)
        {
            wandsCard.Upgrade();
        }
        var swordCard = inventory.GetComponentInChildren<SwordMinorCard>();
        while (swordCard.currentTier < data.swordsTier)
        {
            swordCard.Upgrade();
        }
        var pentaclesCard = inventory.GetComponentInChildren<PentaclesMinorCard>();
        while (pentaclesCard.currentTier < data.pentaclesTier)
        {
            pentaclesCard.Upgrade();
        }
    }

    private void LoadCheckpoint()
    {
        print("Loading from checkpoint...");

        GetRefs();

        string fromJson;

#if UNITY_EDITOR
        try
        {
            fromJson = File.ReadAllText(Application.persistentDataPath + "/GameSaveFile.json");
        }
        catch
        {
            Debug.LogError("Couldn't find the editor save file. Failed to load data");
            return;
        }

#else
        try
        {
            fromJson = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/GameSaveFile.json");
        }
        catch
        {
            Debug.LogError("Couldn't find the game save file. Failed to load data");
            return;
        }     
#endif

        SaveData data = JsonUtility.FromJson<SaveData>(fromJson);

        if (data.checkpoint != Vector3.zero)
        {
            player.gameObject.SetActive(false);
            player.transform.position = data.checkpoint; // Moves player to last checkpoint
            player.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Player's checkpoint was 0,0,0. Ignoring...");
        }
        

        var playerStats = player.GetComponent<PlayerStats>();
        var inventory = player.GetComponentInChildren<InventoryManager>();

        playerStats.Cash = data.currency; // Sets currency data

        // Sets Major Card data
        for (int i = 0; i < data.weaponCardIds.Count; i++)
        {
            inventory.AddCard(data.weaponCardIds[i]);
        }
        for (int i = 0; i < data.movementCardIds.Count; i++)
        {
            inventory.AddCard(data.movementCardIds[i]);
        }
        for (int i = 0; i < data.specialCardIds.Count; i++)
        {
            inventory.AddCard(data.specialCardIds[i]);
        }
        for (int i = 0; i < data.supportCardIds.Count; i++)
        {
            inventory.AddCard(data.supportCardIds[i]);
        }
        for (int i = 0; i < data.ultimateCardIds.Count; i++)
        {
            inventory.AddCard(data.ultimateCardIds[i]);
        }
        for (int i = 0; i < data.passiveCardIds.Count; i++)
        {
            inventory.AddCard(data.passiveCardIds[i]);
        }

        // Sets Minor Card data
        inventory.CreateMinorCards();
        var cupsCard = inventory.GetComponentInChildren<CupsMinorCard>();
        while (cupsCard.currentTier < data.cupsTier)
        {
            cupsCard.Upgrade();
        }
        var wandsCard = inventory.GetComponentInChildren<WandsMinorCard>();
        while (wandsCard.currentTier < data.wandsTier)
        {
            wandsCard.Upgrade();
        }
        var swordCard = inventory.GetComponentInChildren<SwordMinorCard>();
        while (swordCard.currentTier < data.swordsTier)
        {
            swordCard.Upgrade();
        }
        var pentaclesCard = inventory.GetComponentInChildren<PentaclesMinorCard>();
        while (pentaclesCard.currentTier < data.pentaclesTier)
        {
            pentaclesCard.Upgrade();
        }
    }

    public struct SaveData
    {
        public Vector3 checkpoint;
        
        // Major Card ID lists
        public int currency;
        public List<int> weaponCardIds;
        public List<int> movementCardIds;
        public List<int> specialCardIds;
        public List<int> supportCardIds;
        public List<int> ultimateCardIds;
        public List<int> passiveCardIds;

        // Minor Card Tiers
        public int cupsTier;
        public int wandsTier;
        public int pentaclesTier;
        public int swordsTier;

        public string Objective;

        public SaveData(Vector3 checkpoint, int currency, List<int> weaponIDs, List<int> movementIDs, List<int> specialIDs, List<int> supportIDs, List<int> ultimateIDs,
            List<int> passiveIDs, int cups, int wands, int pentacles, int swords, string Objective)
        {
            this.checkpoint = checkpoint;
            this.currency = currency;
            this.weaponCardIds = weaponIDs;
            this.movementCardIds = movementIDs;
            this.specialCardIds = specialIDs;
            this.supportCardIds = supportIDs;
            this.ultimateCardIds = ultimateIDs;
            this.passiveCardIds = passiveIDs;

            this.cupsTier = cups;
            this.wandsTier = wands;
            this.pentaclesTier = pentacles;
            this.swordsTier = swords;

            this.Objective = Objective;
        }
    }  
}
