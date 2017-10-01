using ShadowWatcher.Socket;
using System;
using System.Collections.Generic;
using UnityEngine;
using Wizard.DeckCardEdit;

namespace ShadowWatcher.Deck
{
    public class DeckCardEditUIEnhancer : MonoBehaviour
    {
        public void OnEnable()
        {
            if (DeckCardEditUI.IsCreatedByBuilder && DeckCardEditUI.CopySrcDeckData != null)
            {
                var cards = DeckCardEditUI.CopySrcDeckData.GetCardIdList();
                var ownCards = GameMgr.GetIns().GetDataMgrIns().GetUserOwnCardData();
                var list = new List<int>(40);

                cards.Sort();
                for (var i = 0; i < cards.Count - 1;)
                {
                    var id = cards[i];
                    var count = 1;
                    while (i < cards.Count - 1 && cards[++i] == id) count++;

                    if (id % 10 == 0)
                    {
                        var id2 = id | 1;
                        var count2 = ownCards.ContainsKey(id2) ? Math.Min(ownCards[id2], count) : 0;
                        for (var j = 0; j < count2; j++)
                        {
                            list.Add(id2);
                        }
                        for (var j = 0; j < count - count2; j++)
                        {
                            list.Add(id);
                        }
                    }
                }

                DeckCardEditUI.CopySrcDeckData.SetCardIdList(list);
                Sender.Send("ReplaceAnimatedCard.");
            }
        }

        public static void SetUp()
        {
            var target = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.DeckCardEdit);
            target?.gameObject.AddMissingComponent<DeckCardEditUIEnhancer>();
        }
    }
}
