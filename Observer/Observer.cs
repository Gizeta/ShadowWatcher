using ShadowWatcher.Battle;
using System;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        private BattleManager battleManager = new BattleManager();

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
