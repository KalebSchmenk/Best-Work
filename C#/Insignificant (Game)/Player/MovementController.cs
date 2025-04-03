using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCam;
    private PlayerController playerController;
    private InputHandler inputHandler;
    private Rigidbody rb;

    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;

    [Header("Rotate Speed")]
    [SerializeField] private float rotateSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource jumpSource;

    public bool ControlAnimator = true;

    // Private vars
    private Vector3 currentMoveDir = Vector3.zero;
    private Vector3 targetMoveDir = Vector3.zero;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputHandler = playerController.inputHandler;
        rb = playerController.rb;

        SubscribeToInputs();
    }

    private void OnDestroy()
    {
        UnSubscribeToInputs();
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (ControlAnimator) playerController.animationController.SetAnimatorFloat(PlayerAnimationController.ANIM_SPEED, currentMoveDir.magnitude);
    }

    private void OnMoveInputChange(Vector2 dir)
    {
        Vector3 moveDir = new Vector3(dir.x, 0, dir.y);

        targetMoveDir = moveDir;

        if (targetMoveDir != Vector3.zero && playerController.IsGrounded)
        {
            if (!walkSource.isPlaying) walkSource.Play();
        }
        else
        {
            walkSource.Stop();
        }
    }

    private void MovePlayer()
    {
        if (!playerController.CanMove) return;

        // Lerp current move dir toward target move dir
        currentMoveDir = Vector3.MoveTowards(currentMoveDir, targetMoveDir, 5f);
        Vector3 inputDir = new Vector3(currentMoveDir.x, 0, currentMoveDir.z);

        // Get camera forward and right vectors
        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;

        // Clear vector y vals (they are not needed)
        camForward.y = 0;
        camRight.y = 0;

        // Normalize vectors for better values
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        // Calulate relative directions based on where player is looking and what input they gave
        Vector3 forwardRel = inputDir.z * camForward;
        Vector3 rightRel = inputDir.x * camRight;

        // Calculate player move direction based upon where they were facing and the input they gave
        Vector3 moveDir = forwardRel + rightRel;

        // Give speed boost and make sure framerate does not effect speed
        moveDir *= moveSpeed;

        // Animation stuff
        playerController.animationController.SetAnimatorFloat(PlayerAnimationController.ANIM_X, inputDir.x);
        playerController.animationController.SetAnimatorFloat(PlayerAnimationController.ANIM_Y, inputDir.z);

        // Adjust the movement dir to the slope we are on
        moveDir = AdjustMoveDirToSlope(moveDir);

        // Start rotating player to face calulated move dir
        RotatePlayerToward(moveDir);

        // Reset rigidbody
        moveDir.y = rb.linearVelocity.y;

        // Add calculated move direction to current player position
        rb.linearVelocity = moveDir;
    }

    private Vector3 AdjustMoveDirToSlope(Vector3 moveDir)
    {
        var ray = new Ray(this.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            var adjustedMoveDir = slopeRotation * moveDir;

            if (adjustedMoveDir.y < 0)
            {
                return adjustedMoveDir;
            }
        }

        return moveDir;
    }

    // Set Player Rotation to match move dir
    private void RotatePlayerToward(Vector3 moveDir)
    {
        if (!playerController.CanRotate) return;

        if (playerController.IsAiming)
        {
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            this.transform.rotation = Quaternion.LookRotation(camForward);
            return;
        }

        // Get direction player is moving to
        Vector3 faceDir = ((this.transform.position + moveDir) - this.transform.position).normalized;

        // Get rotate step speed
        float step = rotateSpeed * Time.deltaTime;

        // Get a rotation that is a step towards the current player movement direction
        Vector3 curRotDir = Vector3.RotateTowards(this.transform.forward, faceDir, step, 0.0f);

        this.transform.rotation = Quaternion.LookRotation(curRotDir);
    }

    private void OnRoll()
    {
        if (!playerController.IsGrounded || !playerController.CanMove) return;

        playerController.animationController.SetAnimatorTrigger(PlayerAnimationController.ANIM_ROLL);

        // TODO
        // STOP AIM ANIM - ROLL - IF PLAYER WAS AIMING SET THEM BACK INTO AIMING ANIM
        print("TODO: Roll thing");
    }

    private void OnUse()
    {
        var arr = Physics.OverlapSphere(this.transform.position, 1.5f);

        foreach (var item in arr)
        {
            if (item.TryGetComponent<IInteractable>(out IInteractable interact))
            {
                bool goodInteract = interact.Interact();
                if (goodInteract) 
                {
                    StartCoroutine(UseInteract());
                    break;
                }               
            }
        }
    }

    private IEnumerator UseInteract()
    {
        playerController.animationController.SetAnimatorTrigger(PlayerAnimationController.ANIM_USE);
        playerController.CanMove = false;
        yield return new WaitForSeconds(4f);
        playerController.animationController.SetAnimatorTrigger (PlayerAnimationController.ANIM_STOP_USE);
        yield return new WaitForSeconds(1f);
        playerController.CanMove = true;
    }

    public void BoostUp(float force)
    {
        var curVel = rb.linearVelocity;

        curVel.y = force;

        rb.linearVelocity = curVel;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public float GetMoveSpeed() { return moveSpeed; }

    public void SetMoveDir(Vector3 moveDir) { currentMoveDir = moveDir; }
    public Vector3 GetMoveDir() { return currentMoveDir; }

    public void SetTargetDir(Vector3 moveDir) { targetMoveDir = moveDir; }
    public Vector3 GetTargetDir() { return targetMoveDir; }

    public void SubscribeToInputs()
    {
        inputHandler.OnMoveInputRecieved += OnMoveInputChange;
        inputHandler.OnRollInputRecieved += OnRoll;
        inputHandler.OnInteractInputRecieved += OnUse;
    }
    public void UnSubscribeToInputs()
    {
        inputHandler.OnMoveInputRecieved -= OnMoveInputChange;
        inputHandler.OnRollInputRecieved -= OnRoll;
        inputHandler.OnInteractInputRecieved -= OnUse;
    }
}
