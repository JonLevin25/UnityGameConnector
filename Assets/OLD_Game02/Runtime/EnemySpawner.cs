using Game_02.Scripts;
using UnityEngine;

internal class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool overrideSpeed;
    [SerializeField] private Vector2 speed;

    public void Spawn(Enemy enemyPrefab)
    {
        var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        if (overrideSpeed)
        {
            enemy.Init(speed);
        }
    }
}