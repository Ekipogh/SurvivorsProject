using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Player player;
    public Enemy enemyPrefab;

    public int spawnInterval = 2; // Time interval between spawns in seconds
    public int maxEnemies = 5; // Maximum number of enemies allowed in the scene
    private const float _spawnRadius = 5f; // Radius around the player to spawn enemies
    private int _enemyId = 0; // Unique ID for each enemy
    private const float _zPosition = -1f; // Fixed Z position for spawning enemies

    void Start()
    {
        // Start the enemy spawn coroutine
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if the number of enemies is less than the maximum allowed
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
            {
                // Spawn a new enemy
                SpawnEnemy();
            }

            // Wait for the specified spawn interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        _enemyId++;
        // Generate a random position within the spawn radius around the player
        Vector3 spawnPosition = (Vector2)player.transform.position + Random.insideUnitCircle * _spawnRadius;
        // Ensure the spawn position is within the bounds of the game world
        spawnPosition.z = _zPosition; // Set the Z position to a fixed value

        // Instantiate the enemy prefab at the spawn position with no rotation
        Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Set the enemy's target to the player
        newEnemy.player = player;

        // Set the enemy's tag to "Enemy" for identification
        newEnemy.gameObject.tag = "Enemy";
        // Set the enemy's name to "Enemy" for identification
        newEnemy.gameObject.name = $"Enemy {_enemyId}";
    }
}
