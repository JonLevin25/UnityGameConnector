using TMPro;
using UnityEngine;

namespace GameConnection.UI
{
    public class ConnectionDebugOverlay : MonoBehaviour
    {
        [SerializeField] private TMP_Text debugText;
        [SerializeField] private TMP_Text payloadText;

        public void SetGame(int gameIdx, string payload)
        {
            debugText.text = $"Game: [{gameIdx}]";
            payloadText.text = $"Payload: {payload}";
        }

        public void SetEndScene()
        {
            debugText.text = $"END SCENE";
            payloadText.text = "";
        }
    }
}
