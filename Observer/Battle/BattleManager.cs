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

                    if (gameMgr.IsReplayBattle)
                    {
                        gameMgr.IsAdmin = true;
                    }
                }

                var battleMgr = agent.GetBattleManager();
                if (battleMgr != null)
                {
                    playerMon.CheckReference(battleMgr.BattlePlayer, battleMgr.BattleEnemy);
                }
            }
        }
    }
}
