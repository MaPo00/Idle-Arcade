using DG.Tweening;
using UnityEngine;
using Actions;
using Environment.MoneyStack;
using System.Collections;
public class TriggerZone : ActionZone
{

    public Transform finalPosition;
    public MoneyStack moneyStack;
    private QueueManager queueManager;
    private Coroutine serviceCoroutine;
    [SerializeField] private float moneySpawnHeight = 1f;
    private void Awake()
    {
        queueManager = FindObjectOfType<QueueManager>();
        if (spriteToFill == null)
        {
            Debug.LogError("SpriteRenderer is not set on TriggerZone!");
        }
        else
        {
            FillValue = 0;
        }
    }
    protected override void OnAction()
    {
        if (serviceCoroutine == null)
        {
            serviceCoroutine = StartCoroutine(ServiceBots());
        }
    }
    private IEnumerator ServiceBots()
    {
        while (true)
        {
            GameObject firstBot = queueManager.GetFirstBotInQueue();
            if (firstBot != null)
            {
                BotController botController = firstBot.GetComponent<BotController>();
                if (botController != null)
                {
                    // Спавн грошей з бота
                    if (moneyStack != null)
                    {
                        Vector3 moneySpawnPosition = firstBot.transform.position + Vector3.up * moneySpawnHeight;
                        SpawnMoneyFromBot(moneySpawnPosition);
                    }
                    int bedIndex = queueManager.GetNextFreeBed();
                    if (bedIndex != -1)
                    {
                        Transform bedPosition = queueManager.GetBedTransform(bedIndex);
                        queueManager.OccupyBed(bedIndex);
                        botController.MoveToBed(bedPosition, finalPosition);
                    }
                    else
                    {
                        Debug.LogWarning("No free beds available!");
                    }
                }
                else
                {
                    Debug.LogWarning("BotController not found on the bot!");
                }
                queueManager.UpdateQueuePositions();
            }
            yield return new WaitForSeconds(2.0f);
            ResetSprite();
            StartFill();
            yield return new WaitForSeconds(config.duration + config.fillDelay);
        }
    }
    private void SpawnMoneyFromBot(Vector3 spawnPosition)
    {
        StartCoroutine(SpawnMoneyWithDelay(spawnPosition));
    }
    private IEnumerator SpawnMoneyWithDelay(Vector3 spawnPosition)
    {
        int moneyCount = 5; // Кількість монет, які вилітатимуть з бота
        for (int i = 0; i < moneyCount; i++)
        {
            moneyStack.FillStack(1, spawnPosition);
            yield return new WaitForSeconds(0.1f); // Невелика затримка між спавном кожної монети
        }
    }
    private void ResetSprite()
    {
        FillValue = 0;
    }
    protected override bool OnFillEnabled()
    {
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OnFillEnabled())
        {
            StartFill();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (serviceCoroutine != null)
            {
                StopCoroutine(serviceCoroutine);
                serviceCoroutine = null;
                ResetSprite();
            }
            StartOutFill();
        }
    }
}