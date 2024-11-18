using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    private static readonly int MovementMagnitudeKey = Animator.StringToHash("MovementMagnitude");
    private static readonly int IsCarryingKey = Animator.StringToHash("IsCarrying");

    [SerializeField] private Animator animator;
    [SerializeField] private float animationSpeedMultiplier = 1.5f;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        animator.speed = animationSpeedMultiplier;
    }

    private void Update() => UpdateAnimation();

    private void UpdateAnimation()
    {
        float adjustedMagnitude = _playerMovement.MovementMagnitude * animationSpeedMultiplier;
        animator.SetFloat(MovementMagnitudeKey, adjustedMagnitude);
    }

    public void SetAnimationSpeed(float speed)
    {
        animationSpeedMultiplier = speed;
        animator.speed = speed;
    }

    public void StartCarryAnimation()
    {
        animator.SetBool(IsCarryingKey, true);
    }

    public void StopCarryAnimation()
    {
        animator.SetBool(IsCarryingKey, false);
    }
}