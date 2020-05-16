using Game_02.Scripts;
using UnityEngine;

internal class EnemyManager : MonoBehaviour
{
    private EnemySpawner[] spawners;

    private void Awake()
    {
        spawners = GetComponentsInChildren<EnemySpawner>();
    }

    public void Init(Enemy enemyPrefab)
    {
        foreach (var spawner in spawners)
        {
            spawner.Spawn(enemyPrefab);
        }
    }
}