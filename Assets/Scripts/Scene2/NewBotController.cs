using UnityEngine;
using System.Collections;

public class NewBotController : MonoBehaviour
{
    private NewQueueManager queueManager;
    private BotMovement botMovement;
    private BotAnimation botAnimation;
    public bool isActive = true;

    public GameObject botHealingEffectPrefab;
    public Transform botHealingEffectPoint;
    private GameObject currentBotHealingEffect;

    void Start()
    {
        botMovement = GetComponent<BotMovement>();
        botAnimation = GetComponent<BotAnimation>();
        queueManager = FindObjectOfType<NewQueueManager>();
        if (queueManager == null)
        {
            Debug.LogError("QueueManager not found in the scene!");
            return;
        }
        MoveToQueuePosition();
    }

    void Update()
    {
        botAnimation.UpdateAnimation(botMovement.MovementMagnitude);
    }

    public void MoveToQueuePosition()
    {
        if (queueManager == null) return;

        int queueIndex = queueManager.GetNextFreePosition();
        if (queueIndex != -1)
        {
            queueManager.OccupyPosition(queueIndex, gameObject);
            Transform targetPosition = queueManager.GetPositionTransform(queueIndex);
            if (targetPosition != null)
            {
                botMovement.MoveToPosition(targetPosition.position);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public IEnumerator StartBotHealingEffect()
    {
        if (botHealingEffectPrefab != null && botHealingEffectPoint != null)
        {
            currentBotHealingEffect = Instantiate(botHealingEffectPrefab, botHealingEffectPoint.position, botHealingEffectPoint.rotation, transform);
        }
        else
        {
            Debug.LogWarning("Bot healing effect prefab or point is not set!");
        }
        botAnimation.StartHealingAnimation();
        yield return new WaitUntil(() => botMovement.HasReachedDestination());
        StopBotHealingEffect();
    }

    private void StopBotHealingEffect()
    {
        botAnimation.StopHealingAnimation();
        if (currentBotHealingEffect != null)
        {
            Destroy(currentBotHealingEffect);
        }
    }

    public void SetQueuePosition(int queueIndex)
    {
        if (queueManager != null && queueIndex >= 0 && queueIndex < queueManager.queuePositions.Length)
        {
            Transform targetPosition = queueManager.GetPositionTransform(queueIndex);
            botMovement.MoveToPosition(targetPosition.position);
        }
    }

    public void ActivateBotMovement()
    {
        isActive = false;
        Debug.Log("Bot movement activated");
        botMovement.ResumeMovement();
    }
}