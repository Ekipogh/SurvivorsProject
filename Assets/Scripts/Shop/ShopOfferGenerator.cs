using System.Collections.Generic;
using UnityEngine;

public class ShopOfferGenerator
{
    Dictionary<int, int> weaponLevelByGameLevel = new()
    {
        { 0, 1 },
        { 1, 1 },
        { 2, 2 },
        { 3, 2 },
        { 4, 3 },
        { 5, 3 },
        { 6, 4 },
        { 7, 4 },
        { 8, 5 },
        { 9, 5 },
        { 10, 5 }
    };
    public ShopItemOption[] GenerateShopOffers(int level, int seed)
    {
        ShopItemOption[] offers = new ShopItemOption[3];
        var shipBlockOffer = new ShopItemOption
        {
            ItemType = ShopItemType.ShipBlock,
            Cost = 100 + level * 10,
            DisplayName = "Ship Block",
        };
        offers[0] = shipBlockOffer;
        // generate two more random offers at index 1 and 2
        // for now they going to be weapons of different types

        for (int i = 1; i < 3; i++)
        {
            var weaponType = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);
            // weapon level from the map or max level if bigger than max level
            var minLevel = weaponLevelByGameLevel[0];
            var maxLevel = weaponLevelByGameLevel[weaponLevelByGameLevel.Count - 1];
            var weaponLevel = Mathf.Clamp(weaponLevelByGameLevel[level], minLevel, maxLevel);
            var weaponOffer = new ShopItemOption
            {
                ItemType = ShopItemType.Weapon,
                WeaponType = weaponType,
                Cost = 150 + level * 15,
                DisplayName = weaponType.ToString(),
                WeaponLevel = weaponLevel
            };
            offers[i] = weaponOffer;
        }
        return offers;
    }
}
