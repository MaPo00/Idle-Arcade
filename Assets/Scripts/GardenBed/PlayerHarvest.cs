using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    public GameObject cabbagePrefab;
    public GameObject tomatoPrefab;
    private BasketManager basketManager;
    private VegetableAnimator vegetableAnimator;
    private Animator playerAnimator;

    private void Start()
    {
        basketManager = GetComponent<BasketManager>();
        vegetableAnimator = GetComponent<VegetableAnimator>();
        playerAnimator = GetComponent<Animator>();
        if (basketManager == null || vegetableAnimator == null)
        {
            Debug.LogError("BasketManager або VegetableAnimator компонент відсутній на гравці!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (basketManager == null || vegetableAnimator == null || basketManager.IsBasketFull())
        {
            return;
        }

        GameObject vegetablePrefab = null;
        Transform fieldManagerTransform = null;

        if (other.CompareTag("Cabbage"))
        {
            CabbageGrowth cabbage = other.GetComponent<CabbageGrowth>();
            if (cabbage != null && cabbage.IsFullyGrown())
            {
                vegetablePrefab = cabbagePrefab;
                fieldManagerTransform = cabbage.fieldManager?.transform;
            }
        }
        else if (other.CompareTag("Tomato"))
        {
            TomatoGrowth tomato = other.GetComponent<TomatoGrowth>();
            if (tomato != null && tomato.IsFullyGrown())
            {
                vegetablePrefab = tomatoPrefab;
                fieldManagerTransform = tomato.fieldManager?.transform;
            }
        }

        if (vegetablePrefab != null)
        {
            CollectVegetable(vegetablePrefab, other, fieldManagerTransform);
        }
    }

    private void CollectVegetable(GameObject vegetablePrefab, Collider vegetableCollider, Transform fieldManagerTransform)
    {
        GameObject collectedVegetable = vegetableAnimator.CollectVegetable(vegetablePrefab, vegetableCollider.transform.position);
        if (collectedVegetable != null)
        {
            playerAnimator.SetBool("isCarry", true);

        
            if (vegetableCollider.CompareTag("Tomato"))
            {
                collectedVegetable.transform.localScale *= 1.5f; 
            }

            if (fieldManagerTransform != null)
            {
                if (vegetableCollider.CompareTag("Cabbage"))
                {
                    fieldManagerTransform.GetComponent<CabbageFieldManager>()?.HarvestCabbage(vegetableCollider.transform);
                }
                else if (vegetableCollider.CompareTag("Tomato"))
                {
                    fieldManagerTransform.GetComponent<TomatoFieldManager>()?.HarvestTomato(vegetableCollider.transform);
                }
            }
            Destroy(vegetableCollider.gameObject);
        }
    }

    public bool IsBasketFull()
    {
        return basketManager.IsBasketFull();
    }
}