using UnityEngine;

namespace Environment.MoneyStack
{
    [RequireComponent(typeof(MoneyPolling), typeof(MoneyStackCollector))]
    public class MoneyStack : MonoBehaviour
    {
        [SerializeField] public GameObject pointer;
        [SerializeField] public GameObject moneyPrefab;

        [Header("Grid values")]
        [SerializeField]
        public Vector3 offsets = Vector3.zero;

        [SerializeField] public int layers, rows, columns;

        private MoneyPolling _moneyPolling;
        private MoneyStackCollector _moneyStackCollector;

        private void Awake()
        {
            _moneyPolling = GetComponent<MoneyPolling>();
            _moneyStackCollector = GetComponent<MoneyStackCollector>();
        }

        public void FillStack(int moneyCount, Vector3 spawnPoint)
        {
            for (var _ = 0; _ < moneyCount; _++)
            {
                _moneyPolling.AddMoneyObjectToStack(spawnPoint);
            }
        }


        public void CollectStack(Transform whereToFly)
        {
            _moneyPolling.Collect(whereToFly);
        }
    }
}
