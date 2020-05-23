using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConnection.Payloads;
using GameConnection.Payloads.TestPayloads;
using UnityEngine;

namespace NamesSelectionGame.Runtime
{
    public class SafeOpenGameController : MiniGameControllerBase<NamesPayload, EndPayload>
    {
        [SerializeField] private CodeController codeController;
        [SerializeField] private GameObject demonModeParent; 
        [SerializeField] private GameObject angelModeParent;
            
        private NamesPayload _nameses;
        private IEnumerable<string> _demonicNames = new[]
        {
            "SATAN",
            "LEO",
            "666",
            "ANTICHRIST",
            "DEVIL"
        };

        protected override void InitInternal(NamesPayload payload)
        {
            Debug.Log("Safe Open game init");
            _nameses = payload;
            codeController.Init(payload.NemesisName, OnSafeUnlocked);
            codeController.gameObject.SetActive(false);
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