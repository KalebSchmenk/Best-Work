using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGuardController : MonoBehaviour
{
    [SerializeField] float _timeTillDestroy = 3.5f;
    [SerializeField] GameObject _visualObj;

    Material _matColor;

    private void Start()
    {
        StartCoroutine(DestroyIn());

        _matColor = _visualObj.GetComponent<MeshRenderer>().material;
    }


    void FixedUpdate()
    {
        transform.localScale += new Vector3(0.08f, 0.08f, 0.08f);

        Color color = _matColor.color;
        color.a -= 0.001f;
        color.a = Mathf.Clamp(color.a, 0, 1);
        _matColor.color = color;
        Debug.Log(_matColor.color);
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(_timeTillDestroy);

        Destroy(this.gameObject);
    }

}
