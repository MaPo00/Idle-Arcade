using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Actions
{
    public abstract class ActionZone : MonoBehaviour
    {
        private static readonly int Arc2 = Shader.PropertyToID("_Arc2");

        [SerializeField] protected SpriteRenderer spriteToFill;
        [SerializeField] protected ScriptableObjects.ActionZone config;
        [SerializeField][CanBeNull] private UnityEvent additionalLogic;

        private readonly HashSet<TweenerCore<float, float, FloatOptions>> _dotweenFillAnimations = new();

        protected float FillValue
        {
            get => spriteToFill.material.GetFloat(Arc2);
            set => spriteToFill.material.SetFloat(Arc2, Mathf.Clamp(value, 0, 360));
        }

        protected abstract void OnAction();

        protected virtual bool OnFillEnabled() => true;

        #region Triggers

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && OnFillEnabled()) StartFill();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) StartOutFill();
        }

        #endregion

        #region Fill Animations

        protected void StartFill()
        {
            ClearAnimations();

            _dotweenFillAnimations.Add(
                DOTween.To(
                        () => FillValue,
                        radius => FillValue = radius,
                        360, // Fill to 360 degrees
                        config.duration
                    )
                    .OnComplete(() =>
                    {
                        OnAction();
                        additionalLogic?.Invoke();
                    })
                    .SetDelay(config.fillDelay)
                    .SetEase(Ease.Linear)
            );
        }

        protected void StartOutFill()
        {
            ClearAnimations();

            _dotweenFillAnimations.Add(
                DOTween.To(
                        () => FillValue,
                        radius => FillValue = radius,
                        0, // Empty to 0 degrees
                        config.duration
                    )
                    .SetDelay(config.outFillDelay)
                    .SetEase(Ease.Linear)
            );
        }

        private void ClearAnimations()
        {
            foreach (var fillAnimation in _dotweenFillAnimations) fillAnimation.Kill();
            _dotweenFillAnimations.Clear();
        }

        #endregion
    }
}
