using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConnection.Payloads;
using UnityEngine;

namespace NamesSelectionGame.Runtime
{
    public class SafeOpenGameController : MiniGameControllerBase<NamePayload, EndPayload>
    {
        [SerializeField] private CodeController codeController;
        [SerializeField] private GameObject demonModeParent; 
        [SerializeField] private GameObject angelModeParent;
            
        private NamePayload names;
        private IEnumerable<string> _demonicNames = new[]
        {
            "SATAN",
            "LEO",
            "666",
            "ANTICHRIST",
            "DEVIL"
        };

        protected override void InitInternal(NamePayload payload)
        {
            Debug.Log("Safe Open game init");
            names = payload;
            codeController.Init(payload.NemesisName, OnSafeUnlocked);
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
            throw new NotImplementedException();
        }

        private void AngelMode()
        {
            angelModeParent.SetActive(true);
            Debug.Log("Activated Angel Mode");
            throw new NotImplementedException();
        }

        private bool IsDemonicName(string name) => _demonicNames.Contains(name.ToUpper());
    }
}