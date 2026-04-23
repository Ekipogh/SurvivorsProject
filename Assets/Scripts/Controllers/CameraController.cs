using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Camera MainCamera;
    public Transform PlayerTransform;

    void FixedUpdate()
    {
        if (PlayerTransform != null)
        {
            Vector3 desiredPosition = PlayerTransform.position;
            desiredPosition.z = MainCamera.transform.position.z; // Keep the camera's z position unchanged
            Vector3 smoothedPosition = Vector3.Lerp(MainCamera.transform.position, desiredPosition, Time.deltaTime * 5f);
            MainCamera.transform.position = smoothedPosition;
        }
    }
}
