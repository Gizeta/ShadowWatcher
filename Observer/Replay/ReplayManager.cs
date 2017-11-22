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

using ShadowWatcher.Contract;
using ShadowWatcher.Helper;
using ShadowWatcher.Socket;
using System;
using Wizard;
using Wizard.Replay;

namespace ShadowWatcher.Replay
{
    public class ReplayManager
    {
        private GameMgr gameMgr = GameMgr.GetIns();
        private ReplayController repController;

        public void Loop()
        {
            if (gameMgr.IsNetworkBattle && gameMgr.IsReplayBattle)
            {
                gameMgr.IsAdmin = true;
            }

            if (gameMgr.GetProperty<ReplayController>("_ReplayControl") != repController)
            {
                repController = gameMgr.GetProperty<ReplayController>("_ReplayControl");

                if (repController != null)
                {
                    Sender.Send("ReplayDetail", $"{ReplayData.Parse(Data.ReplayBattleInfo)}");
                }
            }
        }

        public void InjectReplay(string json)
        {
            try
            {
                ReplayData.Parse(json).Assign();
                gameMgr._ReplayControl = new ReplayController();
            }
            catch(Exception e)
            {
                Sender.Send("Error", $"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
