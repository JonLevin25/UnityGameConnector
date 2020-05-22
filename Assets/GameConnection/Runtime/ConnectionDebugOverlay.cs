using TMPro;
using UnityEngine;

public class ConnectionDebugOverlay : MonoBehaviour
{
    [SerializeField] private TMP_Text _debugText;
    
    public void SetGameIdx(int gameIdx)
    {
        _debugText.SetText($"Game [{gameIdx}]");
    }

    public void SetEndScene()
    {
        _debugText.SetText($"END SCENE");
    }
}
