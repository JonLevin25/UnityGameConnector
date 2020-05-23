using System;
using GameConnection.Payloads;
using UnityEngine;

namespace NamesSelectionGame.Runtime
{
    public class SelectNamesGameController : MiniGameControllerBase<StartPayload, NamePayload>
    {
        [SerializeField] private NamePrompt playerNamePrompt;
        [SerializeField] private NamePrompt nemesisNamePrompt;

        private string playerName;
        private string nemesisName;

        private void Awake()
        {
            HideAll();
        }

        protected override void InitInternal(StartPayload inPayload)
        {
            Debug.Log("Select Names game init");
            Show(playerNamePrompt, playerName =>
            {
                this.playerName = playerName;
                Show(nemesisNamePrompt, nemesisName =>
                {
                    this.nemesisName = nemesisName;
                    EndGame();
                });
            });
        }

        private void EndGame()
        {
            var payload = new NamePayload(playerName, nemesisName);
            EndGame(payload);
        }

        private void Show(NamePrompt prompt, Action<string> onSubmit)
        {
            HideAll();
            prompt.gameObject.SetActive(true);
            prompt.OnSubmit += onSubmit;
        }

        private void HideAll()
        {
            playerNamePrompt.gameObject.SetActive(false);
            nemesisNamePrompt.gameObject.SetActive(false);
        }
    }
}