using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public const string ANIM_X = "X";
    public const string ANIM_Y = "Y";
    public const string ANIM_SPEED = "Speed";
    public const string ANIM_GROUNDED = "OnGround";
    public const string ANIM_JUMP = "Jump";
    public const string ANIM_CROUCH = "Crouch";
    public const string ANIM_SHOOT = "Shoot";
    public const string ANIM_RELOADING = "Reloading";
    public const string ANIM_AIMING = "Aiming";
    public const string ANIM_DEAD = "Dead";
    public const string ANIM_USE = "Use";
    public const string ANIM_STOP_USE = "StopUse";
    public const string ANIM_RUN_STOP = "RunStop";
    public const string ANIM_ROLL = "Roll";
    public const string ANIM_SPRINT = "Sprint";
    public const string ANIM_PICKUP = "Pickup";

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    public void SetAnimatorBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetAnimatorFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetAnimatorTrigger(string name)
    {
        animator.SetTrigger(name);
    }
}
