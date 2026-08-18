using UnityEngine;
using UnityEngine.UIElements;

public enum ShopItemType
{
    Weapon,
    ShipBlock,
    Upgrade
}

[System.Serializable]
public class ShopItemOption
{
    public ShopItemType ItemType;
    public string DisplayName;
    public float Cost;
}

public class ShopController : MonoBehaviour
{
    [SerializeField] private UIDocument shopUI;
    private Button _optionA;
    private Button _optionB;
    private Button _optionC;
    private Button[] _shopButtons = System.Array.Empty<Button>();
    private VisualElement _boundRoot;

    [SerializeField] private BuildManager buildManager;
    [SerializeField] private Player player;

    [SerializeField] private ShopItemOption[] shopItemOptions;

    void Awake()
    {
        if (shopUI == null)
        {
            Debug.LogError("Shop UI Document is not assigned.");
            return;
        }
        if (buildManager == null)
        {
            Debug.LogError("BuildManager is not assigned.");
            return;
        }
    }

    void Update()
    {
        if (shopUI == null)
        {
            return;
        }

        if (!shopUI.gameObject.activeInHierarchy)
        {
            UnbindButtons();
            return;
        }

        BindButtons();
        UpdateShopButtonAffordability();
    }

    void OnEnable()
    {
        BindButtons();
    }

    void OnDisable()
    {
        UnbindButtons();
    }

    private void BindButtons()
    {
        if (shopUI == null || !shopUI.gameObject.activeInHierarchy)
        {
            return;
        }

        VisualElement root = shopUI.rootVisualElement;
        if (root == null || root == _boundRoot)
        {
            return;
        }

        UnbindButtons();

        _optionA = root.Q<Button>("ShopOptionA");
        _optionB = root.Q<Button>("ShopOptionB");
        _optionC = root.Q<Button>("ShopOptionC");
        _shopButtons = new Button[] { _optionA, _optionB, _optionC };

        if (_optionA == null || _optionB == null || _optionC == null)
        {
            Debug.LogWarning("ShopController could not find one or more shop option buttons.");
            return;
        }

        _optionA.clicked += OnOptionAClicked;
        _optionB.clicked += OnOptionBClicked;
        _optionC.clicked += OnOptionCClicked;
        _boundRoot = root;
        UpdateShopButtonAffordability();
        Debug.Log("ShopController bound shop option click events.");
    }

    private void UnbindButtons()
    {
        if (_optionA != null) _optionA.clicked -= OnOptionAClicked;
        if (_optionB != null) _optionB.clicked -= OnOptionBClicked;
        if (_optionC != null) _optionC.clicked -= OnOptionCClicked;

        _optionA = null;
        _optionB = null;
        _optionC = null;
        _shopButtons = System.Array.Empty<Button>();
        _boundRoot = null;
    }

    void OnOptionAClicked()
    {
        OnShopButtonClicked(0);
    }

    void OnOptionBClicked()
    {
        OnShopButtonClicked(1);
    }

    void OnOptionCClicked()
    {
        OnShopButtonClicked(2);
    }

    void OnShopButtonClicked(int buttonIndex)
    {
        float baseBlockCost = shopItemOptions[buttonIndex].Cost;
        if (!player.CanAfford(baseBlockCost))
        {
            Debug.Log("Not enough points to purchase the item.");
            return;
        }
        buildManager.PurchaseShopItem(shopItemOptions[buttonIndex]);
        UpdateShopButtonAffordability();
    }

    public void MakeShopUIVisible(bool visible)
    {
        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(visible);
        }
        if (!visible)
        {
            return;
        }

        BindButtons();
        UpdateShopButtonAffordability();
    }

    private void UpdateShopButtonAffordability()
    {
        if (player == null || shopItemOptions == null || _shopButtons == null)
        {
            return;
        }

        for (int i = 0; i < shopItemOptions.Length; i++)
        {
            float itemCost = shopItemOptions[i].Cost;
            bool canAfford = player.CanAfford(itemCost);
            if (i < _shopButtons.Length && _shopButtons[i] != null)
            {
                _shopButtons[i].SetEnabled(canAfford);
            }
        }
    }
}
