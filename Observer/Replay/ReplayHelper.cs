using ShadowWatcher.Helper;
using Wizard.Replay;

namespace ShadowWatcher.Replay
{
    public static class GameMgrHelper
    {
        public static ReplayController GetReplayControl(this GameMgr mgr)
        {
            return mgr.GetProperty("_ReplayControl").GetValue(mgr, null) as ReplayController;
        }
    }
}
