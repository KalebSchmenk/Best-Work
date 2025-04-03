using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [Header("References")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] private Transform groundCheckTransform;

    [Header("Custom Controller References")]
    [SerializeField] public InputHandler inputHandler;
    [SerializeField] public MovementController movementController;
    [SerializeField] public PlayerAnimationController animationController;
    [SerializeField] public AudioSource audioSource;

    [Header("Grounded Settings")]
    [SerializeField] private float groundedSphereCheckRadius = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip landedAudio;

    // Flags
    public bool CanMove { get; set; } = true;
    public bool CanRotate { get; set; } = true;
    public bool IsGrounded { get; set; } = true;

    public bool IsAiming { get; set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(GroundedCheck());
    }

    //  Is player grounded check
    private IEnumerator GroundedCheck()
    {
        while (true)
        {
            bool groundCollision = false;

            Collider[] colliders = Physics.OverlapSphere(groundCheckTransform.position, groundedSphereCheckRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Ground"))
                {
                    if (!IsGrounded)
                    {
                        // Landing
                        audioSource.PlayOneShot(landedAudio);
                    }

                    groundCollision = true;
                    break;
                }
            }

            if (groundCollision)
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }

            animationController.SetAnimatorBool(PlayerAnimationController.ANIM_GROUNDED, IsGrounded);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
