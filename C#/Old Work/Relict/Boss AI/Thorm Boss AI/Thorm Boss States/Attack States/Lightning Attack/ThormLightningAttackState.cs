using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThormLightningAttackState : EnemyBossBaseState
{
    ThormBossAIController aiController;
    public int lightningSpawnCount = 7;

    private Transform playerTrans;

    public override void EnterState(EnemyBossBaseController baseController)
    {
        print("Thorm boss entered lightning attack state!");

        aiController = (ThormBossAIController)baseController;

        playerTrans = GameManager.instance.player.transform;
    }

    public override void UpdateState()
    {
        //print("Updating attack state");
    }

    public override void ExitState()
    {
        print("Thorm boss exited lightning attack state!");

        Destroy(this);
    }

    public void SpawnLightning()
    {
        List<Vector3> spawnPosList = new List<Vector3>();

        for(int i = 0; i < lightningSpawnCount; i++)
        {
            Vector3 spawnPos;

            float randomX = playerTrans.position.x + UnityEngine.Random.Range(-35f, 35f);
            float randomZ = playerTrans.position.z + UnityEngine.Random.Range(-35f, 35f);
            float upY = playerTrans.position.y + 75;
            Vector3 skyPos = new Vector3(randomX, upY, randomZ);
            RaycastHit hit = RayCast(skyPos, Vector3.down, Mathf.Infinity, aiController.groundMask); 

            if (hit.collider == null)
            {
                spawnPos = playerTrans.position;
            }
            else
            {
                spawnPos = hit.point;
            }

            spawnPosList.Add(spawnPos);
        }

        foreach(Vector3 spawnPos in spawnPosList)
        {
            Instantiate(aiController.lightningAttack, spawnPos, Quaternion.identity);
        }
    }

    // Ground Check Raycast
    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len, LayerMask layerMask)
    {
        RaycastHit hit;

        //Debug.DrawLine(from, from + (dir * len), UnityEngine.Color.green, 20f, false); // Debug draw

        if (Physics.Raycast(from, dir, out hit, len, layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit;
        }

        return new RaycastHit();
    }
}
