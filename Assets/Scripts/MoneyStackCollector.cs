using System.Collections;
using UnityEngine;

namespace Environment.MoneyStack
{
    [RequireComponent(typeof(MoneyStack), typeof(MoneyPolling))]
    public class MoneyStackCollector : MonoBehaviour
    {
        private MoneyPolling _moneyPolling;
        private MoneyStack _moneyStack;

        private void Awake()
        {
            _moneyStack = GetComponent<MoneyStack>();
            _moneyPolling = GetComponent<MoneyPolling>();
        }

        private void Start()
        {
            var simpleMoney = Instantiate(_moneyStack.moneyPrefab);
            Destroy(simpleMoney);
        }

        public void AddRealMoney() { }

        public void Collect()
        {
            StartCoroutine(AddMoneyToSavings());
        }

        private IEnumerator AddMoneyToSavings()
        {
            yield return new WaitForSeconds(_moneyPolling.takeMoneyTotalAnimationDuration);
        }
    }
}
