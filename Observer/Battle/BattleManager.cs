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
using Wizard;

namespace ShadowWatcher.Battle
{
    public class BattleManager
    {
        private GameMgr gameMgr = GameMgr.GetIns();
        private static RealTimeNetworkBattleAgent agent;
        private PlayerMonitor playerMon = new PlayerMonitor();
        private ReceiverMonitor receiverMon;

        public void Loop()
        {
            if (gameMgr.IsNetworkBattle)
            {
                if (agent != ToolboxGame.RealTimeNetworkBattle)
                {
                    agent = ToolboxGame.RealTimeNetworkBattle;
                    receiverMon = new ReceiverMonitor(agent);

                    Sender.Send("BattleReady");
                }

                var battleMgr = agent.GetField<NetworkBattleManagerBase>("networkBattleMgr");
                if (battleMgr != null)
                {
                    playerMon.CheckReference(battleMgr.BattlePlayer, battleMgr.BattleEnemy);
                }
            }
        }
    }
}
