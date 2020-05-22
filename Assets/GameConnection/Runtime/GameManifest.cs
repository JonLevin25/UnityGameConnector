using NaughtyAttributes;
using UnityEngine;

namespace GameConnection
{
    /// <summary>
    /// A 
    /// </summary>
    public class GameManifest : ScriptableObject
    {
        [ReorderableList]
        [SerializeField] private Runtime_ConnectedGame[] connectedGames;
        
        // TODO: Order type validation

        public Runtime_ConnectedGame[] Games => connectedGames;
    }
}