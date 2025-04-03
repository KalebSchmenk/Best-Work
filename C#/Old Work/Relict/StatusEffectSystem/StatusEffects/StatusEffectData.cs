using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="StatusEffects/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public GameObject EffectParticles;
    public float DamageOverTime;
    public float TickSpeed;
    public float Lifetime;
    public Color damageNumberColor = Color.white;

    public Sprite StatusEffectIcon;
    public GameObject StatusEffectPrefab;
}
