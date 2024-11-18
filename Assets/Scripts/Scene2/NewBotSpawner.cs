using UnityEngine;
using System.Collections;

public class NewBotSpawner : MonoBehaviour
{
    public GameObject botPrefab;
    public float initialSpawnInterval = 5.0f;
    public float minSpawnInterval = 2.0f;
    public float spawnIntervalDecreaseRate = 0.1f;
    private float currentSpawnInterval;
    private NewQueueManager queueManager;
    private Coroutine spawnCoroutine;
    private int spawnedBots = 0;

    void Start()
    {
        queueManager = FindObjectOfType<NewQueueManager>();
        if (queueManager == null)
        {
            Debug.LogError("QueueManager not found in the scene!");
            return;
        }
        currentSpawnInterval = initialSpawnInterval;
        spawnCoroutine = StartCoroutine(SpawnBot());
    }

    IEnumerator SpawnBot()
    {
        while (true)
        {
            if (queueManager == null)
            {
                Debug.LogError("QueueManager is null.");
                yield break;
            }

            int freePosition = queueManager.GetNextFreePosition();
            if (freePosition != -1)
            {
                GameObject newBot = Instantiate(botPrefab, transform.position, transform.rotation);
                BotController botController = newBot.GetComponent<BotController>();
                if (botController != null)
                {
                    botController.MoveToQueuePosition();
                    spawnedBots++;

                    // «меншуЇмо ≥нтервал спавну п≥сл€ кожного п'€того бота
                    if (spawnedBots % 5 == 0 && currentSpawnInterval > minSpawnInterval)
                    {
                        currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseRate, minSpawnInterval);
                    }
                }
                yield return new WaitForSeconds(currentSpawnInterval);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void ResumeSpawning()
    {
        if (spawnCoroutine == null && queueManager != null && !queueManager.IsQueueFull())
        {
            spawnCoroutine = StartCoroutine(SpawnBot());
        }
    }
}