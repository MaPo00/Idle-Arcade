using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Environment.MoneyStack
{
    [RequireComponent(typeof(MoneyStack))]
    public class MoneyPolling : MonoBehaviour
    {
        [Header("PUT MONEY ANIMATION")]
        [SerializeField]
        private float putToStackDurationsRandom = 0.5f;

        [Header("First jump")]
        [SerializeField]
        private float putJumpPower = 1.75f;
        [SerializeField] private float putJumpScatteringPower = 0.25f;
        [SerializeField] private float putJumpDuration = 0.5f;
        [SerializeField] private float putJumpRotation = 175f;

        [Header("Put to stack")]
        [SerializeField]
        private float putToStackDuration = 0.5f;

        [Header("TAKE MONEY ANIMATION")]
        [SerializeField]
        private float takeToPlayerDurationsRandom = 0.5f;

        [Header("First jump")]
        [SerializeField]
        private float takeJumpPower = 1.75f;
        [SerializeField] private float takeJumpScatteringPower = 1.5f;
        [SerializeField] private float takeJumpDuration = 0.5f;
        [SerializeField] private float takeJumpRotation = 175f;

        [Header("Take to player")]
        [SerializeField]
        private float takeToPlayerDuration = 0.5f;

        private readonly HashSet<GameObject> _activeMoneyObjects = new();
        private int _currentLayer = 1, _currentRow = 1, _currentColumn = 1;
        private ObjectPool<GameObject> _moneyPool;
        private MoneyStack _moneyStack;
        private int _poolCapacity;
        private Vector3 _stackCenter;

        public float takeMoneyTotalAnimationDuration => takeToPlayerDuration * 2;

        private void Awake()
        {
            _moneyStack = GetComponent<MoneyStack>();

            _stackCenter = new Vector3(
                _moneyStack.offsets.x * (_moneyStack.rows / 2f),
                _moneyStack.offsets.y * (_moneyStack.layers / 2f),
                _moneyStack.offsets.z * (_moneyStack.columns / 2f)
            );

            InitMoneyPoller();
        }

        public void AddMoneyObjectToStack(Vector3 spawnPoint)
        {
            var moneyObject = _moneyPool.Get();

            var filled = _activeMoneyObjects.Count >= _poolCapacity;
            _activeMoneyObjects.Add(moneyObject);

            moneyObject.transform.position = spawnPoint;

            var complexAddMoneyAnimation = DOTween.Sequence();

            // Jump & Rotation
            var jumpAndRotateDuration = putJumpDuration * Random.Range(1, 1 + putToStackDurationsRandom);
            complexAddMoneyAnimation.Append(
                moneyObject.transform
                    .DOMove(
                        Vectors.GetRandomVectorAbove(spawnPoint, putJumpPower, putJumpScatteringPower),
                        jumpAndRotateDuration
                    )
                    .SetEase(Ease.InOutCubic)
            );
            complexAddMoneyAnimation.Join(
                moneyObject.transform
                    .DOLocalRotate(
                        Vectors.GetRandomVectorRotation(putJumpRotation),
                        jumpAndRotateDuration
                    )
            );

            // Jump & Put
            var jumpAndPutDuration = putToStackDuration * Random.Range(1, 1 + putToStackDurationsRandom);
            complexAddMoneyAnimation.Append(moneyObject.transform
                .DOMove(_moneyStack.pointer.transform.position, jumpAndPutDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    if (!filled) return;

                    _moneyPool.Release(moneyObject);
                    _activeMoneyObjects.Remove(moneyObject);
                }));

            complexAddMoneyAnimation.Join(
                moneyObject.transform
                    .DOLocalRotate(Vector3.zero, jumpAndPutDuration)
                    .SetEase(Ease.InOutCubic)
            );

            UpdatePointerPosition();
        }

        public void Collect(Transform whereToFly)
        {
            foreach (var activeMoneyObject in _activeMoneyObjects)
            {
                activeMoneyObject.transform.DOKill();

                var complexAddMoneyAnimation = DOTween.Sequence();

                // Jump & Rotation
                var jumpAndRotateDuration = takeJumpDuration * Random.Range(1, 1 + takeToPlayerDurationsRandom);
                complexAddMoneyAnimation.Append(
                    activeMoneyObject.transform
                        .DOMove(
                            Vectors.GetRandomVectorAbove(activeMoneyObject.transform.position, takeJumpPower, takeJumpScatteringPower),
                            jumpAndRotateDuration
                        )
                        .SetEase(Ease.InOutCubic)
                );
                complexAddMoneyAnimation.Join(
                    activeMoneyObject.transform
                        .DOLocalRotate(
                            Vectors.GetRandomVectorRotation(takeJumpRotation),
                            jumpAndRotateDuration
                        )
                );

                // Jump & Take
                var jumpAndTakeDuration = takeToPlayerDuration * Random.Range(1, 1 + takeToPlayerDurationsRandom);

                activeMoneyObject.transform.SetParent(whereToFly);

                complexAddMoneyAnimation.Append(activeMoneyObject.transform
                    .DOLocalMove(Vector3.up, jumpAndTakeDuration)
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() => _moneyPool.Release(activeMoneyObject)));

                complexAddMoneyAnimation.Join(
                    activeMoneyObject.transform
                        .DOLocalRotate(Vector3.zero, jumpAndTakeDuration)
                        .SetEase(Ease.InOutCubic)
                );
                complexAddMoneyAnimation.Join(
                    activeMoneyObject.transform
                        .DOScale(Vector3.zero, jumpAndTakeDuration)
                        .SetDelay(jumpAndTakeDuration * 0.25f)
                        .SetEase(Ease.InOutCubic)
                );
                complexAddMoneyAnimation.Append(activeMoneyObject.transform.DOScale(Vector3.one, 0.01f));
            }

            _activeMoneyObjects.Clear();
            _currentColumn = 1;
            _currentRow = 1;
            _currentLayer = 1;
            _moneyStack.pointer.transform.localPosition = Vector3.zero;
        }

        private void UpdatePointerPosition()
        {
            var p = _moneyStack.pointer.transform.localPosition;

            if (_currentColumn < _moneyStack.columns)
            {
                _currentColumn++;

                p.z += _moneyStack.offsets.z;
                _moneyStack.pointer.transform.localPosition = p;
            }
            else if (_currentRow < _moneyStack.rows)
            {
                _currentRow++;
                _currentColumn = 1;

                p.z = 0;
                p.x += _moneyStack.offsets.x;
                _moneyStack.pointer.transform.localPosition = p;
            }
            else if (_currentLayer < _moneyStack.layers)
            {
                _currentLayer++;
                _currentColumn = 1;
                _currentRow = 1;

                p.x = 0;
                p.z = 0;
                p.y += _moneyStack.offsets.y;
                _moneyStack.pointer.transform.localPosition = p;
            }
            else
            {
                if (_moneyStack.pointer.transform.localPosition == _stackCenter) return;
                _moneyStack.pointer.transform.localPosition = _stackCenter;
            }
        }

        private void InitMoneyPoller()
        {
            _poolCapacity = _moneyStack.columns * _moneyStack.rows * _moneyStack.layers;

            _moneyPool = new ObjectPool<GameObject>(
                () => {
                    var money = Instantiate(_moneyStack.moneyPrefab, gameObject.transform);
                    money.transform.localScale = Vector3.one * 7; // Set initial scale
                    return money;
                },
                money => {
                    money.SetActive(true);
                    money.transform.localScale = Vector3.one * 7; // Reset scale
                },
                money =>
                {
                    money.SetActive(false);
                    money.transform.SetParent(gameObject.transform);
                    money.transform.position = gameObject.transform.position;
                },
                money => Destroy(money.gameObject),
                true,
                _poolCapacity,
                _poolCapacity + 100
            );
        }
    }
}
