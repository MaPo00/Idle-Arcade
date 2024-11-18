using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Action Zone", menuName = "Data/ActionZone")]
    public class ActionZone : ScriptableObject
    {
        [Range(0, 2)] public float duration = 1;
        [Range(0, 2)] public float fillDelay = 0.5f;
        [Range(0, 2)] public float outFillDelay = 0.5f;
    }
}