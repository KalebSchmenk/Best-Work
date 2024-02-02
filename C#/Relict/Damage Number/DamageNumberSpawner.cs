using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    // Spawn damage number with a color
    public static void SpawnDamageNumber(Vector3 spawnPos, float damage, Color color)
    {
        var obj = new GameObject();
        obj.name = "Damage Number";
        obj.transform.position = spawnPos;
        obj.AddComponent<FaceCameraController>();
        obj.transform.localScale = new Vector3(-1, 1, 1);

        var textMesh = obj.AddComponent<TextMeshPro>();
        textMesh.font = GameManager.instance.damageNumberFont;
        textMesh.color = color;
        textMesh.faceColor = color;
        textMesh.fontSize = 7f;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.text = damage.ToString("#");

        
        var mat = obj.GetComponent<MeshRenderer>().material;
        mat = new Material(mat.shader = Shader.Find("TextMeshPro/Distance Field Overlay"));

        obj.AddComponent<DamageNumberController>();
    }

    // Spawn damage number
    public static void SpawnDamageNumber(Vector3 spawnPos, float damage)
    {
        var obj = new GameObject();
        obj.name = "Damage Number";
        obj.transform.position = spawnPos;
        obj.AddComponent<FaceCameraController>();
        obj.transform.localScale = new Vector3(-1, 1, 1);
        
        var textMesh = obj.AddComponent<TextMeshPro>();
        textMesh.fontSize = 7f;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.text = damage.ToString("#");

        textMesh.font = GameManager.instance.damageNumberFont;
        var mat = obj.GetComponent<MeshRenderer>().material;
        mat = new Material(mat.shader = Shader.Find("TextMeshPro/Distance Field Overlay"));

        obj.AddComponent<DamageNumberController>();
    }
}
