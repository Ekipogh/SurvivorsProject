using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTransform;

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position;
            desiredPosition.z = mainCamera.transform.position.z; // Keep the camera's z position unchanged
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 5f);
            mainCamera.transform.position = smoothedPosition;
        }
    }
}
