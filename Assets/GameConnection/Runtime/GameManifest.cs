using NaughtyAttributes;
using UnityEngine;

namespace GameConnection
{
    /// <summary>
    /// A 
    /// </summary>
    public class GameManifest : ScriptableObject
    {
        // Build index of the first scene shown (should contain Game Connection initializer)
        [ReadOnly, SerializeField] private int startSceneIdx = 0; 
        
        // Build index of the scene shown when all games completed
        [ReadOnly, SerializeField] private int endSceneIdx;
        
        // Build index of the first scene that is a game (games are assumed to be sequential)
        [ReadOnly, SerializeField] private int firstGameIdx = 1; 
        
        [ReorderableList]
        [SerializeField] private ConnectedGame[] connectedGames;

        public ConnectedGame GetGame(int gameIdx) => connectedGames[gameIdx];
        public int GetGameSceneBuildIdx(int gameIdx) => firstGameIdx + gameIdx;
        
        // TODO: Validations that sequential games have compatible payload types 
        
        public ConnectedGame[] Games => connectedGames;
        public int StartSceneIdx => startSceneIdx;
        public int FirstGameIdx => firstGameIdx;
        public int LastGameIdx => firstGameIdx + Games.Length - 1;

        public int EndSceneBuildIdx => startSceneIdx;
    }
}