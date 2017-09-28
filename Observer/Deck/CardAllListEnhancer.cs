﻿using ShadowWatcher.Socket;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWatcher.Deck
{
    public class CardAllListEnhancer : MonoBehaviour
    {
        private static readonly List<int> summonCards = new List<int> {
            702114011,
            702211011,
            702221011,
            702324011,
            702431011,
            702434011,
            702511011,
            702624011,
            702721011,

            900011011,
            900011021,
            900011031,
            900011041,
            900021011,
            900031011,
            900041011,
            900041021,
            900041031,
            900044011,
            900044021,
            900111011,
            900131011,
            900141011,
            900144011,
            900211011,
            900211021,
            900211031,
            900211041,
            900211051,
            900211061,
            900211071,
            900232011,
            900241011,
            900242011,
            900311011,
            900311021,
            900311031,
            900311041,
            900311051,
            900311061,
            900312011,
            900314011,
            900314021,
            900334011,
            900334021,
            900411011,
            900411021,
            900411031,
            900441041,
            900511011,
            900511021,
            900511031,
            900511041,
            900541011,
            900541021,
            900541031,
            900544011,
            900544021,
            900611011,
            900611021,
            900641011,
            900711011,
            900711021,
            900711031,
            900711041,
            900711051,
            900711061,
            900711071,
            900711081,
            900711091,
            900733011,
            900743011,
            900743021,
            900743031,

#if KAMA
            704341011, // 次元魔女・ドロシー
            704441011, // インペリアルドラグーン・アルドス
#endif
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
