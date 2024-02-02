using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField] private float moveUpTo = 6.5f; // Move up distance
    [SerializeField] private float speed = 4.5f; // How fast it moves up
    [SerializeField] private float power = 10.0F; // How powerful it hits enemies
    [SerializeField] private float damage = 5f; // How much damage it does to enemies
    [SerializeField] private float stunLength = 5f; // How long to stun enemy

    bool moving = true;

    Vector3 targetPos;
    List<GameObject> launchedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position + (transform.up * moveUpTo);
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving) return; // Guard Clause


        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            moving = false;
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }


    // Throws Enemy upward
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!moving) return; // Guard Clause

            bool goForLaunch = true; 

            foreach (GameObject launched in launchedObjects) // Loop over already launched objs
            {
                if (GameObject.ReferenceEquals(launched, collision.gameObject)) // Abort launch if obj is in list
                {
                    goForLaunch = false;
                    break;
                }
            }

            if (goForLaunch) // Launch the object if true
            {
                print("Launching obj");

                launchedObjects.Add(collision.gameObject);

                var obj = collision.gameObject;
                var pillarToObj = obj.transform.position - this.transform.position;
                pillarToObj.y = 0;
                pillarToObj.Normalize();

                ITakeDamage damageInterface;
                obj.TryGetComponent<ITakeDamage>(out damageInterface);

                if (damageInterface != null)
                {
                    damageInterface.TakeDamage(collision.contacts[0].point, Color.white, damage, true);
                }

                ILaunchable launchInterface;
                obj.TryGetComponent<ILaunchable>(out launchInterface);

                if (launchInterface != null)
                {
                    launchInterface.Launch(pillarToObj, power * 1.5f, stunLength);
                }
            }
        }
    }
}
