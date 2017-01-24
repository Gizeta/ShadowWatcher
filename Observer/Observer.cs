using ShadowWatcher.Battle;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        private BattleManager battleManager = new BattleManager();

        public void LateUpdate()
        {
            battleManager.Poll();
        }
    }
}
