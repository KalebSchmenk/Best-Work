using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathController : MonoBehaviour
{
    private MeleeEnemyAIController meleeEnemyAIController;
    [SerializeField] GameObject enemyObject;
    private bool isBoss;
    [SerializeField] private bool bossDead;

     public bool _bossBattle;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioSource bossMusicObject;
    [Header("Boss Health Bar")]
    [SerializeField] GameObject bossHealthBar;
    bool unMuteOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        meleeEnemyAIController = enemyObject.GetComponent<MeleeEnemyAIController>();

    }

    // Update is called once per frame
    void Update()
    {
        bossDead = meleeEnemyAIController._bossDead;

        if(bossDead){
            bossMusicObject.mute = true;
            _bossBattle = false;
            bossHealthBar.SetActive(false);
            if(unMuteOnce){
                gameMusic.mute = false;
                unMuteOnce = false;
            }
        }
    }
}
