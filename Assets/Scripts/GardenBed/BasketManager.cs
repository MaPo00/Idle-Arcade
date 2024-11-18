using UnityEngine;
using System.Collections.Generic;

public class BasketManager : MonoBehaviour
{
    public Transform basketSpawnPoint;
    public int maxItems = 100;
    public int columns = 2;
    private List<GameObject> carriedItems = new List<GameObject>();
    private VegetableStackPhysics stackPhysics;

    private void Start()
    {
        stackPhysics = GetComponent<VegetableStackPhysics>();
        UpdateStackPhysics();
    }
    public bool HasBasket()
    {
        return true;
    }

    public bool CanAddItem()
    {
        return carriedItems.Count < maxItems;
    }
    public void UpdateStackPhysics()
    {
        if (stackPhysics != null)
        {
            stackPhysics.UpdateVegetableList();
        }
    }
    public void AddItem(GameObject item)
    {
        if (CanAddItem())
        {
            carriedItems.Add(item);

       
            if (item.CompareTag("Tomato"))
            {
                item.transform.localScale *= 1.5f;
            }

            UpdateStackPhysics();
        }
    }
    public bool IsBasketFull()
    {
        return carriedItems.Count >= maxItems;
    }

    public bool IsBasketEmpty()
    {
        return carriedItems.Count == 0;
    }

    public List<GameObject> GetBasketContents()
    {
        return carriedItems;
    }

    public void RemoveItemFromEnd(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (carriedItems.Count > 0)
            {
                int lastIndex = carriedItems.Count - 1;
                Destroy(carriedItems[lastIndex]);
                carriedItems.RemoveAt(lastIndex);
            }
        }
        UpdateStackPhysics();
    }

    public List<GameObject> GetCarriedItems()
    {
        return carriedItems;
    }

    public void ClearItems()
    {
        carriedItems.Clear();
    }
    public void RemoveItem(GameObject item)
    {
        carriedItems.Remove(item);
        UpdateStackPhysics();
    }
    public Vector3 GetItemPosition(int index)
    {
        int column = index % columns;
        int row = index / columns;

        float xOffset = (column - 0.5f) * 0.3f; 
        float yOffset = row * 0.2f;
        float zOffset = 0f; 

        return new Vector3(xOffset, yOffset, zOffset);
    }
}