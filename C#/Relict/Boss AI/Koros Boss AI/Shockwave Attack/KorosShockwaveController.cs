using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosShockwaveController : MonoBehaviour
{
    public float damageOutput = 5f; // Shockwave damage outout
    public float speed = 20f; // Collider move speed
    bool hitPlayer = false; // Has hit player bool
    public List<GameObject> DamageControllerList = new List<GameObject>(); // List of gameobject that have colliders
    public GameObject parentObj; // Parent obj of all above gameobjects

    // Audio code
    AudioSource audioSource;

    public AudioClip shockwaveSound; // shockwave audio clip

    private void Start()
    {
        SetDrawRefs();

        SetRotsAndVars();

        Destroy(this.gameObject, 10f);

        Invoke("ActivateShockwave", 0.1f);

        audioSource = GetComponent<AudioSource>(); // Audio source code
    }

    private void SetDrawRefs()
    {
        for (int i = 0; i < DamageControllerList.Count; i++)
        {
            if (DamageControllerList[i] == null) continue;

            var lineController = DamageControllerList[i].GetComponent<KorosShockwaveLineController>();

            if (i != DamageControllerList.Count - 1)
            {
                lineController.objToDrawTo = DamageControllerList[i + 1];
            }
        }
    }

    private void OnDestroy()
    {
        print("Attack over");
        Destroy(this.gameObject);
    }

    // If a collider hit a player
    // Makes sure player only takes damage once even if multiple colliders were hit
    public void HitPlayer(ITakeDamage damageable, Vector3 collisionPoint)
    {
        if (hitPlayer) return;

        damageable.TakeDamage(collisionPoint, Color.white, damageOutput, true);
        hitPlayer = true;
        print("Hit player!");
    }

    // Starts coroutine to set objs rots and vars
    private void SetRotsAndVars()
    {
        StartCoroutine(RotsAndVarsSetting());
    }

    // Coroutine that sets rots and vars over a few frames
    IEnumerator RotsAndVarsSetting()
    {
        int i = 0;
        int j = 0;
        foreach (var gameObj in DamageControllerList)
        {
            gameObj.gameObject.transform.eulerAngles = new Vector3(0, i, 0);
            var controller = gameObj.GetComponent<KorosShockwaveDamageController>();
            controller.manager = this;
            controller.speed = speed;

            i++;
            j++;

        }
        yield return null;
    }

    // Activates shockwave
    public void ActivateShockwave()
    {
        parentObj.SetActive(true);

        PlaySound(shockwaveSound);
    }

    public void PlaySound(AudioClip clip) // Allows the boss to play sounds
    {
        audioSource.PlayOneShot(clip);
    }
}
