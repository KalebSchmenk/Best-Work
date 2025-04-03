using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SendToLevelController : MonoBehaviour
{
    public string _sendToLevel;
    [SerializeField] private Image _blackImg;
    [SerializeField] private Animator _anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_sendToLevel != null)
            {
                StartCoroutine(Fading());
            }
        }
    }

    private IEnumerator Fading()
    {
        _anim.SetBool("Fade", true);
        yield return new WaitUntil(() => _blackImg.color.a == 1);
        SceneManager.LoadScene(_sendToLevel);
    }
}
