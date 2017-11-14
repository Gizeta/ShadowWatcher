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
using Wizard;
using NetworkDataURI = RealTimeNetworkBattleAgent.NetworkDataURI;

namespace ShadowWatcher.Battle
{
    public class ReceiverMonitor
    {
        private RealTimeNetworkBattleAgent _agent;

        public ReceiverMonitor(RealTimeNetworkBattleAgent agent)
        {
            agent.OnReceivedEvent += receivedHandler;
            _agent = agent;
        }

        private void receivedHandler(Dictionary<string, object> dict)
        {
            var uri = (NetworkDataURI)Enum.Parse(typeof(NetworkDataURI), dict["uri"].ToString());

            switch (uri)
            {
                case NetworkDataURI.SpecialWin:
                case NetworkDataURI.DeckOutWin:
                case NetworkDataURI.Retire:
                    if (dict["isWin"].ToString() == "1")
                        Sender.Send("Win");
                    else
                        Sender.Send("Lose");
                    break;
                case NetworkDataURI.BattleFinish:
                    var code = (int)ToolboxGame.RealTimeNetworkBattle.GetBattleManager().JudgeCurrentFinishStatus();
                    if (code < 0x60 || code > 0xff)
                        break;
                    if (code % 2 == 0)
                        Sender.Send("Lose");
                    else
                        Sender.Send("Win");
                    break;
                case NetworkDataURI.OppoConnect:
                    Sender.Send("OppoConnect");
                    break;
                case NetworkDataURI.OppoDisconnect:
                    Sender.Send("OppoDisconnect");
                    break;
            }
        }

        ~ReceiverMonitor()
        {
            if (_agent != null)
                _agent.OnReceivedEvent -= receivedHandler;
        }
    }
}
