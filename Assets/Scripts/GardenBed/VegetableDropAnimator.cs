using UnityEngine;
using DG.Tweening;

public class VegetableDropAnimator : MonoBehaviour
{
    [SerializeField] private float dropDuration = 1.5f; // Збільшено тривалість анімації
    [SerializeField] private float arcHeight = 2f; // Збільшено висоту арки для кращої видимості

    public void DropVegetableToBrewer(GameObject vegetable, Vector3 startPosition, Transform brewerTransform)
    {
        if (vegetable == null) return;

        Vector3 endPosition = brewerTransform.position + Vector3.up * 0.5f;

        vegetable.transform.SetParent(null);
        vegetable.transform.position = startPosition;

        Sequence dropSequence = DOTween.Sequence();

        // Невелике підняття перед основним рухом
        dropSequence.Append(vegetable.transform.DOMoveY(startPosition.y + 0.2f, 0.2f).SetEase(Ease.OutQuad));

        // Основний рух по дузі
        dropSequence.Append(vegetable.transform.DOJump(endPosition, arcHeight, 1, dropDuration - 0.2f).SetEase(Ease.OutQuad));

        // Обертання під час руху
        dropSequence.Join(vegetable.transform.DORotate(new Vector3(360, 360, 360), dropDuration, RotateMode.FastBeyond360));

        // Зміна розміру
        dropSequence.Join(vegetable.transform.DOScale(vegetable.transform.localScale * 1.2f, dropDuration / 2)
            .SetLoops(2, LoopType.Yoyo));

        dropSequence.OnComplete(() => {
            Destroy(vegetable);
        });
    }
}