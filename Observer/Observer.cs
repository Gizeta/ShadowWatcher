using ShadowWatcher.Battle;
using System;
using System.Collections;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        private BattleManager battleManager = new BattleManager();

        public void Awake()
        {
            Sender.Initialize();
            StartCoroutine(sendLoad());
        }

        private IEnumerator sendLoad()
        {
            yield return new WaitForSeconds(1);
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
