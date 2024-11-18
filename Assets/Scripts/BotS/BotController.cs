using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour
{
    private QueueManager queueManager;
    private BotMovement botMovement;
    private BotAnimation botAnimation;
    public bool isActive = true;
    public bool isWaitingAtBed = false;
    private BedInteractionManager bedInteractionManager;
    private Transform bedPosition;
    private Transform finalPosition;

    public GameObject botHealingEffectPrefab;
    public Transform botHealingEffectPoint;
    private GameObject currentBotHealingEffect;

    void Start()
    {
        botMovement = GetComponent<BotMovement>();
        botAnimation = GetComponent<BotAnimation>();
        queueManager = FindObjectOfType<QueueManager>();
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

        if (!isActive && isWaitingAtBed)
        {
            Debug.Log("Bot should move to final position now!");
            ContinueToFinalPosition();
        }
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

    public void MoveToBed(Transform bedPosition, Transform finalPosition)
    {
        this.bedPosition = bedPosition;
        this.finalPosition = finalPosition;
        StartCoroutine(MoveToBedAndWait());
    }

    private IEnumerator MoveToBedAndWait()
    {
        Debug.Log("Moving to bed");
        botMovement.MoveToPosition(bedPosition.position);
        yield return new WaitUntil(() => botMovement.HasReachedDestination());

        Debug.Log("Reached bed, waiting for player");
        isWaitingAtBed = true;
        bedInteractionManager = bedPosition.GetComponent<BedInteractionManager>();
        if (bedInteractionManager != null)
        {
            bedInteractionManager.SetCurrentBot(this);
        }
        else
        {
            Debug.LogError("BedInteractionManager not found on bed object!");
        }

        yield return new WaitUntil(() => !isActive);

        Debug.Log("Player activated trigger, continuing to final position");
        ContinueToFinalPosition();
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

    private void ContinueToFinalPosition()
    {
        isWaitingAtBed = false;
        if (finalPosition != null)
        {
            StartCoroutine(MoveToFinalPosition());
        }
        else
        {
            Debug.LogError("Final position is null!");
        }
    }

    private IEnumerator MoveToFinalPosition()
    {
        Debug.Log("Moving to final position");
        botMovement.MoveToPosition(finalPosition.position);
        yield return StartCoroutine(StartBotHealingEffect());

        Debug.Log("Reached final position, deactivating");
        gameObject.SetActive(false);
        int bedIndex = System.Array.IndexOf(queueManager.bedPositions, bedPosition);
        if (bedIndex != -1)
        {
            queueManager.FreeBed(bedIndex);
        }
        queueManager.UpdateQueuePositions();
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
        isWaitingAtBed = false;
        Debug.Log("Bot movement activated");
        botMovement.ResumeMovement();
    }
}