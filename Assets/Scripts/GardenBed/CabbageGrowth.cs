using UnityEngine;
using System.Collections;

public class CabbageGrowth : MonoBehaviour
{
    public float growthTime = 20f;
    public CabbageFieldManager fieldManager;
    private bool isFullyGrown = false;

    void Start()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float currentTime = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = Vector3.one;

        while (currentTime < growthTime)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, currentTime / growthTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        isFullyGrown = true;
    }

    // Додаємо новий публічний метод
    public bool IsFullyGrown()
    {
        return isFullyGrown;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isFullyGrown)
        {
            PlayerHarvest playerHarvest = other.GetComponent<PlayerHarvest>();
            if (playerHarvest != null && !playerHarvest.IsBasketFull())
            {
                fieldManager.HarvestCabbage(transform);
                Destroy(gameObject);
            }
        }
    }
}