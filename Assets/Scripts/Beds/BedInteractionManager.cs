using UnityEngine;
using System.Collections;

public class BedInteractionManager : MonoBehaviour
{
    private BotController currentBot;
    public PotionPickupTrigger playerPotionManager;
    public GameObject healingEffectPrefab; // Префаб ефекту лікування на ліжку
    public Transform healingEffectPoint; // Точка на ліжку для анімації
    private GameObject currentHealingEffect; // Поточний об'єкт анімації
    private const float HEALING_DURATION = 1f; // Тривалість першої анімації

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered bed trigger");
            if (currentBot != null && currentBot.isWaitingAtBed)
            {
                StartCoroutine(HealingProcess());
            }
        }
    }

    private IEnumerator HealingProcess()
    {
        if (CheckAndRemovePotion())
        {
            Debug.Log("Starting healing process");
            PlayHealingAnimation();
            yield return new WaitForSeconds(HEALING_DURATION);
            StopHealingAnimation();
            currentBot.StartBotHealingEffect();
            currentBot.ActivateBotMovement();
            Debug.Log("Healing process complete, bot activated");
        }
        else
        {
            Debug.Log("Player doesn't have a potion. Bot remains waiting.");
        }
    }

    private bool CheckAndRemovePotion()
    {
        if (playerPotionManager != null)
        {
            GameObject potion = playerPotionManager.RemoveFirstPotion();
            if (potion != null)
            {
                Debug.Log("Potion removed successfully");
                Destroy(potion);
                return true;
            }
            else
            {
                Debug.Log("No potion found to remove");
            }
        }
        else
        {
            Debug.LogError("playerPotionManager is not set in BedInteractionManager!");
        }
        return false;
    }

    private void PlayHealingAnimation()
    {
        if (healingEffectPrefab != null && healingEffectPoint != null)
        {
            currentHealingEffect = Instantiate(healingEffectPrefab, healingEffectPoint.position, healingEffectPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Healing effect prefab or point is not set!");
        }
    }

    private void StopHealingAnimation()
    {
        if (currentHealingEffect != null)
        {
            Destroy(currentHealingEffect);
        }
    }

    public void SetCurrentBot(BotController bot)
    {
        currentBot = bot;
    }
}