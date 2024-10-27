using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;




public class EnemySpawner : MonoBehaviour
{
    [FoldoutGroup("Stats")]
    public EnemyBiomePool enemyPool;
    [FoldoutGroup("Stats")]
    public EnemySpawnSettings spawnSettings;
    [FoldoutGroup("Stats")]
    public float SpawnRadius = 10;

    //Calculate
    [FoldoutGroup("Debug")]
    public List<GameObject> enemiesToSpawn, matchingPrefabs, enemies;
    [FoldoutGroup("Debug")]
    private Vector3 potentialPosition, spawnPosition;

    private int randomIndex;
    private Vector2 randomPoint;

    private void Start()
    {
        LoadEnemiesToSpawn();
        Spawn(transform.position, SpawnRadius);
    }

    [FoldoutGroup("Event Test")]
    [Button]
    public void Spawn(Vector3 position, float spawnRadius)
    {
        foreach (var enemy in enemiesToSpawn)
        {
            // Find a valid spawn position
            spawnPosition = FindValidSpawnPosition(position, spawnRadius);

            // Check if a valid position was found
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("No valid spawn position");
                return;
            }
            Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
        }
    }

    [FoldoutGroup("Event Test")]
    [Button]
    public void LoadEnemiesToSpawn()
    {
        if (enemiesToSpawn != null) enemiesToSpawn.Clear();

        // Add enemies based on the counts in the Scriptable Object
        if (spawnSettings.easyCount > 0)
            enemiesToSpawn.AddRange(SpawnEnemiesOfType(EnemyDifficulty.Easy, spawnSettings.easyCount));
        if (spawnSettings.normalCount > 0)
            enemiesToSpawn.AddRange(SpawnEnemiesOfType(EnemyDifficulty.Normal, spawnSettings.normalCount));
        if (spawnSettings.hardCount > 0)
            enemiesToSpawn.AddRange(SpawnEnemiesOfType(EnemyDifficulty.Hard, spawnSettings.hardCount));
        if (spawnSettings.specialCount > 0)
            enemiesToSpawn.AddRange(SpawnEnemiesOfType(EnemyDifficulty.Special, spawnSettings.specialCount));
        if (spawnSettings.bossesCount > 0)
            enemiesToSpawn.AddRange(SpawnEnemiesOfType(EnemyDifficulty.Bosses, spawnSettings.bossesCount));
    }

    #region Calculate

    private Vector3 FindValidSpawnPosition(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a valid position
        {
            // Generate a random point within a circle
            randomPoint = Random.insideUnitSphere * radius;
            potentialPosition = center + new Vector3(randomPoint.x, 0, randomPoint.y);
            // Check if the potential position is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(potentialPosition, out hit, radius, NavMesh.AllAreas))
                return hit.position; // Return the valid position found
        }
        return Vector3.zero; // Return zero vector if no valid position found
    }

    private List<GameObject> SpawnEnemiesOfType(EnemyDifficulty difficulty, int count)
    {
        if (enemies != null) enemies.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject enemyPrefab = GetEnemyPrefab(difficulty);
            if (enemyPrefab != null)
                enemies.Add(enemyPrefab);
        }
        return enemies;
    }
    private GameObject GetEnemyPrefab(EnemyDifficulty difficulty)
    {
        matchingPrefabs.Clear();
        // Collect all prefabs of the specified difficulty
        foreach (var enemySpawn in enemyPool.enemyPool)
        {
            if (enemySpawn.enemyDifficulty == difficulty)
                matchingPrefabs.Add(enemySpawn.EnemyPrefab);
        }
        // If there are matching prefabs, pick one at random
        if (matchingPrefabs.Count > 0)
        {
            randomIndex = Random.Range(0, matchingPrefabs.Count);
            return matchingPrefabs[randomIndex];
        }
        return null; // Return null if no prefab found
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green; // Set the color of the Gizmo
        DebugExtension.DrawCircle(transform.position, Vector3.up, Color.green, SpawnRadius);
    }

    #endregion
}
