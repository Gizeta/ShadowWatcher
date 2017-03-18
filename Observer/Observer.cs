using ShadowWatcher.Battle;
using System;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        private BattleManager battleManager = new BattleManager();

        public void Awake()
        {
            Sender.Initialize();
            Sender.Send("Load.");
        }

        public void OnDestroy()
        {
            Sender.Send("Unload.");
            Sender.Destroy();
        }

        public void LateUpdate()
        {
            try
            {
                battleManager.Loop();
            }
            catch(Exception e)
            {
                Sender.Send($"Error:{e.Message}");
            }
        }
    }
}
