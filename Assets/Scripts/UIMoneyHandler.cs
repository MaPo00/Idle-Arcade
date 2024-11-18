using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using Managers.SaveManager;
using TMPro;
using UnityEngine;
using Utils.Extensions;

namespace UI.GUI.Money
{
    public class UIMoneyHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private GameObject moneyImage;
        [SerializeField] private float animationDuration;

        [CanBeNull] private TweenerCore<Vector3, Vector3, VectorOptions> _lastAnimation;

        private void Awake() => SaveManager.Instance.OnMoneyChanged.AddListener(OnMoneyChangedListener);

        private void OnDestroy() => SaveManager.Instance.OnMoneyChanged.RemoveListener(OnMoneyChangedListener);


        private void OnMoneyChangedListener(int money)
        {
            moneyText.text = money.KorMFormat();

            if (_lastAnimation != null && _lastAnimation.IsPlaying()) return;

            _lastAnimation = moneyImage.transform
                .DOScale(1.1f, animationDuration / 2).OnComplete(() =>
                    moneyImage.transform.DOScale(1, animationDuration / 2)
                );
        }
    }
}