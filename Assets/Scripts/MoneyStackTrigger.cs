using UnityEngine;

namespace Environment.MoneyStack
{
    [RequireComponent(typeof(MoneyStack), typeof(BoxCollider))]
    public class MoneyStackTrigger : MonoBehaviour
    {
        [SerializeField] private float colliderOffset = 0.25f;
        private BoxCollider _boxCollider;
        private MoneyStack _moneyStack;

        private void Awake()
        {
            _moneyStack = GetComponent<MoneyStack>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Start() => InitBoxColliderSizes();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _moneyStack.CollectStack(other.transform);
            }
        }

        private void InitBoxColliderSizes()
        {
            var newCollideSize = new Vector3(
                _moneyStack.offsets.x * _moneyStack.rows + colliderOffset,
                _moneyStack.offsets.y * _moneyStack.layers + colliderOffset,
                _moneyStack.offsets.z * _moneyStack.columns + colliderOffset
            );

            _boxCollider.size = newCollideSize;
            _boxCollider.center = newCollideSize / 2 - Vector3.one * colliderOffset / 2;
        }
    }
}
