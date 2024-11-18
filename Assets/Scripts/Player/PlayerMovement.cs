using Environment.MoneyStack;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(0, 7)] private float speed;
    [SerializeField][Range(0.2f, 0.5f)] private float rotationSpeed;

    private CharacterController _controller;

    private DynamicJoystick _joystick;

    public float MovementMagnitude => _joystick.MovementEnabled ? _controller.velocity.magnitude / speed : 0;

    private Vector3 MovementDirection => new(
        _joystick.Horizontal * GetCharacterSpeed(),
        0,
        _joystick.Vertical * GetCharacterSpeed()
    );

    private Vector3 RotateDirection => Vector3.RotateTowards(
        _controller.transform.forward,
        MovementDirection,
        rotationSpeed,
        0
    );

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _joystick = FindObjectOfType<DynamicJoystick>();
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (!_joystick.MovementEnabled) return;

        _controller.SimpleMove(MovementDirection);

        transform.rotation = Quaternion.LookRotation(RotateDirection);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            other.GetComponent<MoneyStack>().CollectStack(transform);
        }
    }
    private float GetCharacterSpeed() => speed;
}