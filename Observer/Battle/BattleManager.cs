using System;
using System.Collections.Generic;
using Wizard;

namespace ShadowWatcher.Battle
{
    public class BattleManager
    {
        private GameMgr gameMgr = GameMgr.GetIns();
        private static RealTimeNetworkBattleAgent agent;
        private Action<Dictionary<string, object>> lastOnReceivedEvent;

        private static bool error = false;

        private static readonly Action<Dictionary<string, object>> receivedHandler = (dict) =>
        {
            try
            {
                ReceiveDataHandler.Deal(agent, dict);
            }
            catch (Exception e)
            {
                Sender.Send($"Error:{e.Message}, {e.StackTrace}");
            }
        };

        public void Poll()
        {
            if (gameMgr.IsNetworkBattle)
            {
                if (ReferenceEquals(agent, ToolboxGame.RealTimeNetworkBattle))
                {
                    if (!ReferenceEquals(lastOnReceivedEvent, agent.OnReceivedEvent))
                    {
                        bindEvent(agent);
                        lastOnReceivedEvent = agent.OnReceivedEvent;
                    }
                }
                else
                {
                    if (agent != null)
                    {
                        unbindEvent(agent);
                    }
                    agent = ToolboxGame.RealTimeNetworkBattle;
                    bindEvent(agent);
                    lastOnReceivedEvent = agent.OnReceivedEvent;
                }
            }
        }

        private void bindEvent(RealTimeNetworkBattleAgent agent)
        {
            agent.OnReceivedEvent += receivedHandler;
        }

        private void unbindEvent(RealTimeNetworkBattleAgent agent)
        {
            agent.OnReceivedEvent -= receivedHandler;
        }
    }
}
