using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    [FormerlySerializedAs("player")]
    public Player Player;
    [FormerlySerializedAs("enemyPrefab")]
    public Enemy EnemyPrefab;

    [FormerlySerializedAs("mapGrid")]
    public GameObject MapGrid;

    [FormerlySerializedAs("spawnInterval")]
    public int SpawnInterval = 2; // Time interval between spawns in seconds
    [FormerlySerializedAs("maxEnemies")]
    public int MaxEnemies = 5; // Maximum number of enemies allowed in the scene
    private const float SpawnRadius = 5f; // Radius around the player to spawn enemies
    private int _enemyId = 0; // Unique ID for each enemy
    private const float ZPosition = -1f; // Fixed Z position for spawning enemies
    private const int MaxSpawnAttempts = 20;

    private List<Enemy> _enemies = new List<Enemy>(); // List to keep track of spawned enemies

    private Tilemap _groundMap;
    private Tilemap _wallMap;
    private Tilemap _objectsMap;
    private Bounds _spawnBounds;

    void Start()
    {
        // Start the enemy spawn coroutine
        StartCoroutine(SpawnEnemies());
    }

    void Awake()
    {
        CacheTilemaps();

        if (_groundMap == null)
        {
            Debug.LogError("EnemyController could not find GroundMap under the assigned MapGrid.");
            enabled = false;
            return;
        }

        _spawnBounds = GetTilemapWorldBounds(_groundMap);
    }

    private void CacheTilemaps()
    {
        if (MapGrid == null)
        {
            Debug.LogError("EnemyController requires a MapGrid reference.");
            return;
        }

        Tilemap[] tilemaps = MapGrid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            switch (tilemap.gameObject.name)
            {
                case "GroundMap":
                    _groundMap = tilemap;
                    break;
                case "WallMap":
                    _wallMap = tilemap;
                    break;
                case "ObjectsMap":
                    _objectsMap = tilemap;
                    break;
            }
        }

        if (_groundMap == null && tilemaps.Length > 0)
        {
            _groundMap = tilemaps[0];
        }
    }

    private Bounds GetTilemapWorldBounds(Tilemap tilemap)
    {
        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 min = tilemap.CellToWorld(cellBounds.min);
        Vector3 max = tilemap.CellToWorld(cellBounds.max);
        Vector3 center = (min + max) * 0.5f;
        Vector3 size = max - min;

        center.z = ZPosition;
        size.z = 0f;

        return new Bounds(center, size);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if the number of enemies is less than the maximum allowed
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < MaxEnemies)
            {
                // Spawn a new enemy
                SpawnEnemy();
            }

            // Wait for the specified spawn interval before spawning the next enemy
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        _enemyId++;
        if (!TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            Debug.LogWarning("EnemyController could not find a valid spawn position.");
            _enemyId--;
            return;
        }

        // Instantiate the enemy prefab at the spawn position with no rotation
        Enemy newEnemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

        // Set the enemy's target to the player
        newEnemy.Player = Player;

        // Set the enemy's tag to "Enemy" for identification
        newEnemy.gameObject.tag = "Enemy";
        // Set the enemy's name to "Enemy" for identification
        newEnemy.gameObject.name = $"Enemy {_enemyId}";

        _enemies.Add(newEnemy); // Add the new enemy to the list
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        for (int attempt = 0; attempt < MaxSpawnAttempts; attempt++)
        {
            Vector3 candidate = (Vector2)Player.transform.position + Random.insideUnitCircle * SpawnRadius;
            candidate = ClampToSpawnBounds(candidate);

            if (!IsSpawnCellWalkable(candidate, out Vector3 snappedSpawnPosition))
            {
                continue;
            }

            spawnPosition = snappedSpawnPosition;
            return true;
        }

        spawnPosition = Vector3.zero;
        return false;
    }

    private Vector3 ClampToSpawnBounds(Vector3 position)
    {
        Vector3 min = _spawnBounds.min;
        Vector3 max = _spawnBounds.max;

        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);
        position.z = ZPosition;

        return position;
    }

    private bool IsSpawnCellWalkable(Vector3 position, out Vector3 snappedSpawnPosition)
    {
        Vector3Int cellPosition = _groundMap.WorldToCell(position);
        if (!_groundMap.HasTile(cellPosition))
        {
            snappedSpawnPosition = Vector3.zero;
            return false;
        }

        if (_wallMap != null && _wallMap.HasTile(cellPosition))
        {
            snappedSpawnPosition = Vector3.zero;
            return false;
        }

        if (_objectsMap != null && _objectsMap.HasTile(cellPosition))
        {
            snappedSpawnPosition = Vector3.zero;
            return false;
        }

        snappedSpawnPosition = _groundMap.GetCellCenterWorld(cellPosition);
        snappedSpawnPosition.z = ZPosition;
        return true;
    }

    void Update()
    {
        // clear dead enemies from the list
        List<Enemy> deadEnemies = new List<Enemy>();
        foreach (Enemy enemy in _enemies)
        {
            if (enemy.IsDead())
            {
                deadEnemies.Add(enemy);
            }
        }
        foreach (Enemy deadEnemy in deadEnemies)
        {
            _enemies.Remove(deadEnemy);
            Destroy(deadEnemy.gameObject); // Destroy the enemy game object
        }
        // update player's enemies list
        Player.UpdateEnemyList(_enemies);
    }
}
