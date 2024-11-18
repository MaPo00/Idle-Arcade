using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionBrewerTrigger : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform[] potionSpawnPoints;
    public BasketManager basketManager;
    public int tomatoesRequired = 2;
    public float brewingTime = 2f;

    private bool isBrewingInProgress = false;
    private List<GameObject> brewedPotions = new List<GameObject>();

    public bool CanBrewPotion()
    {
        return !isBrewingInProgress && brewedPotions.Count < potionSpawnPoints.Length;
    }

    public IEnumerator BrewPotion()
    {
        if (!CanBrewPotion()) yield break;

        isBrewingInProgress = true;

        // Анімація варіння (можна додати візуальні ефекти)
        yield return new WaitForSeconds(brewingTime);

        if (potionSpawnPoints.Length > brewedPotions.Count)
        {
            GameObject newPotion = Instantiate(potionPrefab, potionSpawnPoints[brewedPotions.Count].position, Quaternion.identity);
            brewedPotions.Add(newPotion);
        }

        isBrewingInProgress = false;
    }

    public GameObject GetBrewedPotion()
    {
        if (brewedPotions.Count > 0)
        {
            GameObject potion = brewedPotions[0];
            brewedPotions.RemoveAt(0);
            return potion;
        }
        return null;
    }
}