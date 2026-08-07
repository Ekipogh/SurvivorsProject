using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Transform shopUI;
    [SerializeField] private Transform gameUI;
    public void EnableBuildingMode(bool enable)
    {
        // Implement logic to enable or disable building mode
        if (enable)
        {
            Debug.Log("Building mode enabled.");
            if (shopUI != null)
            {
                shopUI.gameObject.SetActive(true); // Show the building mode UI
                gameUI?.gameObject.SetActive(false); // Hide the game UI
            }
            else
            {
                Debug.LogWarning("Build mode UI reference is not set.");
            }
        }
        else
        {
            Debug.Log("Building mode disabled.");
            if (shopUI != null)
            {
                shopUI.gameObject.SetActive(false); // Hide the building mode UI
                gameUI?.gameObject.SetActive(true); // Show the game UI
            }
            else
            {
                Debug.LogWarning("Build mode UI reference is not set.");
            }
        }
    }
}
