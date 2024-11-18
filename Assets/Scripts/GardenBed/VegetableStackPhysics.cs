using UnityEngine;
using System.Collections.Generic;

public class VegetableStackPhysics : MonoBehaviour
{
    public float inertiaStrength = 15f;
    public float verticalOffset = 0.17f;

    private List<Transform> vegetableTransforms = new List<Transform>();
    private List<Vector3> targetPositions = new List<Vector3>();

    private void Update()
    {
        if (vegetableTransforms.Count == 0) return;

        UpdateTargetPositions();
        ApplyInertia();
    }

    private void UpdateTargetPositions()
    {
        targetPositions.Clear();
        Vector3 basePosition = transform.position + transform.forward * 0.5f;
        for (int i = 0; i < vegetableTransforms.Count; i++)
        {
            targetPositions.Add(basePosition + Vector3.up * (i * verticalOffset));
        }
    }

    private void ApplyInertia()
    {
        for (int i = 0; i < vegetableTransforms.Count; i++)
        {
            Transform vegetable = vegetableTransforms[i];
            Vector3 targetPosition = targetPositions[i];

            vegetable.position = Vector3.Lerp(vegetable.position, targetPosition, Time.deltaTime * inertiaStrength);
        }
    }

    public void UpdateVegetableList()
    {
        vegetableTransforms.Clear();
        foreach (Transform child in transform)
        {
            vegetableTransforms.Add(child);
        }
    }
}