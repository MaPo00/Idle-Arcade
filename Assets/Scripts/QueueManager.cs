using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePositions;
    public Transform[] bedPositions;
    private bool[] isOccupied;
    private bool[] isBedOccupied;
    private List<GameObject> botsInQueue = new List<GameObject>();
    private BotSpawner botSpawner;

    void Awake()
    {
        if (queuePositions == null || queuePositions.Length == 0)
        {
            Debug.LogError("queuePositions не ініціалізовано або порожній.");
        }
        else
        {
            isOccupied = new bool[queuePositions.Length];
        }

        if (bedPositions == null || bedPositions.Length == 0)
        {
            Debug.LogError("bedPositions не ініціалізовано або порожній.");
        }
        else
        {
            isBedOccupied = new bool[bedPositions.Length];
        }

        botSpawner = FindObjectOfType<BotSpawner>();
        if (botSpawner == null)
        {
            Debug.LogError("BotSpawner íå çíàéäåíî ó ñöåí³.");
        }
    }

    public int GetNextFreePosition()
    {
        for (int i = 0; i < isOccupied.Length; i++)
        {
            if (!isOccupied[i])
            {
                return i;
            }
        }
        return -1;
    }

    public int GetNextFreeBed()
    {
        for (int i = 0; i < isBedOccupied.Length; i++)
        {
            if (!isBedOccupied[i])
            {
                return i;
            }
        }
        return -1;
    }

    public void OccupyPosition(int index, GameObject bot)
    {
        if (index >= 0 && index < isOccupied.Length)
        {
            isOccupied[index] = true;
            botsInQueue.Add(bot);
        }
    }

    public void OccupyBed(int index)
    {
        if (index >= 0 && index < isBedOccupied.Length)
        {
            isBedOccupied[index] = true;
        }
    }

    public void FreePosition(int index)
    {
        if (index >= 0 && index < isOccupied.Length)
        {
            isOccupied[index] = false;
            botsInQueue.RemoveAll(bot => bot == null);
        }
    }

    public void FreeBed(int index)
    {
        if (index >= 0 && index < isBedOccupied.Length)
        {
            isBedOccupied[index] = false;
        }
    }

    public Transform GetPositionTransform(int index)
    {
        if (index >= 0 && index < queuePositions.Length)
        {
            return queuePositions[index];
        }
        return null;
    }

    public Transform GetBedTransform(int index)
    {
        if (index >= 0 && index < bedPositions.Length)
        {
            return bedPositions[index];
        }
        return null;
    }

    public void UpdateQueuePositions()
    {
        List<GameObject> activeBotsInQueue = new List<GameObject>();

        // Збираємо всіх активних ботів
        foreach (GameObject bot in botsInQueue)
        {
            if (bot != null && bot.activeSelf && bot.GetComponent<BotController>().isActive)
            {
                activeBotsInQueue.Add(bot);
            }
        }

        // Оновлюємо позиції для активних ботів
        for (int i = 0; i < activeBotsInQueue.Count; i++)
        {
            BotController botController = activeBotsInQueue[i].GetComponent<BotController>();
            if (botController != null)
            {
                botController.SetQueuePosition(i);
                isOccupied[i] = true;
            }
        }

        // Очищаємо невикористані позиції
        for (int i = activeBotsInQueue.Count; i < isOccupied.Length; i++)
        {
            isOccupied[i] = false;
        }

        // Оновлюємо список ботів у черзі
        botsInQueue = activeBotsInQueue;

        // Перевіряємо, чи можна відновити спавн ботів
        if (botSpawner != null && !IsQueueFull())
        {
            botSpawner.ResumeSpawning();
        }
    }

    public GameObject GetFirstBotInQueue()
    {
        if (botsInQueue.Count > 0)
        {
            for (int i = 0; i < botsInQueue.Count; i++)
            {
                GameObject bot = botsInQueue[i];
                if (bot != null && bot.activeSelf && bot.GetComponent<BotController>().isActive)
                {
                    botsInQueue.RemoveAt(i);
                    return bot;
                }
            }
        }
        return null;
    }

    public bool IsQueueFull()
    {
        if (isOccupied == null)
        {
            Debug.LogError("Масив isOccupied не ініціалізовано.");
            return true; // Повертаємо true, щоб запобігти подальшому спавну
        }

        foreach (bool occupied in isOccupied)
        {
            if (!occupied)
            {
                return false;
            }
        }
        return true;
    }

    public void ResumeBotSpawning()
    {
        if (botSpawner != null && !IsQueueFull())
        {
            botSpawner.ResumeSpawning();
        }
    }
}