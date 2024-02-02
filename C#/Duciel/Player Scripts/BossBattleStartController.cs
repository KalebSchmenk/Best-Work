using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleStartController : MonoBehaviour
{
    [Header("Music Controller")]
    public bool _bossBattle;
    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioSource bossMusicObject;
    [SerializeField] AudioClip bossMusicClip;
    [Header("Boss Health Bar")]
    [SerializeField] GameObject bossHealthBar;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            _bossBattle = true;
            bossMusicObject.clip = bossMusicClip;
            bossMusicObject.Play();
            gameMusic.mute = true;
            bossHealthBar.SetActive(true);
            Destroy(gameObject);

        }
    }


}
