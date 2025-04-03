using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls Debug Menu

public class DebugUIManager : MonoBehaviour
{
    public GameObject player; // Player ref
    public GameObject mainCam; // Camera ref
    [SerializeField] UI_PauseMenu pauseMenu;
    public Canvas canvas; // Canvas ref
    public TarotReadingManager tarotReadingManager;
    public InventoryManager inventoryManager; // Player inventory ref
    public PlayerStats playerStats; // Player stats ref
    public WeaponStats weaponStats; // Player weapon stats red

    public GameObject defaultMenu; // Default debug menu
    public GameObject minorCardMenu; // Minor card debug menu
    public GameObject majorCardMenu; // Major card debug menu
    public GameObject otherMenu; // Other debug menu
    public GameObject levelSettingsMenu; // Level settings menu

    // Text refs for each minor card
    [Header("Minor Cards")]
    [Header("Cup")]
    public TextMeshProUGUI cupStat;
    public TextMeshProUGUI cupTier;
    public TextMeshProUGUI increasedHealth;
    public TextMeshProUGUI healOnKill;
    [Header("Sword")]
    public TextMeshProUGUI swordStat;
    public TextMeshProUGUI swordTier;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI critChance;
    [Header("Wand")]
    public TextMeshProUGUI wandStat;
    public TextMeshProUGUI wandTier;
    public TextMeshProUGUI RechargeRate;
    public TextMeshProUGUI jumpHeight;
    [Header("Pentatcles")]
    public TextMeshProUGUI pentStat;
    public TextMeshProUGUI pentTier;
    public TextMeshProUGUI discountOnPurchase;
    public TextMeshProUGUI moneyDoubleChance;

    SwordMinorCard swordCardScript; // Sword minor card script ref
    PentaclesMinorCard pentCardScript; // Pentacles minor card script ref
    WandsMinorCard wandCardScript; // Wand card script ref
    CupsMinorCard cupCardScript; // Cup card script ref

    // Major card text refs
    [Header("Major Cards")]
    public TextMeshProUGUI weaponCardName;
    public TextMeshProUGUI movementCardName;
    public TextMeshProUGUI specialCardName;
    public TextMeshProUGUI supportCardName;
    public TextMeshProUGUI ultimateCardName;
    public TextMeshProUGUI passiveOneCardName;
    public TextMeshProUGUI passiveTwoCardName;

    // Major Card Image refs
    public Sprite defaultCardImage;
    public Image weaponCardImage;
    public Image movementCardImage;
    public Image specialCardImage;
    public Image supportCardImage;
    public Image ultimateCardImage;
    public Image passiveOneCardImage;
    public Image passiveTwoCardImage;

    // Major card script refs
    MajorCardBase weaponCard;
    MajorCardBase movementCard;
    MajorCardBase specialCard;
    MajorCardBase supportCard;
    MajorCardBase ultimateCard;
    MajorCardBase passiveOneCard;
    MajorCardBase passiveTwoCard;

    // Card Controller Ref
    public List<DebugUIMajorCardAddController> controllerList = new List<DebugUIMajorCardAddController>();

    // Player stat text refs
    [Header("Player Stats")]
    public TextMeshProUGUI playerHP;

    [Header("Other Panel Scroll Refs")]
    public GameObject contentObj;
    public GameObject contentObjPrefab;
    public TMP_Dropdown difficultyDropdown;

    [Header("Spawnable Enemies")]
    public List<GameObject> spawnableEnemies = new List<GameObject>();

    public delegate void ResetMajorCards();
    public static ResetMajorCards ResetAllMajorCards;

    public StatusEffectData currentSelectedStatusEffect;
    List<GameObject> contentObjs = new List<GameObject>();
    public delegate void AddStatusEffectToSelectedEnemies(StatusEffectData statusEffect);
    public static AddStatusEffectToSelectedEnemies AddStatusEffect;
    public delegate void RemoveStatusEffectToSelectedEnemies(StatusEffectData statusEffect);
    public static RemoveStatusEffectToSelectedEnemies RemoveStatusEffect;


    public static bool pause; // Used by the audiolistener to determine whether the game is paused or not

    // Level Settings Vars and Options
    private int levelIndex = -1;
    private int zoneIndex = -1;
    [SerializeField] private List<GameObject> zoneSelectedList = new List<GameObject>();

    // God mode health save
    private int initHealth;

    private void Start()
    {
        //LockCursor(); // Locks cursor BE CAREFUL OF HAVING THIS SCRIPT CONTROL THE CURSOR
        GetScripts(); // Gets scripts
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            LevelManager.instance.NextLevel();
        }


        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (canvas.enabled)
            {
                GameManager.instance.isMenuOpen = false;

                canvas.enabled = false;
                defaultMenu.SetActive(false);
                CloseDebugMenu();
                LockCursor();
                Time.timeScale = 1;
                if (player != null)
                {
                    player.GetComponent<PlayerController>().enabled = true;
                    player.GetComponent<PlayerWeaponController>().enabled = true;
                }
                mainCam.SetActive(true);

                AudioListener.pause = false; // Allows audio to play when debug menu is inactive
            }
            else
            {
                if (GameManager.instance.isMenuOpen) return;

                GameManager.instance.isMenuOpen = true;

                difficultyDropdown.value = (int)GameManager.instance.GameDifficulty;

                UpdateControllers();
                canvas.enabled = true;
                defaultMenu.SetActive(true);
                UnLockCursor();
                if (player != null)
                {
                    player.GetComponent<PlayerController>().enabled = false;
                    player.GetComponent<PlayerWeaponController>().enabled = false;
                }
                Time.timeScale = 0;
                mainCam.SetActive(false);

                AudioListener.pause = true; // Disables audio when in debug menu
            }
        }

        if (minorCardMenu.activeSelf)
        {
            HandleMinorCardValues();
        }
        if (majorCardMenu.activeSelf)
        {
            HandleMajorCardValues();
        }
        if (otherMenu.activeSelf)
        {
            HandlePlayerHP();
        }
    }

    // Handles Player HP text
    private void HandlePlayerHP()
    {
        playerHP.text = "Player HP: " + playerStats.HP.currentValue;
    }

    // Handles Major card stats displayed on screen
    private void HandleMajorCardValues()
    {
        if (inventoryManager.weaponMajorCards.Count > 0 && inventoryManager.weaponMajorCards[0].cardSO)
        {
            weaponCardName.text = inventoryManager.weaponMajorCards[0].cardSO.cardName;
            weaponCardImage.sprite = inventoryManager.weaponMajorCards[0].cardSO.cardImage;
        }
        else { weaponCardName.text = "Weapon Card"; weaponCardImage.sprite = defaultCardImage; }

        if (inventoryManager.movementMajorCards.Count > 0 && inventoryManager.movementMajorCards[0].cardSO)
        {
            movementCardName.text = inventoryManager.movementMajorCards[0].cardSO.cardName;
            movementCardImage.sprite = inventoryManager.movementMajorCards[0].cardSO.cardImage;
        }
        else { movementCardName.text = "Movement Card"; movementCardImage.sprite = defaultCardImage; }

        if (inventoryManager.specialMajorCards.Count > 0 && inventoryManager.specialMajorCards[0].cardSO)
        {
            specialCardName.text = inventoryManager.specialMajorCards[0].cardSO.cardName;
            specialCardImage.sprite = inventoryManager.specialMajorCards[0].cardSO.cardImage;
        }
        else { specialCardName.text = "Special Card"; specialCardImage.sprite = defaultCardImage; }

        if (inventoryManager.supportMajorCards.Count > 0 && inventoryManager.supportMajorCards[0].cardSO)
        {
            supportCardName.text = inventoryManager.supportMajorCards[0].cardSO.cardName;
            supportCardImage.sprite = inventoryManager.supportMajorCards[0].cardSO.cardImage;
        }
        else { supportCardName.text = "Support Card"; supportCardImage.sprite = defaultCardImage; }

        if (inventoryManager.ultimateMajorCards.Count > 0 && inventoryManager.ultimateMajorCards[0].cardSO)
        {
            ultimateCardName.text = inventoryManager.ultimateMajorCards[0].cardSO.cardName;
            ultimateCardImage.sprite = inventoryManager.ultimateMajorCards[0].cardSO.cardImage;
        }
        else { ultimateCardName.text = "Ultimate Card"; ultimateCardImage.sprite = defaultCardImage; }

        if (inventoryManager.passiveMajorCards.Count > 0 && inventoryManager.passiveMajorCards[0].cardSO)
        {
            passiveOneCardName.text = inventoryManager.passiveMajorCards[0].cardSO.cardName;
            passiveOneCardImage.sprite = inventoryManager.passiveMajorCards[0].cardSO.cardImage;
        }
        else { passiveOneCardName.text = "Passive Card"; passiveOneCardImage.sprite = defaultCardImage; }

        if (inventoryManager.passiveMajorCards.Count > 1 && inventoryManager.passiveMajorCards[1].cardSO)
        {
            passiveTwoCardName.text = inventoryManager.passiveMajorCards[1].cardSO.cardName;
            passiveTwoCardImage.sprite = inventoryManager.passiveMajorCards[1].cardSO.cardImage;
        }
        else { passiveTwoCardName.text = "Passive Card"; passiveTwoCardImage.sprite = defaultCardImage; }
    }

    // Handles minor card stats displayed on screen
    private void HandleMinorCardValues()
    {
        cupTier.text = "Level: " + cupCardScript.currentTier;
        cupStat.text = "Current max hp: " + playerStats.MaxHP.Value;
        increasedHealth.text = "Current increased healing: " + playerStats.HealthRegeneration.Value;
        healOnKill.text = "Current heal on kill: " + playerStats.HealthOnKill.Value;

        swordTier.text = "Level: " + swordCardScript.currentTier;
        swordStat.text = "Current damage: " + weaponStats.Damage.Value;
        attackSpeed.text = "Current attack speed: " + weaponStats.RateOfFire.Value;
        critChance.text = "Current crit chance: " + weaponStats.CritChance.Value;

        wandTier.text = "Level: " + wandCardScript.currentTier;
        wandStat.text = "Current cooldown reduction: " + playerStats.BaseCooldown.Value;
        RechargeRate.text = "Current recharge rate: " + weaponStats.RechargeRate.Value;
        jumpHeight.text = "Current jump height: " + playerStats.JumpHeight.Value;


        pentTier.text = "Level: " + pentCardScript.currentTier;
        pentStat.text = "Current minor card drop rate: " + playerStats.MinorArcanaDropRate.Value;
        discountOnPurchase.text = "Discount on purchase: " + playerStats.DiscountOnPurchase.Value;
        moneyDoubleChance.text = "Chance money doubling: " + playerStats.ChanceToDoubleMoney.Value;
    }

    // Gets scripts
    void GetScripts()
    {
        swordCardScript = inventoryManager.GetComponentInChildren<SwordMinorCard>();
        pentCardScript = inventoryManager.GetComponentInChildren<PentaclesMinorCard>();
        wandCardScript = inventoryManager.GetComponentInChildren<WandsMinorCard>();
        cupCardScript = inventoryManager.GetComponentInChildren<CupsMinorCard>();

        if (inventoryManager.weaponMajorCards.Count > 0) weaponCard = inventoryManager.weaponMajorCards[0].cardScript;
        if (inventoryManager.movementMajorCards.Count > 0) movementCard = inventoryManager.movementMajorCards[0].cardScript;
        if (inventoryManager.specialMajorCards.Count > 0) specialCard = inventoryManager.specialMajorCards[0].cardScript;
        if (inventoryManager.supportMajorCards.Count > 0) supportCard = inventoryManager.supportMajorCards[0].cardScript;
        if (inventoryManager.ultimateMajorCards.Count > 0) ultimateCard = inventoryManager.ultimateMajorCards[0].cardScript;
        if (inventoryManager.passiveMajorCards.Count > 0) passiveOneCard = inventoryManager.passiveMajorCards[0].cardScript;
        if (inventoryManager.passiveMajorCards.Count > 1) passiveTwoCard = inventoryManager.passiveMajorCards[1].cardScript;
    }

    // Locks cursor
    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Unlocks cursor
    private void UnLockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // Opens a panel passed in
    public void OpenPanel(GameObject panelToOpen)
    {
        panelToOpen.SetActive(true);
        defaultMenu.SetActive(false);
    }
    // Closes panel passed in
    public void ClosePanel(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
        defaultMenu.SetActive(true);
    }
    // Closes all menus
    public void CloseDebugMenu()
    {
        defaultMenu.SetActive(false);
        minorCardMenu.SetActive(false);
        majorCardMenu.SetActive(false);
        otherMenu.SetActive(false);
        levelSettingsMenu.SetActive(false);

        ClearContent();

        LockCursor();
    }
    // Clears all minor cards tiers back to 0
    public void ClearCardModifiers()
    {
        // GET ME OFF THIS WILD LOOPING RIDE

        int i = 1;
        while ( !(i == 0))
        {
            i = swordCardScript.Downgrade();
        }
        i = 1;
        while (!(i == 0))
        {
            i = cupCardScript.Downgrade();
        }
        i = 1;
        while (!(i == 0))
        {
            i = wandCardScript.Downgrade();
        }
        i = 1;
        while (!(i == 0))
        {
            i = pentCardScript.Downgrade();
        }
    }
    // Clears all major cards from inventory
    public void ClearMajorCards()
    {
        inventoryManager.ClearMajorCards();
        ResetAllMajorCards?.Invoke();
    }
    // Upgrades card passed in as a string
    public void UpgradeCard(string card)
    {
        card = card.ToLower();

        switch (card)
        {
            case "swords":

                if (swordCardScript) swordCardScript.Upgrade();

                break;
            case "pentacles":

                if (pentCardScript) pentCardScript.Upgrade();

                break;
            case "wands":

                if (wandCardScript) wandCardScript.Upgrade();

                break;
            case "cups":

                if (cupCardScript) cupCardScript.Upgrade();

                break;
        }
    }
    // Downgrades card passed in as a string
    public void DowngradeCard(string card)
    {
        card = card.ToLower();

        switch (card)
        {
            case "swords":

                if (swordCardScript) swordCardScript.Downgrade();

                break;
            case "pentacles":

                if (pentCardScript) pentCardScript.Downgrade();

                break;
            case "wands":

                if (wandCardScript) wandCardScript.Downgrade();

                break;
            case "cups":

                if (cupCardScript) cupCardScript.Downgrade();

                break;
        }
    }
    // Heals player by 10
    public void HealPlayer()
    {
        player.GetComponent<PlayerStats>().HP.currentValue += 10;
    }
    // Hurts player by 10
    public void HurtPlayer()
    {
        player.GetComponent<PlayerStats>().HP.currentValue -= 10;
    }

    // Invokes OnEnemyKill Event
    public void KillEnemy()
    {
        PlayerEvents.OnEnemyKill.Invoke(10);
    }

    // Starts a tarot reading
    public void StartTarotReading()
    {
        CloseDebugMenu();
        tarotReadingManager.StartTarotReading();
    }

    // Gets all effectables in scene and creates obj to display in scroll list
    public void GetEffectables()
    {
        var effectableArray = FindObjectsOfType<MonoBehaviour>().OfType<IEffectable>();

        int i = 0;
        foreach (IEffectable effectable in effectableArray)
        {
            MonoBehaviour mono = effectable as MonoBehaviour;
            GameObject effectableObj = mono.gameObject;

            var contentSpawnedObj = Instantiate(contentObjPrefab, contentObj.transform);
            contentObjs.Add(contentSpawnedObj);

            var scrollItem = contentSpawnedObj.GetComponent<DebugMenuEnemyScrollItem>();
            scrollItem.effectable = effectable;
            scrollItem.effectableName.text = effectable.ToString() + " " + i;
            scrollItem.effectableName.text += " - " + Vector3.Distance(effectableObj.transform.position, player.transform.position).ToString("0.00");
            i++;
        }
    }

    // Gets all enemies and kills them
    public void PurgeEnemies()
    {
        var damageableArray = FindObjectsOfType<MonoBehaviour>().OfType<ITakeDamage>();

        foreach (ITakeDamage damageable in damageableArray)
        {
            MonoBehaviour mono = damageable as MonoBehaviour;
            GameObject damageableObj = mono.gameObject;

            if (damageableObj.CompareTag("Enemy"))
            {
                damageable.TakeDamage(damageableObj.transform.position, Color.magenta, float.MaxValue, false);
            }
        }
    }

    // Toggles god mode on and off
    public void GodModeToggle(bool toggle)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (toggle) // Turn on
        {
            initHealth = (int)playerStats.MaxHP.Value;
            playerStats.MaxHP.BaseValue = int.MaxValue;
            playerStats.HP.currentValue = int.MaxValue - 1;
        }
        else // Turn off
        {
            playerStats.MaxHP.BaseValue = initHealth - 2; // why do i have to minus 2 here? i have no idea... Will be incorrect if not done
            playerStats.HP.currentValue = playerStats.MaxHP.BaseValue;
        }
    }

    // Warps to chariot and enables it
    public void WarpToChariot()
    {
        ZoneManager zoneManager = GameObject.FindGameObjectWithTag("ZoneManager").GetComponent<ZoneManager>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        CharacterController characterController = player.GetComponent<CharacterController>();
        CinemachineVirtualCamera cam = GameManager.instance.cinemachineCam.GetComponent<CinemachineVirtualCamera>();

        zoneManager.chariot.SetActive(true);

        playerController.enabled = false;
        characterController.enabled = false;

        var oldPlayerPos = player.transform.position;
        player.transform.position = zoneManager.chariot.transform.position + (Vector3.up * 15f);
        cam.OnTargetObjectWarped(player.transform, zoneManager.chariot.transform.position - oldPlayerPos);

        playerController.enabled = true;
        characterController.enabled = true;
    }

    // Warps to nearest enemy
    public void WarpToNearestEnemy()
    {
        var damageableArray = FindObjectsOfType<MonoBehaviour>().OfType<ITakeDamage>();
        List<GameObject> enemyList = new List<GameObject>();

        foreach (ITakeDamage damageable in damageableArray)
        {
            MonoBehaviour mono = damageable as MonoBehaviour;
            GameObject damageableObj = mono.gameObject;

            if (damageableObj.CompareTag("Enemy"))
            {
                if (damageableObj.TryGetComponent<AIDead>(out AIDead aiDead))
                {
                    if (aiDead.isDead) continue; // Dont add to loop
                    enemyList.Add(damageableObj);
                }
            }
        }

        if (enemyList.Count <= 0) return; // Guard clause

        enemyList = enemyList.OrderBy((obj) => (obj.transform.position - player.transform.position).sqrMagnitude).ToList(); // Sort by distance

        PlayerController playerController = player.GetComponent<PlayerController>();
        CharacterController characterController = player.GetComponent<CharacterController>();
        CinemachineVirtualCamera cam = GameManager.instance.cinemachineCam.GetComponent<CinemachineVirtualCamera>();

        playerController.enabled = false;
        characterController.enabled = false;

        var oldPlayerPos = player.transform.position;
        player.transform.position = enemyList[0].transform.position + (Vector3.up * 15f);
        cam.OnTargetObjectWarped(player.transform, enemyList[0].transform.position - oldPlayerPos);

        playerController.enabled = true;
        characterController.enabled = true;
    }

    // Completes current objective
    public void CompleteCurrentObjective()
    {
        ZoneManager zoneManager = GameObject.FindGameObjectWithTag("ZoneManager").GetComponent<ZoneManager>();
        if (zoneManager.activeObjective != null) zoneManager.activeObjective.FinishObjective();
    }

    // Clears content from effectables scroll list
    private void ClearContent()
    {
        foreach(var obj in contentObjs)
        {
            Destroy(obj);
        }
    }

    // Invokes event for selected effectables to add the passed in status effect
    public void AddStatusEffectToEffectable()
    {
        if (currentSelectedStatusEffect != null) AddStatusEffect?.Invoke(currentSelectedStatusEffect);
    }
    // Invokes event for selected effectables to remove the passed in status effect
    public void RemoveStatusEffectFromEffectable()
    {
        if (currentSelectedStatusEffect != null) RemoveStatusEffect?.Invoke(currentSelectedStatusEffect);
    }

    // temporary code to invoke player win state
    public void ActivatePlayerWinState()
    {
        PlayerEvents.onWin?.Invoke(); // invoke player win state
    }

    // Tells controllers to check if they were already selected in the inventory and to update accordingly
    private void UpdateControllers()
    {
        foreach(var controller in controllerList)
        {
            controller.CheckIfAlreadyOwned();
        }
    }

    // Gets Card ID input and adds card
    public void AddCardByID(string id)
    {
        inventoryManager.AddCard(Int32.Parse(id));
    }

    // Force load level by name
    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    // Loads new zone
    public void LoadNewZone()
    {
        if (levelIndex == -1 || zoneIndex == -1) return; // Guard clause

        foreach (var obj in zoneSelectedList)
        {
            obj.SetActive(false);
        }

        LevelManager.instance.ForceLoadZone(levelIndex, zoneIndex);
    }

    // Open A Koros Zone
    public void LoadKorosZone(int newZoneIndex)
    {
        levelIndex = 0;// Assume koros is index = 0
        zoneIndex = newZoneIndex;
        foreach(var obj in zoneSelectedList)
        {
            obj.SetActive(false);
        }
    }

    // Open A Thorm Zone
    public void LoadThormZone(int newZoneIndex)
    {
        levelIndex = 1; // Assume thorm is index = 1
        zoneIndex = newZoneIndex;
        foreach (var obj in zoneSelectedList)
        {
            obj.SetActive(false);
        }
    }

    // Open A Serun Zone
    public void LoadSerunZone(int newZoneIndex)
    {
        levelIndex = 2; // Assume serun is index = 2
        zoneIndex = newZoneIndex;
        foreach (var obj in zoneSelectedList)
        {
            obj.SetActive(false);
        }
    }

    // Spawn an enemy
    public void SpawnEnemy(int index)
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        var rayHit = RayCast(cam.transform.position, cam.transform.forward, Mathf.Infinity);

        if (rayHit.collider != null)
        {
            Instantiate(spawnableEnemies[index], rayHit.point, Quaternion.identity);
        }
    }

    // Raycast
    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len)
    {
        RaycastHit hit;

        //Debug.DrawLine(from, from + (dir * maxSpawnDistance), UnityEngine.Color.green); // Debug draw

        if (Physics.Raycast(from, dir, out hit, len, -1, QueryTriggerInteraction.Ignore))
        {
            return hit;
        }

        return new RaycastHit();
    }

    public void ChangeDifficulty(int value)
    {
        GameManager.instance.ChangeDifficulty((GameManager.Difficulty)value);
    }
}
