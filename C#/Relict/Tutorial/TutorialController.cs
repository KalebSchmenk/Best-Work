using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{
    public bool inWaveOne = false;
    public bool inWaveTwo = false;
    public bool hadATarotReading = false;

    public List<AIMain> waveOneList = new List<AIMain>();
    public List<AIMain> waveTwoList = new List<AIMain>();

    public UnityEvent WaveOneDestroyed;
    public UnityEvent TarotReading;
    public UnityEvent AllEnemiesDead;

    private void Start()
    {
        GameManager.instance.UpdateObjective("Listen to The Magician!");
    }

    // Update is called once per frame
    void Update()
    {
        if (!hadATarotReading) CheckForReading();

        if (!inWaveOne && !inWaveTwo) return; // Not in any wave
        ProcessEnemies();
    }

    private void ProcessEnemies()
    {       
        if (inWaveOne)
        {
            bool enemyAlive = false;
            foreach (AIMain enemy in waveOneList)
            {
                if (!enemy.aiDead.isDead) enemyAlive = true;
            }

            // If all wave one enemies are dead (no enemy alive)
            if (!enemyAlive)
            {
                inWaveOne = false;
                WaveOneDestroyed?.Invoke();
            }
        }
        
        if (inWaveTwo)
        {
            bool enemyAlive = false;
            foreach (AIMain enemy in waveTwoList)
            {
                if (enemy == null) continue;
                if (!enemy.aiDead.isDead) enemyAlive = true;
            }

            // If all wave two enemies are dead (no enemy alive)
            if (!enemyAlive)
            {
                inWaveTwo = false;
                AllEnemiesDead?.Invoke();
            }
        }
    }

    public void StartWaveOne()
    {
        inWaveOne = true;
    }

    private void CheckForReading()
    {
        // If the tarot reading menu was ever opened, we know the player had a reading
        if (GameManager.instance.tarotReadingManager.tarotReadingMenu.activeSelf)
        {
            hadATarotReading = true;
            inWaveTwo = true;
            foreach (AIMain enemy in waveTwoList)
            {
                enemy.gameObject.SetActive(true);
            }
            TarotReading?.Invoke();
        }
    }
}
