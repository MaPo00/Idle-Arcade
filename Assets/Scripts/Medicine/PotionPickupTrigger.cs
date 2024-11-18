using UnityEngine;
using System.Collections.Generic;

public class PotionPickupTrigger : MonoBehaviour
{
    public PotionBrewerTrigger potionBrewer;
    public Transform[] playerPotionSlots;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupPotions();
        }
    }

    private void PickupPotions()
    {
        if (potionBrewer == null)
        {
            Debug.LogError("PotionBrewerTrigger is not assigned!");
            return;
        }

        int availableSlots = CountAvailableSlots();
        if (availableSlots == 0)
        {
            Debug.Log("Player's potion slots are full. Cannot pick up more potions.");
            return;
        }

        int potionsPickedUp = 0;
        foreach (Transform slot in playerPotionSlots)
        {
            if (slot.childCount == 0)
            {
                GameObject potion = potionBrewer.GetBrewedPotion();
                if (potion != null)
                {
                    potion.transform.SetParent(slot);
                    potion.transform.localPosition = Vector3.zero;
                    potion.transform.localRotation = Quaternion.identity;
                    potionsPickedUp++;

                    if (potionsPickedUp >= availableSlots)
                    {
                        break; // Stop if all available slots are filled
                    }
                }
                else
                {
                    break; // No more potions available
                }
            }
        }

        Debug.Log($"Picked up {potionsPickedUp} potions");
        if (potionsPickedUp == 0)
        {
            Debug.Log("No potions were picked up. Check if potions are available or if player slots are full.");
        }
    }
    public GameObject RemoveFirstPotion()
    {
        foreach (Transform slot in playerPotionSlots)
        {
            if (slot.childCount > 0)
            {
                GameObject potion = slot.GetChild(0).gameObject;
                potion.transform.SetParent(null);
                return potion;
            }
        }
        return null;
    }
    private int CountAvailableSlots()
    {
        int availableSlots = 0;
        foreach (Transform slot in playerPotionSlots)
        {
            if (slot.childCount == 0)
            {
                availableSlots++;
            }
        }
        return availableSlots;
    }
}