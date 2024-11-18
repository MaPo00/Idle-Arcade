using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class VegetableAnimator : MonoBehaviour
{
    [SerializeField] private float collectDuration = 0.75f;
    [SerializeField] private float arcHeight = 2f;
    private BasketManager basketManager;

    private void Awake()
    {
        basketManager = GetComponent<BasketManager>();
    }

    public GameObject CollectVegetable(GameObject vegetablePrefab, Vector3 startPosition)
    {
        if (basketManager.IsBasketFull()) return null;

        GameObject vegetableObject = Instantiate(vegetablePrefab, startPosition, Quaternion.identity);
        AnimateVegetableToBasket(vegetableObject, startPosition);
        return vegetableObject;
    }

    private void AnimateVegetableToBasket(GameObject vegetableObject, Vector3 startPosition)
    {
        Vector3 initialTargetPosition = CalculateTargetPosition();
        Vector3 currentPosition = startPosition;
        float elapsedTime = 0f;

        DOTween.To(() => 0f, x => {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / collectDuration;

            Vector3 currentTargetPosition = CalculateTargetPosition();
            Vector3 midPoint = CalculateMidPoint(currentPosition, currentTargetPosition);

            Vector3 m1 = Vector3.Lerp(currentPosition, midPoint, t);
            Vector3 m2 = Vector3.Lerp(midPoint, currentTargetPosition, t);
            vegetableObject.transform.position = Vector3.Lerp(m1, m2, t);

            vegetableObject.transform.Rotate(Vector3.one * (360f * Time.deltaTime / collectDuration), Space.Self);

            float scale = 1f + 0.2f * Mathf.Sin(t * Mathf.PI);
            vegetableObject.transform.localScale = Vector3.one * scale;

        }, 1f, collectDuration)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            vegetableObject.transform.SetParent(basketManager.basketSpawnPoint);
            vegetableObject.transform.localPosition = basketManager.GetItemPosition(basketManager.GetCarriedItems().Count);
            vegetableObject.transform.localRotation = Quaternion.identity;
            vegetableObject.transform.localScale = Vector3.one;
            basketManager.AddItem(vegetableObject);
        });
    }

    private Vector3 CalculateTargetPosition()
    {
        int itemCount = basketManager.GetCarriedItems().Count;
        Vector3 localOffset = basketManager.GetItemPosition(itemCount);
        return basketManager.basketSpawnPoint.TransformPoint(localOffset);
    }

    private Vector3 CalculateMidPoint(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (start + end) / 2f;
        midPoint.y += arcHeight;
        return midPoint;
    }

    public void ClearBasket()
    {
        foreach (var item in basketManager.GetCarriedItems())
        {
            Destroy(item);
        }
        basketManager.ClearItems();
    }
}