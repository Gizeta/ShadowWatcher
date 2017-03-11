using Wizard;

namespace ShadowWatcher.Battle
{
    public class BattleManager
    {
        private GameMgr gameMgr = GameMgr.GetIns();
        private static RealTimeNetworkBattleAgent agent;
        private ReceiverMonitor receiverMon;

        public void Loop()
        {
            if (gameMgr.IsNetworkBattle)
            {
                if (agent != ToolboxGame.RealTimeNetworkBattle)
                {
                    agent = ToolboxGame.RealTimeNetworkBattle;
                    receiverMon = new ReceiverMonitor(agent);
                }
            }
        }
    }
}
