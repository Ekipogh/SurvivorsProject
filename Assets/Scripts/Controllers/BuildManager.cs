using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Transform shopUI;
    [SerializeField] private Transform gameUI;
    [SerializeField] private ShipBuilder shipBuilder;
    [SerializeField] private Player player;
    [SerializeField] private GameStageManager gameStageManager;
    public void EnableBuildingMode(bool enable)
    {
        shopUI.gameObject.SetActive(enable);
        gameUI.gameObject.SetActive(!enable);
        player.SetControlEnabled(!enable);
        if (enable)
        {
            shipBuilder.BeginBuildMode();
        }
        else
        {
            shipBuilder.EndBuildMode();
        }
    }

    public void PurchaseShopItem(ShopItemType itemType)
    {
        // Disable shop UI
        shopUI.gameObject.SetActive(false);
        switch (itemType)
        {
            case ShopItemType.Weapon:
                // Implement weapon purchase logic
                Debug.Log("Purchasing Weapon");
                break;
            case ShopItemType.ShipBlock:
                // Implement ship block purchase logic
                Debug.Log("Purchasing Ship Block");
                shipBuilder.BeginPlacingBlock();
                break;
            case ShopItemType.Upgrade:
                // Implement upgrade purchase logic
                Debug.Log("Purchasing Upgrade");
                break;
            default:
                Debug.LogWarning("Unknown shop item type.");
                break;
        }
    }

    public void OnBlockPlaced()
    {
        gameStageManager.SetGameStage(GameStage.Battle);
    }
}
