using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BotAnimation : MonoBehaviour
{
    private static readonly int MovementMagnitudeKey = Animator.StringToHash("MovementMagnitude");
    private static readonly int IsHealingKey = Animator.StringToHash("IsHealing");

    [SerializeField] private Animator animator;
    [SerializeField] private float animationSpeedMultiplier = 1.5f;

    private void Awake()
    {
        animator.speed = animationSpeedMultiplier;
    }

    public void UpdateAnimation(float movementMagnitude)
    {
        animator.SetFloat(MovementMagnitudeKey, movementMagnitude);
    }

    public void SetAnimationSpeed(float speed)
    {
        animationSpeedMultiplier = speed;
        animator.speed = speed;
    }

    public void StartHealingAnimation()
    {
        animator.SetBool(IsHealingKey, true);
    }

    public void StopHealingAnimation()
    {
        animator.SetBool(IsHealingKey, false);
    }
}