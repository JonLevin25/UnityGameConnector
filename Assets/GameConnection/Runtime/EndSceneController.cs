using UnityEngine;

namespace GameConnection
{
    public class EndSceneController : MonoBehaviour
    {
        private GameManifest _manifest;
        
        public void Init(GameManifest manifest)
        {
            _manifest = manifest;
        }
    }
}
