using ShadowWatcher.Contract;
using ShadowWatcher.Socket;
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
            ReplayData.Parse(json).AssignTo(Data.ReplayBattleInfo);
            gameMgr._ReplayControl = new ReplayController();
        }
    }
}
