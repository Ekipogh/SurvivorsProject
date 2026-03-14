using UnityEngine;

public abstract class ProjectileBehaviour : ScriptableObject
{
    public abstract void Shoot(Weapon weapon);
}