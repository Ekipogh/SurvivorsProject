using UnityEngine;

[System.Serializable]
public class BaseBlockInfo
{
    public string DisplayName;
    public int BaseCost;
}

[System.Serializable]
public class WeaponEntry
{
    public int CatalogId;
    public string DisplayName;
    public WeaponType WeaponType;
    public int WeaponLevel;
    public int BaseCost;
    public int MinimumTier;
}

[System.Serializable]
public class UpgradeEntry
{
    public int CatalogId;
    public string DisplayName;
    public int Cost;
}

[System.Serializable]
public class ProgressionTier
{
    public int MinimumGameLevel;
    public int MaximumWeaponLevel;
    public int CostMultiplier;
    public float UpgradeProbability;
}

[CreateAssetMenu(fileName = "ShopCatalog", menuName = "ScriptableObjects/ShopCatalog", order = 1)]
public class ShopCatalog : ScriptableObject
{
    public BaseBlockInfo baseBlockInfo;
    public WeaponEntry[] weaponEntries;
    public UpgradeEntry[] upgradeEntries;
    public ProgressionTier[] progressionTiers;

    public int Seed = 0;
}