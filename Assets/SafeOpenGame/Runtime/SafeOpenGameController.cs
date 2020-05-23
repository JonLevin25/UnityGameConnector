using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConnection.Payloads;
using GameConnection.Payloads.TestPayloads;
using UnityEngine;
using UnityEngine.Serialization;

namespace NamesSelectionGame.Runtime
{
    public class SafeOpenGameController : MiniGameControllerBase<NamesPayload, EndPayload>
    {
        [SerializeField] private CodeController codeController;
        [SerializeField] private GameObject demonModeParent; 
        [SerializeField] private GameObject angelModeParent;

        [FormerlySerializedAs("testPslayerName")]
        [Header("Test Values")] 
        [SerializeField] private string testPlayerName = "Satan";
        [SerializeField] private string testEnemyName = "Bobby";
            
        private NamesPayload _names;
        private readonly IEnumerable<string> _demonicNames = new[]
        {
            "SATAN",
            "LEO",
            "666",
            "ANTICHRIST",
            "DEVIL"
        };

        private IEnumerator Start()
        {
            // FOR TESTING (start in game scene)
            yield return null;
            var wasInit = _names != null;
            if (!wasInit) InitInternal(new NamesPayload(testPlayerName, testEnemyName));
        }

        protected override void InitInternal(NamesPayload payload)
        {
            Debug.Log("Safe Open game init");
            _names = payload;
            codeController.Init(payload.NemesisName, OnSafeUnlocked);
            codeController.gameObject.SetActive(false);
            
            demonModeParent.SetActive(false);
            angelModeParent.SetActive(false);
            
            if (IsDemonicName(payload.PlayerName))
            {
                DemonMode();
            }
            else if (IsDemonicName(payload.NemesisName))
            {
                AngelMode();
            }
        }

        private void OnSafeUnlocked() => EndGame();
        
        public void EndGame() => EndGame(new EndPayload());

        private void DemonMode()
        {
            Debug.Log("Activated Demon Mode");
            demonModeParent.SetActive(true);
        }

        private void AngelMode()
        {
            angelModeParent.SetActive(true);
            Debug.Log("Activated Angel Mode");
        }

        private bool IsDemonicName(string name) => _demonicNames.Contains(name.ToUpper());
    }
}