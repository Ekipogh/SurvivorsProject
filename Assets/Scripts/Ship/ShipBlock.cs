using UnityEngine;

public class ShipBlock : MonoBehaviour
{
    [SerializeField] private float hpBonus = 25f;
    [SerializeField] private Transform weaponMountPoint;

    public Vector2Int GridPosition { get; private set; }
    public float HPBonus => hpBonus;
    public Transform WeaponMountPoint => weaponMountPoint;
    public Weapon MountedWeapon { get; private set; }

    public void Initialize(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
        name = $"ShipBlock_{gridPosition.x}_{gridPosition.y}";
    }

    public bool HasWeaponMounted => MountedWeapon != null;

    public bool TryMountWeapon(Weapon weapon)
    {
        if (weapon == null || HasWeaponMounted)
            return false;

        MountedWeapon = weapon;
        weapon.transform.SetParent(weaponMountPoint, false);
        weapon.transform.localPosition = Vector3.zero;
        return true;
    }
}