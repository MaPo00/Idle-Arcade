using UnityEngine;
using System.Collections;

public class CabbageFieldManager : MonoBehaviour
{
    public GameObject cabbagePrefab;
    public Transform[] plantPoints;
    public float respawnTime = 20f;

    void Start()
    {
        foreach (var point in plantPoints)
        {
            StartCoroutine(GrowCabbage(point));
        }
    }

    public void HarvestCabbage(Transform cabbageTransform)
    {
        Destroy(cabbageTransform.gameObject);
        StartCoroutine(GrowCabbage(cabbageTransform));
    }

    IEnumerator GrowCabbage(Transform point)
    {
        GameObject cabbage = Instantiate(cabbagePrefab, point.position, Quaternion.identity);
        cabbage.GetComponent<CabbageGrowth>().growthTime = 20f;
        cabbage.GetComponent<CabbageGrowth>().fieldManager = this;

        float currentTime = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = Vector3.one;

        while (currentTime < 20f)
        {
            if (cabbage == null)
            {
                yield break; // Вийти з корутини, якщо об'єкт було знищено
            }
            cabbage.transform.localScale = Vector3.Lerp(initialScale, finalScale, currentTime / 20f);
            currentTime += Time.deltaTime;
            yield return null;
        }

        if (cabbage != null)
        {
            cabbage.transform.localScale = finalScale;
        }
    }
}
