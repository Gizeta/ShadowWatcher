using ShadowWatcher.Contract;
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
            if (gameMgr.GetReplayControl() != repController)
            {
                repController = gameMgr.GetReplayControl();

                if (repController != null)
                {
                    Sender.Send($"ReplayDetail:{ReplayData.Parse(Data.ReplayBattleInfo)}");
                }
            }
        }

        public void InjectReplay(string json)
        {
            try
            {
                ReplayData.Parse(json).Assign();
                gameMgr._ReplayControl = new ReplayController();
                Sender.Send("OK.");
            }
            catch(Exception e)
            {
                Sender.Send($"Error:{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
