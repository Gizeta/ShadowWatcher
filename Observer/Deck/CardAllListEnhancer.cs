using ShadowWatcher.Socket;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWatcher.Deck
{
    public class CardAllListEnhancer : MonoBehaviour
    {
        private static readonly List<int> summonCards = new List<int> {
            702114010,
            702211010,
            702221010,
            702324010,
            702431010,
            702434010,
            702511010,
            702624010,
            702721010,

            900011010,
            900011020,
            900011030,
            900011040,
            900021010,
            900031010,
            900041010,
            900041020,
            900041030,
            900044010,
            900044020,
            900111010,
            900131010,
            900141010,
            900144010,
            900211010,
            900211020,
            900211030,
            900211040,
            900211050,
            900211060,
            900211070,
            900232010,
            900241010,
            900242010,
            900311010,
            900311020,
            900311030,
            900311040,
            900311050,
            900311060,
            900312010,
            900314010,
            900314020,
            900334010,
            900334020,
            900411010,
            900411020,
            900411030,
            900441040,
            900511010,
            900511020,
            900511030,
            900511040,
            900541010,
            900541020,
            900541030,
            900544010,
            900544020,
            900611010,
            900611020,
            900641010,
            900711010,
            900711020,
            900711030,
            900711040,
            900711050,
            900711060,
            900711070,
            900711080,
            900711090,
            900733010,
            900743010,
            900743020,
            900743030,
        };

        public void OnEnable()
        {
            var ownCards = GameMgr.GetIns().GetDataMgrIns().GetUserOwnCardData();
            foreach (var id in summonCards)
            {
                if (ownCards.ContainsKey(id))
                    ownCards[id]++;
                else
                    ownCards.Add(id, 1);
            }
            Sender.Send($"AddSummonCard.");
        }

        public void OnDisable()
        {
            var ownCards = GameMgr.GetIns().GetDataMgrIns().GetUserOwnCardData();
            foreach (var id in summonCards)
            {
                if (ownCards[id] == 1)
                    ownCards.Remove(id);
                else
                    ownCards[id]--;
            }
            Sender.Send("RemoveSummonCard.");
        }

        public static void SetUp()
        {
            var target = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.CardAllList);
            target?.gameObject.AddMissingComponent<CardAllListEnhancer>();
        }
    }
}
