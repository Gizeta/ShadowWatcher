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

using ShadowWatcher.Asset;
using ShadowWatcher.Battle;
using ShadowWatcher.Deck;
using ShadowWatcher.Replay;
using ShadowWatcher.Socket;
using System;
using UnityEngine;

namespace ShadowWatcher
{
    public class Observer : MonoBehaviour
    {
        public static Action OnTick;

        private BattleManager battleManager = new BattleManager();
        private ReplayManager replayManager = new ReplayManager();

        public void Start()
        {
            InvokeRepeating("Tick", 0, 1);
        }

        private void Tick()
        {
            OnTick?.Invoke();
        }

        public void Awake()
        {
            Sender.Initialize();
            Receiver.Initialize(0);

            Receiver.OnReceived = Receiver_OnReceived;

            Sender.Send("Load", $"{Receiver.ListenPort}");
        }

        public void OnDestroy()
        {
            Sender.Send("Unload");
            Sender.Destroy();
            Receiver.Destroy();
        }

        public void LateUpdate()
        {
            try
            {
                if (Settings.RecordEnemyCard || Settings.RecordPlayerCard || Settings.ShowCountdown)
                    battleManager.Loop();

                if (Settings.EnhanceReplay)
                    replayManager.Loop();

                if (Settings.ShowSummonCard)
                    CardAllListEnhancer.SetUp();

                if (Settings.CopyAnimatedCardFirst)
                    DeckCardEditUIEnhancer.SetUp();
            }
            catch (Exception e)
            {
                Sender.Send("Error", $"{e.Message}\n{e.StackTrace}");
            }
        }

        private void Receiver_OnReceived(string action, string data)
        {
            try
            {
                switch (action)
                {
                    case "ReplayRequest":
                        if (Settings.EnhanceReplay)
                            replayManager.InjectReplay(data);
                        break;
                    case "Setting":
                        Settings.Parse(data);
                        Sender.Send("Setting", $"{data}");
                        break;
                    case "BasePath":
                        if (Settings.EnableMod)
                        {
                            var assetModder = new AssetModder(data);
                            assetModder.SetUp();
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Sender.Send("Error", $"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
