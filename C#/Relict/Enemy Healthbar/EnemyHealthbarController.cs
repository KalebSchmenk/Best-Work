using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbarController : MonoBehaviour
{
    [SerializeField] private Transform healthBar;
    [SerializeField] private AIMain aiController;
    [SerializeField] private GameObject healthbarParent;
    private float enemyCurrentHealth;
    private float enemyMaxHealth;

    private GameManager gameManagerInstance;

    private void Start()
    {
        gameManagerInstance = GameManager.instance;

        UpdateMaxHealth();

        enemyCurrentHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCurrentHealth = aiController.health;

        UpdateMaxHealth();

        if (enemyCurrentHealth < enemyMaxHealth)
        {
            if (healthbarParent.activeSelf == false) healthbarParent.SetActive(true);

            // Updating the Healthbar Size
            healthBar.localScale = new Vector3(enemyCurrentHealth / enemyMaxHealth, 1f, 1f);
        }
        else
        {
            healthbarParent.SetActive(false);
        }

        if (enemyCurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void UpdateMaxHealth()
    {
        switch (gameManagerInstance.GameDifficulty)
        {
            case GameManager.Difficulty.Easy:
                enemyMaxHealth = aiController.easyInfo.newHealth;
                break;

            case GameManager.Difficulty.Normal:
                enemyMaxHealth = aiController.normalInfo.newHealth;
                break;

            case GameManager.Difficulty.Hard:
                enemyMaxHealth = aiController.hardInfo.newHealth;
                break;
        }
    }
}
