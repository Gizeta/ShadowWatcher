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

using ShadowWatcher.Helper;
using ShadowWatcher.Socket;
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
            900031021,
            900041011,
            900041021,
            900041031,
            900044011,
            900044021,
            900044031,
            900111011,
            900111021,
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
            900311071,
            900312011,
            900314011,
            900314021,
            900314031,
            900314041,
            900314051,
            900314061,
            900334011,
            900334021,
            900411011,
            900411021,
            900411031,
            900411041,
            900411051,
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
            900711101,
            900711111,
            900711121,
            900731011,
            900733011,
            900743011,
            900743021,
            900743031,
            900811011,
            900811021,
            900811031,
            900811041,
            900811051,
            900841011,

#if KAMA
            704241011,
            704341011,
            704441011,
            704541011,
            704611011,
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
            Sender.Send("AddSummonCard");
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
            Sender.Send("RemoveSummonCard");
        }

        public void Update()
        {
            if (Settings.KeyboardFilterShortcut)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    var ui = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.CardAllList) as CardAllListUI;
                    var filter = ui.GetField<FilterController>("_filter");
                    #region Cost
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 2);
                        Sender.Send("Keyboard", "1");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 3);
                        Sender.Send("Keyboard", "2");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 4);
                        Sender.Send("Keyboard", "3");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 5);
                        Sender.Send("Keyboard", "4");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 6);
                        Sender.Send("Keyboard", "5");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 7);
                        Sender.Send("Keyboard", "6");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 8);
                        Sender.Send("Keyboard", "7");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 9);
                        Sender.Send("Keyboard", "8");
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.COST, 0);
                        Sender.Send("Keyboard", "0");
                    }
                    #endregion
                    #region Class
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 1);
                        Sender.Send("Keyboard", "Q");
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 2);
                        Sender.Send("Keyboard", "W");
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 3);
                        Sender.Send("Keyboard", "E");
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 4);
                        Sender.Send("Keyboard", "R");
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 5);
                        Sender.Send("Keyboard", "T");
                    }
                    else if (Input.GetKeyDown(KeyCode.Y))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 6);
                        Sender.Send("Keyboard", "Y");
                    }
                    else if (Input.GetKeyDown(KeyCode.U))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 7);
                        Sender.Send("Keyboard", "U");
                    }
                    else if (Input.GetKeyDown(KeyCode.I))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 8);
                        Sender.Send("Keyboard", "I");
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 9);
                        Sender.Send("Keyboard", "O");
                    }
                    else if (Input.GetKeyDown(KeyCode.P))
                    {
                        filter.SwitchFilter(DeckUIHelper.FILTER_TYPE.CLASS, 0);
                        Sender.Send("Keyboard", "P");
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
            var target = UIManager.GetInstance().GetUIBase(UIManager.ViewScene.CardAllList);
            target?.gameObject.AddMissingComponent<CardAllListEnhancer>();
        }
    }
}
