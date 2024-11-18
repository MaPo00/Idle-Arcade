using UnityEngine;
using System.Collections.Generic;

public class NewQueueManager : MonoBehaviour
{
    public Transform[] queuePositions;
    private bool[] isOccupied;
    private List<GameObject> botsInQueue = new List<GameObject>();
    private NewBotSpawner botSpawner;

    void Awake()
    {
        if (queuePositions == null || queuePositions.Length == 0)
        {
            Debug.LogError("queuePositions �� ������������ ��� �������.");
        }
        else
        {
            isOccupied = new bool[queuePositions.Length];
        }

        botSpawner = FindObjectOfType<NewBotSpawner>();
        if (botSpawner == null)
        {
            Debug.LogError("BotSpawner �� �������� � ����.");
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

    public void OccupyPosition(int index, GameObject bot)
    {
        if (index >= 0 && index < isOccupied.Length)
        {
            isOccupied[index] = true;
            botsInQueue.Add(bot);
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

    public Transform GetPositionTransform(int index)
    {
        if (index >= 0 && index < queuePositions.Length)
        {
            return queuePositions[index];
        }
        return null;
    }

    public void UpdateQueuePositions()
    {
        List<GameObject> activeBotsInQueue = new List<GameObject>();

        // ������� ��� �������� ����
        foreach (GameObject bot in botsInQueue)
        {
            if (bot != null && bot.activeSelf && bot.GetComponent<BotController>().isActive)
            {
                activeBotsInQueue.Add(bot);
            }
        }

        // ��������� ������� ��� �������� ����
        for (int i = 0; i < activeBotsInQueue.Count; i++)
        {
            BotController botController = activeBotsInQueue[i].GetComponent<BotController>();
            if (botController != null)
            {
                botController.SetQueuePosition(i);
                isOccupied[i] = true;
            }
        }

        // ������� ������������ �������
        for (int i = activeBotsInQueue.Count; i < isOccupied.Length; i++)
        {
            isOccupied[i] = false;
        }

        // ��������� ������ ���� � ����
        botsInQueue = activeBotsInQueue;

        // ����������, �� ����� �������� ����� ����
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
            Debug.LogError("����� isOccupied �� ������������.");
            return true; // ��������� true, ��� �������� ���������� ������
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