using UnityEngine;

public abstract class WeaponBehaviour : ScriptableObject
{
    public abstract void Shoot(Weapon weapon);
}