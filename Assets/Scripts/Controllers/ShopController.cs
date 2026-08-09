using UnityEngine;
using UnityEngine.UIElements;

public enum ShopItemType
{
    Weapon,
    ShipBlock,
    Upgrade
}

public class ShopController : MonoBehaviour
{
    [SerializeField] private UIDocument shopUI;
    private Button _optionA;
    private Button _optionB;
    private Button _optionC;
    private VisualElement _boundRoot;

    [SerializeField] private BuildManager buildManager;

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

        if (_optionA == null || _optionB == null || _optionC == null)
        {
            Debug.LogWarning("ShopController could not find one or more shop option buttons.");
            return;
        }

        _optionA.clicked += OnOptionAClicked;
        _optionB.clicked += OnOptionBClicked;
        _optionC.clicked += OnOptionCClicked;
        _boundRoot = root;
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
        Debug.Log("Shop button clicked: " + buttonIndex);
        buildManager.PurchaseShopItem(ShopItemType.ShipBlock);
    }
}
