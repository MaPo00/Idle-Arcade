using UnityEngine;

public class BotModelManager : MonoBehaviour
{
    public GameObject initialBotModel;
    public GameObject treatedBotModel;
    private GameObject currentModel;

    void Start()
    {
        if (initialBotModel != null)
        {
            currentModel = Instantiate(initialBotModel, transform);
        }
        else
        {
            Debug.LogError("Initial bot model is not set!");
        }
    }

    public void SwitchToTreatedModel()
    {
        if (treatedBotModel != null)
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
            }
            currentModel = Instantiate(treatedBotModel, transform);
        }
        else
        {
            Debug.LogError("Treated bot model is not set!");
        }
    }

    public Animator GetCurrentAnimator()
    {
        return currentModel?.GetComponent<Animator>();
    }
}