using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionBrewInteractionManager : MonoBehaviour
{
    public PotionBrewerTrigger potionBrewer;
    public BasketManager basketManager;
    public VegetableDropAnimator dropAnimator;
    public float delayBetweenDrops = 1f;
    public PlayerAnimation playerAnimation; // «м≥нено на PlayerAnimation

    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessing)
        {
            StartCoroutine(ProcessBrewing());
        }
    }

    private IEnumerator ProcessBrewing()
    {
        isProcessing = true;

        while (potionBrewer.CanBrewPotion() && basketManager.GetBasketContents().Count >= potionBrewer.tomatoesRequired)
        {
            yield return StartCoroutine(DropVegetablesToBrewer());
            yield return StartCoroutine(potionBrewer.BrewPotion());
        }

        isProcessing = false;
        CheckCarryAnimation();
    }

    private IEnumerator DropVegetablesToBrewer()
    {
        List<GameObject> basketContents = basketManager.GetBasketContents();
        List<GameObject> tomatoesToDrop = new List<GameObject>();

        // «м≥нюЇмо лог≥ку, щоб брати овоч≥ з к≥нц€ списку
        for (int i = basketContents.Count - 1; i >= 0 && tomatoesToDrop.Count < potionBrewer.tomatoesRequired; i--)
        {
            if (basketContents[i].CompareTag("Tomato"))
            {
                tomatoesToDrop.Add(basketContents[i]);
            }
        }

        // ѕеревертаЇмо список, щоб ан≥мац≥€ в≥дбувалас€ у правильному пор€дку
        tomatoesToDrop.Reverse();

        foreach (GameObject tomato in tomatoesToDrop)
        {
            Vector3 startPosition = tomato.transform.position;
            dropAnimator.DropVegetableToBrewer(tomato, startPosition, potionBrewer.transform);
            basketManager.RemoveItem(tomato);
            yield return new WaitForSeconds(delayBetweenDrops);
        }

        CheckCarryAnimation();
    }

    private void CheckCarryAnimation()
    {
        if (basketManager.GetBasketContents().Count == 0 && playerAnimation != null)
        {
            playerAnimation.StopCarryAnimation();
        }
        else if (basketManager.GetBasketContents().Count > 0 && playerAnimation != null)
        {
            playerAnimation.StartCarryAnimation();
        }
    }
}