using ShadowWatcher.Battle;
using ShadowWatcher.Replay;
using ShadowWatcher.Socket;
using System;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        private BattleManager battleManager = new BattleManager();
        private ReplayManager replayManager = new ReplayManager();

        public void Awake()
        {
            Sender.Initialize();
            Receiver.Initialize(0);

            Receiver.OnReceived = Receiver_OnReceived;

            Sender.Send($"Load:{Receiver.ListenPort}");
        }

        public void OnDestroy()
        {
            Sender.Send("Unload.");
            Sender.Destroy();
            Receiver.Destroy();
        }

        public void LateUpdate()
        {
            try
            {
                battleManager.Loop();
                replayManager.Loop();
            }
            catch (Exception e)
            {
                Sender.Send($"Error:{e.Message}\n{e.StackTrace}");
            }
        }

        private void Receiver_OnReceived(string action, string data)
        {
        }
    }
}
