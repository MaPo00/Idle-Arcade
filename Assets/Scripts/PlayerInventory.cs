using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int potionCount = 0;

    public void AddPotion()
    {
        potionCount++;
    }

    public bool HasPotion()
    {
        return potionCount > 0;
    }
}
