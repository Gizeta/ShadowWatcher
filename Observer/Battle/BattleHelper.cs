using ShadowWatcher.Helper;
using BattleFinishStatus = TaskManager.BattleFinishStatus;

namespace ShadowWatcher.Battle
{
    public static class BattleAgentHelper
    {
        public static int ToInt(this object o)
        {
            return int.Parse(o.ToString());
        }

        public static NetworkBattleManagerBase GetBattleManager(this RealTimeNetworkBattleAgent agent)
        {
            return agent.GetField("networkBattleMgr").GetValue(agent) as NetworkBattleManagerBase;
        }
    }

    public static class BattleManagerHelper
    {
        public static BattleFinishStatus InvokeJudgeCurrentFinishStatus(this NetworkBattleManagerBase manager)
        {
            return (BattleFinishStatus)manager.GetMethod("JudgeCurrentFinishStatus").Invoke(manager, null);
        }
    }
}
