using UnityEngine;

public class ContentTrashBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHarvest playerHarvest = other.GetComponent<PlayerHarvest>();
            BasketManager basketManager = other.GetComponent<BasketManager>();
            VegetableAnimator vegetableAnimator = other.GetComponent<VegetableAnimator>();
            Animator playerAnimator = other.GetComponent<Animator>();

            if (playerHarvest != null && basketManager != null && vegetableAnimator != null && playerAnimator != null)
            {
                if (basketManager.HasBasket() && !basketManager.IsBasketEmpty())
                {
                    vegetableAnimator.ClearBasket();
                    playerAnimator.SetBool("isCarry", false);
                    Debug.Log("Basket content cleared and carrying animation stopped!");
                }
                else
                {
                    Debug.Log("No basket or basket is empty!");
                }
            }
        }
    }
}