using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TomatoFieldManager : MonoBehaviour
{
    public GameObject tomatoPrefab;
    public Transform[] plantPoints;
    public float respawnTime = 2f;

    private Dictionary<Transform, GameObject> activeTomatoes = new Dictionary<Transform, GameObject>();

    void Start()
    {
        foreach (var point in plantPoints)
        {
            StartCoroutine(GrowTomato(point));
        }
    }

    public void HarvestTomato(Transform point)
    {
        if (activeTomatoes.ContainsKey(point))
        {
            Destroy(activeTomatoes[point]);
            activeTomatoes.Remove(point);
        }

        StartCoroutine(GrowTomato(point));
    }

    IEnumerator GrowTomato(Transform point)
    {
        if (activeTomatoes.ContainsKey(point))
        {
            Destroy(activeTomatoes[point]);
            activeTomatoes.Remove(point);
        }

        GameObject tomato = Instantiate(tomatoPrefab, point.position, Quaternion.identity);
        activeTomatoes[point] = tomato;

        if (tomato != null)
        {
            tomato.GetComponent<TomatoGrowth>().growthTime = 2f;
            tomato.GetComponent<TomatoGrowth>().fieldManager = this;

            float currentTime = 0f;
            Vector3 initialScale = Vector3.zero;
            Vector3 finalScale = Vector3.one;

            while (currentTime < 2f)
            {
                if (tomato != null)
                {
                    tomato.transform.localScale = Vector3.Lerp(initialScale, finalScale, currentTime / 20f);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    yield break;
                }
            }

            if (tomato != null)
            {
                tomato.transform.localScale = finalScale;
            }
        }
    }
}
