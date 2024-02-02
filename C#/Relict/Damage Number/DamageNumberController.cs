using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    TextMeshPro tmpro;
    float alpha;

    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TextMeshPro>();
        tmpro.color = new Color(tmpro.color.r, tmpro.color.g, tmpro.color.b, 1f);
        alpha = 1f;
        Destroy(this.gameObject, 7.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 1.5f * Time.deltaTime);

        alpha = alpha - 1.25f * Time.deltaTime;

        Color newColor = new Color(tmpro.faceColor.r, tmpro.faceColor.g, tmpro.faceColor.b, alpha);
        tmpro.faceColor = newColor;
    }
}
