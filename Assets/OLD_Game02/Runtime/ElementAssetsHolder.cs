using System;
using UnityEngine;

namespace Game_02.Scripts
{
    [Serializable]
    public class ElementAssetsHolder
    {
        [SerializeField] private Element element;
        [SerializeField] private G02_CharController charPrefab;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Enemy enemyPrefab;

        internal Element Element => element;
        internal G02_CharController CharPrefab => charPrefab;
        internal Projectile ProjectilePrefab => projectilePrefab;
        internal Enemy EnemyPrefab => enemyPrefab;
    }
}