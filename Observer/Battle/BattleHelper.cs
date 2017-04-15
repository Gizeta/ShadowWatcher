using ShadowWatcher.Helper;

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
}
