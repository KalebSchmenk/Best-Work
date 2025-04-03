using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Image bossHealthMask;
    [SerializeField] GameObject enemyObject;
    [SerializeField] TextMeshProUGUI bossHealthText;
    private MeleeEnemyAIController meleeEnemyAIController;
    private float currentHealth;
    private float maxHealth;
    private bool isBoss;
    float normSize;
    float amount;

    private void Start() {
        meleeEnemyAIController = enemyObject.GetComponent<MeleeEnemyAIController>();
        maxHealth = 1500;
        normSize = bossHealthMask.rectTransform.rect.width;
    }

    private void Update() {
        isBoss = meleeEnemyAIController._isFinalBoss;
        if(isBoss){
            currentHealth = meleeEnemyAIController._health;
            currentHealth = currentHealth * 100;
        amount = currentHealth/maxHealth;



        bossHealthMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normSize * amount);
        bossHealthText.text = currentHealth + "/" + maxHealth;
        
        }
    }
}
