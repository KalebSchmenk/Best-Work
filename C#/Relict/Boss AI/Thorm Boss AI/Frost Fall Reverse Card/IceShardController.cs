using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardController : MonoBehaviour
{
    [NonSerialized] public FrostFallController frostFallController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            frostFallController.ShardHitPlayer(collision.GetContact(0).point);
        }

        Destroy(this.gameObject);
    }
}
