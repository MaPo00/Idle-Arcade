using UnityEngine;

namespace CameraView
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private GameObject followObject;
        [SerializeField] private Vector3 offset;
        [SerializeField][Range(0, 2)] private float smooth;

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                followObject.transform.position + offset,
                Mathf.Lerp(2f, 0.1f, smooth)
            );
        }
    }
}