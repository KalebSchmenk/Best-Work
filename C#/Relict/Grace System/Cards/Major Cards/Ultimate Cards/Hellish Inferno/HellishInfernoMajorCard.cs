using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellishInfernoMajorCard : MajorCardBase
{
    // Start is called before the first frame update
    [Header("Hellish Inferno Refs and Settings")]
    public GameObject targetGroundReticle; // Gameobject of targetting ground reticle
    public GameObject solarBeam; // Solar Beam object for VFX and damaging
    public LayerMask defaultLayerMask; // Layer mask for ray cast
    public float maxPathDistance = 5f; // The length of the path for the Inferno to travel.
    public float damageOutput = 10f;
    public float enableDamageAfterSpawnedIn = 1f;
    public float destroyAfterEnableDamageIn = 0.5f;
    
    public bool CanStunEnemies;
    public float TornadoSpeed;
    
    public StatusEffectData HellfireStatusEffectData; // Hellfire status effect data
    public StatusEffectData DazedStatusEffectData; // Hellfire status effect data

    public AudioClip targetLockedSound; // Target Locked sound effect

    private Vector3 tornadoDestination;
    private Vector3 VFX_Pos;

    GameObject spawnedTargetGroundReticle; // Spawned ground targetting reticle
    GameObject cam; // Camera reference
    PlayerController playerController; // Player controller script reference

    //public int TornadosToFire;
    //public int EffectArcLength;

    // On ability key up do solar beam ability and destroy target reticle
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return. Or if target reticle coroutine is empty

        //Arc TornadoArc = new Arc(GetLookAngle(), EffectArcLength);
        
        //foreach(float angle in TornadoArc.GetEvenlyDistrbutedAnglesInArc(TornadosToFire))
        //{
            
            //Vector3 noAngle = cam.transform.forward;
            //Quaternion spreadAngle = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
            //Vector3 newVector = spreadAngle * noAngle;

            OrientInferno(cam.transform.forward);

            print("Hellish Inferno key down");

            print(this + " called its ability");

            if (spawnedTargetGroundReticle == null) // Guard clause in case no target reticle was ever shown to player
            {
                Debug.LogWarning("No place to Hellish Inferno. Hellish Inferno summon aborted");
                return;
            }

            HellishInferno();
        //}
            
        //Invoke(nameof(SpawnVFX), enableDamageAfterSpawnedIn);

        StartCooldown();
    }

    // Spawns solar beam
    private void HellishInferno()
    {
        var hellishInfernoSpawnedObj = Instantiate(solarBeam, player.transform.position, solarBeam.transform.rotation);
        HellishInfernoController hellishInfernoController = hellishInfernoSpawnedObj.GetComponent<HellishInfernoController>();
        hellishInfernoController.damage = damageOutput;
        hellishInfernoController.enableDamageIn = enableDamageAfterSpawnedIn;
        hellishInfernoController.destination = tornadoDestination;
        hellishInfernoController.tornadoSpeed = TornadoSpeed;
        hellishInfernoController.CanStun = CanStunEnemies;
        hellishInfernoController.DazedStatusEffectData = DazedStatusEffectData;
        hellishInfernoController.HellfireStatusEffectData = HellfireStatusEffectData;

        DestroyTargetReticle();
    }

    public float GetLookAngle()
    {
        return Mathf.Atan2(cam.transform.forward.y * maxPathDistance, cam.transform.forward.z * maxPathDistance) * Mathf.Rad2Deg;
    }

    public void SpreadProjectilesEvenly(int count, Arc currentGunArc)
    {
        //float lookAngle = spawnedTargetGroundReticle.transform.forward.z;


        //Debug.Log($"ARC Origin: {currentGunArc.originAngle}, MIN: {currentGunArc.minAngle}, MAX: {currentGunArc.maxAngle}");
        //float[] bulletAngles = currentGunArc.GetEvenlyDistrbutedAnglesInArc(count);

        //foreach (float bulletAngle in bulletAngles)
        //{
        //    Debug.Log(bulletAngle);
        //    SolarBeam();
        //}
    }

    // Spawns target reticle on the ground
    private void SpawnTargetReticle()
    {
        spawnedTargetGroundReticle = Instantiate(targetGroundReticle, player.transform.position, player.transform.rotation);
        spawnedTargetGroundReticle.gameObject.name = "Hellish Inferno";
    }

    // Destroys target reticle on ground
    private void DestroyTargetReticle()
    {
        Destroy(spawnedTargetGroundReticle);

        playerController.PlaySound(targetLockedSound); // Plays target locked sound
    }

    //Determines placement and location data of the inferno.
    //Coroutine
    private void OrientInferno(Vector3 Dir)
    {
        var rayHit = RayCast(cam.transform.position, Dir, maxPathDistance);


        if (rayHit.collider != null) // If ray from hit something on the layer mask
        {
            if (spawnedTargetGroundReticle == null) SpawnTargetReticle(); // We need it if it doesnt exist

            spawnedTargetGroundReticle.transform.position = rayHit.point;
            var lookRot = rayHit.normal;
            spawnedTargetGroundReticle.transform.rotation = Quaternion.FromToRotation(spawnedTargetGroundReticle.transform.up, lookRot) * spawnedTargetGroundReticle.transform.rotation;
            tornadoDestination = spawnedTargetGroundReticle.transform.position;

            if (spawnedTargetGroundReticle.transform.up.y > 0.99f) // If ghost is upright face the camera
            {
                var upLookRot = cam.transform.forward;
                upLookRot.y = 0;
                spawnedTargetGroundReticle.transform.rotation = Quaternion.LookRotation(upLookRot);
            }
        }
        else // If ray from cam did not hit something on the layer mask
        {
            var camRot = Dir;
            camRot.y = 0;

            var rayDownHit = RayCast(cam.transform.position + (camRot * maxPathDistance), Vector3.down, Mathf.Infinity);
            if (rayDownHit.collider != null)
            {
                if (spawnedTargetGroundReticle == null) SpawnTargetReticle(); // We need it if it doesnt exist
                
                spawnedTargetGroundReticle.transform.position = rayDownHit.point;
                spawnedTargetGroundReticle.transform.rotation = Quaternion.LookRotation(camRot);
                tornadoDestination = spawnedTargetGroundReticle.transform.position;
            }
            else // No spawn could be found. Destroy ghost
            {
                DestroyTargetReticle();
            }
        }
    }

    // Ground Check Raycast
    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len)
    {
        RaycastHit hit;

        //Debug.DrawLine(from, from + (dir * maxSpawnDistance), UnityEngine.Color.green); // Debug draw

        if (Physics.Raycast(from, dir, out hit, len, defaultLayerMask))
        {
            return hit;
        }

        return new RaycastHit();
    }


    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameObject.FindGameObjectWithTag("MainCamera");

        playerController = player.GetComponent<PlayerController>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    public override void SpawnVFX()
    {
        Vector3 rotation = vfx.gameObject.transform.rotation.eulerAngles;
        rotation += new Vector3(-90f, 0, 0);

        Instantiate(vfx.gameObject, VFX_Pos, Quaternion.Euler(rotation));
    }
}

public class Arc
{
    private float originAngle;
    private float minAngle;
    private float maxAngle;

    public Arc(float _originAngle = 0f, float _offsetAngle = 0f)
    {
        originAngle = _originAngle;
        minAngle = originAngle - _offsetAngle / 2;
        maxAngle = originAngle + _offsetAngle / 2;
    }


    public float GetRandomAngleInArc()
    {
        return Random.Range(minAngle, maxAngle);
    }

    public float[] GetEvenlyDistrbutedAnglesInArc(int count)
    {
        List<float> points = new List<float>();

        float stepAngle = (2 * (maxAngle - originAngle)) / count;
        for (int i = 0; i < count; i++)
        {
            points.Add(minAngle + (stepAngle * i));
        }

        return points.ToArray();
    }
}
