// Copyright 2017 Gizeta
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
                Sender.Send("ReplaceWithAnimatedCard");
            }
        }

        public void Update()
        {
            if (Settings.KeyboardFilterShortcut)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    var ui = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.DeckCardEdit) as DeckCardEditUI;
                    var filter = ui.GetFilterController();
                    #region Cost
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 1);
                        Sender.Send("Keyboard", "1");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 2);
                        Sender.Send("Keyboard", "2");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 3);
                        Sender.Send("Keyboard", "3");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 4);
                        Sender.Send("Keyboard", "4");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 5);
                        Sender.Send("Keyboard", "5");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 6);
                        Sender.Send("Keyboard", "6");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 7);
                        Sender.Send("Keyboard", "7");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 8);
                        Sender.Send("Keyboard", "8");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 0);
                        Sender.Send("Keyboard", "9");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 0);
                        Sender.Send("Keyboard", "0");
                    }
                    #endregion
                    #region Type
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.TYPE, 1);
                        Sender.Send("Keyboard", "A");
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.TYPE, 2);
                        Sender.Send("Keyboard", "S");
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.TYPE, 3);
                        Sender.Send("Keyboard", "D");
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.TYPE, 0);
                        Sender.Send("Keyboard", "F");
                    }
                    else if (Input.GetKeyDown(KeyCode.G))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.TYPE, 0);
                        Sender.Send("Keyboard", "G");
                    }
                    #endregion
                    #region Rarity
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 1);
                        Sender.Send("Keyboard", "Z");
                    }
                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 2);
                        Sender.Send("Keyboard", "X");
                    }
                    else if (Input.GetKeyDown(KeyCode.C))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 3);
                        Sender.Send("Keyboard", "C");
                    }
                    else if (Input.GetKeyDown(KeyCode.V))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 4);
                        Sender.Send("Keyboard", "V");
                    }
                    else if (Input.GetKeyDown(KeyCode.B))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 0);
                        Sender.Send("Keyboard", "B");
                    }
                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.RARITY, 0);
                        Sender.Send("Keyboard", "N");
                    }
                    #endregion
                }
            }
        }

        public static void SetUp()
        {
            var target = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.DeckCardEdit);
            target?.gameObject.AddMissingComponent<DeckCardEditUIEnhancer>();
        }
    }
}
