using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    [SerializeField] float interactDistance;
    public PlayerInputActions playerControls;
    private InputAction interact;
    string tagInfo;
     private Camera camera;
     Transform hitObject;
     [SerializeField] Animator transition;


    void Awake(){
        playerControls = new PlayerInputActions();
        
    }

    private void OnEnable(){
        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable(){
        interact.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(interact.triggered){
            InteractInfo();
        }
    }


     public void InteractInfo(){
        RaycastHit hit;
        //var diaRay = new Ray(transform.position, Vector3.forward);
        Ray diaRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(diaRay, out hit, interactDistance)){
            tagInfo = hit.transform.tag;
            hitObject = hit.transform;
            if(tagInfo == "Hammer"){
                StartCoroutine(SceneTransition());
                
            }
        }
    }

    IEnumerator SceneTransition(){
        transition.SetTrigger("In");

        yield return new WaitForSeconds(1.25f);

        SceneManager.LoadScene("EndCutscene");
        
    }
}
