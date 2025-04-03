using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction interact;
    private InputAction skip;
    [SerializeField] float interactDistance;
    string tagInfo;
    [Header("Dialogue Objects")]
    [SerializeField] GameObject dialogueScreen;
    [SerializeField] TextMeshProUGUI npcName;
    [SerializeField] TextMeshProUGUI npcDialogue;
    [SerializeField] GameObject npcNameObject;
    [SerializeField] GameObject npcDialogueObject;
    [SerializeField] Transform playerObject;
    Transform npcObject;
    [Header("Dialogue Lines")]
    [SerializeField] int numLines;
    int currentLine;
    [Header("NPC 1")]
    [SerializeField] string jSmith1;
    [SerializeField] string jSmith2;
    string jSmith;
    [Header("Typing effect speed")]
    [SerializeField] float typeSpeed = 0.02f;
    float distance;
    private Coroutine displayDialogueCoroutine;
    
    
    private Camera camera;

    void Start(){
        camera = Camera.main;
    }

    void Awake(){
        playerControls = new PlayerInputActions();
        
    }

    void Update()
    {
        if(interact.triggered){
            InteractInfo();
        }

        if(currentLine > numLines){
            currentLine = 0;
        }

        if(currentLine == 0 || dialogueScreen.activeSelf == false){
            ClearDialogue();
        }

        if(npcObject != null){
        float distance = Vector3.Distance(playerObject.position, npcObject.position);
        if(distance > interactDistance){
            ClearDialogue();
        }
        }
    }

    private void OnEnable(){
        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable(){
        interact.Disable();
    }

    public void InteractInfo(){
        RaycastHit hit;
        //var diaRay = new Ray(transform.position, Vector3.forward);
        Ray diaRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(diaRay, out hit, interactDistance)){
            tagInfo = hit.transform.tag;
            npcObject = hit.transform;
        if(tagInfo != "Untagged"){
            currentLine += 1;
            Dialogue(tagInfo);
            Debug.Log(currentLine);
        }
        else if(tagInfo == "Untagged"){
            Dialogue("null");
        }
        }

    }

    public void Dialogue(string hitTag){
        if(tagInfo != "null"){
            dialogueScreen.SetActive(true);
            if(tagInfo == "Dumont"){
                npcName.text = tagInfo;
                dialogueScreen.SetActive(true);
                if(displayDialogueCoroutine != null){
                   StopCoroutine(displayDialogueCoroutine);
                }
                    if(currentLine == 1){
                        displayDialogueCoroutine = StartCoroutine(DisplayDialogueEffect(jSmith1));
                    }
                    if(currentLine == 2){
                        displayDialogueCoroutine = StartCoroutine(DisplayDialogueEffect(jSmith2));   
                    }


            }
            else{
            npcDialogue.text = "";
            npcName.text = "";
            dialogueScreen.SetActive(false);
            }
        }
    }

    private IEnumerator DisplayDialogueEffect(string dialogue){
        npcDialogue.text = "";
        foreach (char letter in dialogue.ToCharArray()){
            npcDialogue.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void ClearDialogue(){
        npcDialogue.text = "";
        npcName.text = "";
        currentLine = 0;
        dialogueScreen.SetActive(false);

    }
}
