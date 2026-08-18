using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private ShopController shopController;
    [SerializeField] private Transform gameUI;
    [SerializeField] private ShipBuilder shipBuilder;
    [SerializeField] private Player player;
    [SerializeField] private GameStageManager gameStageManager;
    public void EnableBuildingMode(bool enable)
    {
        shopController.MakeShopUIVisible(enable);
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

    public void PurchaseShopItem(ShopItemOption itemOption)
    {
        // Disable shop UI
        shopController.MakeShopUIVisible(false);
        switch (itemOption.ItemType)
        {
            case ShopItemType.Weapon:
                // Implement weapon purchase logic
                Debug.Log("Purchasing Weapon");
                break;
            case ShopItemType.ShipBlock:
                // Implement ship block purchase logic
                Debug.Log("Purchasing Ship Block");
                shipBuilder.BeginPlacingBlock(itemOption.Cost);
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

    public void CancelCurrentPurchase()
    {
        shipBuilder.CancelCurrentPlacement();
        shopController.MakeShopUIVisible(true);
    }
}
