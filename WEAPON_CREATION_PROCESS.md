# Weapon Creation Process

This document describes the current weapon pipeline used in SurvivorsProject and maps it to the in-progress rocket launcher implementation.

## Core runtime flow

1. Player owns a list of Weapon components.
2. Each Weapon chooses a target, aims, and when cooldown is ready calls projectile behaviour Shoot(Weapon).
3. Projectile behaviour decides how the attack is executed (spawn projectile, raycast, laser, and so on).
4. WeaponStats provides tunable values for attack behavior.

## Step-by-step process

### 1) Create projectile prefab and projectile MonoBehaviour (projectile weapons only)

- Create a projectile prefab under Assets/Prefabs/Projectiles.
- Add required components:
  - SpriteRenderer
  - Collider2D with Is Trigger enabled
  - Rigidbody2D (2D physics) with Gravity Scale = 0
  - Projectile script
- Put hit and impact logic in the projectile script.
- Reminder: for projectile weapons, collider should be trigger and Rigidbody2D gravity should stay at 0.

Rocket launcher example:

- Projectile prefab: Assets/Prefabs/Projectiles/RocketProjectile.prefab
- Projectile script: Assets/Scripts/Projectiles/SimpleRocket.cs (class SimpleHommingRocket)
- Explosion effect prefab: Assets/Prefabs/Effects/ExplosionEffect.prefab
- Explosion cleanup script: Assets/Scripts/Effects/ExplosionEffect.cs

### 2) Create projectile behavior ScriptableObject

- Inherit from ProjectileBehaviour and implement Shoot(Weapon weapon).
- In Shoot(Weapon):
  - Instantiate the projectile prefab.
  - Set target from weapon.TargetEnemy when needed.
  - Copy runtime stats from weapon.stats into projectile fields.
- Create the ScriptableObject asset and assign projectile prefab references.

Rocket launcher example:

- Behavior script: Assets/Scripts/SO/SimpleRocketBehavior.cs
- Behavior asset: Assets/ScriptableObjects/Projectiles/SimpleRocketBehavior.asset
- Key reference: rocketPrefab points to Assets/Prefabs/Projectiles/RocketProjectile.prefab

### 3) Create WeaponStats ScriptableObject

- Create a WeaponStats asset and set parameters:
  - damageModifier
  - rotationSpeed
  - range
  - attackAngle
  - cooldownTime
  - projectileSpeed
  - aoeRadius (for explosive weapons)

Rocket launcher example:

- Stats asset: Assets/ScriptableObjects/SimpleRocketStats.asset
- Current values:
  - damageModifier: 50
  - rotationSpeed: 50
  - range: 10
  - attackAngle: 45
  - cooldownTime: 1
  - projectileSpeed: 2
  - aoeRadius: 2

### 4) Create weapon prefab and wire references

- Create weapon prefab under Assets/Prefabs/Weapons with:
  - Weapon component
  - Child FiringPoint transform
  - Optional sprite setup
- Assign on Weapon component:
  - stats
  - projectileBehaviour
  - firingPoint
  - player (or assign on scene instance override)

Rocket launcher example:

- Weapon prefab: Assets/Prefabs/Weapons/SimpleRocketGun.prefab
- Assigned in prefab:
  - projectileBehaviour -> SimpleRocketBehavior
  - firingPoint -> FiringPoint child transform
- Assigned in BattleScene instance override:
  - stats -> SimpleRocketStats
  - player -> Player component instance

### 5) Add weapon to player weapons list

- Add the weapon instance to Player.weapons.
- Ensure the array/list includes the new entry and points to the Weapon component.

Rocket launcher example:

- Scene: Assets/Scenes/BattleScene.unity
- Player weapons list size increased from 3 to 4.
- New entry weapons[3] points to the SimpleRocketGun Weapon component.

## Files involved in the rocket launcher implementation

- Scripts
  - Assets/Scripts/Projectiles/SimpleRocket.cs
  - Assets/Scripts/SO/SimpleRocketBehavior.cs
  - Assets/Scripts/Effects/ExplosionEffect.cs
- Prefabs
  - Assets/Prefabs/Projectiles/RocketProjectile.prefab
  - Assets/Prefabs/Effects/ExplosionEffect.prefab
  - Assets/Prefabs/Weapons/SimpleRocketGun.prefab
- ScriptableObjects
  - Assets/ScriptableObjects/Projectiles/SimpleRocketBehavior.asset
  - Assets/ScriptableObjects/SimpleRocketStats.asset
- Scene wiring
  - Assets/Scenes/BattleScene.unity

## Quick checklist for any new weapon

- Decide attack type: projectile, hitscan, or beam.
- If projectile: create projectile prefab and MonoBehaviour script.
- Projectile prefab reminder: Rigidbody2D Gravity Scale = 0 and Collider2D Is Trigger = true.
- Create ProjectileBehaviour script and ScriptableObject asset.
- Create WeaponStats ScriptableObject asset.
- Create or duplicate weapon prefab and wire references.
- Add weapon to Player.weapons.
- Test target acquisition, aiming, cooldown, damage, and cleanup.