using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    [SerializeField] private GameObject[] _localArr;
    [SerializeField] ParticleSystem _fire;

    private List<ParticleSystem> _fireList = new List<ParticleSystem>();

    [SerializeField] private float _destroyIn = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            StartCoroutine(StartFire());
        }
    }

    private IEnumerator StartFire()
    {
        foreach (GameObject local in _localArr)
        {
            _fireList.Add(Instantiate(_fire, local.transform.position, Quaternion.identity));
        }

        yield return new WaitForSeconds(_destroyIn);

        foreach (var item in _fireList)
        {
            Destroy(item);
        }

        Destroy(this.gameObject);
    }

}
