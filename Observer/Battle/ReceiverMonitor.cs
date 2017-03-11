using System;
using System.Collections.Generic;
using Wizard;
using BattleFinishStatus = TaskManager.BattleFinishStatus;
using NetworkDataURI = RealTimeNetworkBattleAgent.NetworkDataURI;

namespace ShadowWatcher.Battle
{
    public class ReceiverMonitor
    {
        private static Action<Dictionary<string, object>> _onReceivedEvent;

        private static readonly Action<Dictionary<string, object>> receivedHandler = (dict) =>
        {
            var uri = (NetworkDataURI)Enum.Parse(typeof(NetworkDataURI), dict["uri"].ToString());

            switch (uri)
            {
                case NetworkDataURI.Ready:
                    Sender.Send("BattleReady.");
                    break;
                case NetworkDataURI.SpecialWin:
                case NetworkDataURI.Retire:
                    if (dict["isWin"].ToString() == "1")
                        Sender.Send("Win.");
                    else
                        Sender.Send("Lose.");
                    break;
                case NetworkDataURI.BattleFinish:
                    switch (ToolboxGame.RealTimeNetworkBattle.GetBattleManager().InvokeJudgeCurrentFinishStatus())
                    {
                        case BattleFinishStatus.Life_Win:
                        case BattleFinishStatus.ShortageDeck_Win:
                            Sender.Send("Win.");
                            break;
                        case BattleFinishStatus.Life_Lose:
                        case BattleFinishStatus.ShortageDeck_Lose:
                            Sender.Send("Lose.");
                            break;
                    }
                    break;
            }
        };

        public ReceiverMonitor(RealTimeNetworkBattleAgent agent)
        {
            if (_onReceivedEvent != null) _onReceivedEvent -= receivedHandler;
            agent.OnReceivedEvent += receivedHandler;
            _onReceivedEvent = agent.OnReceivedEvent;
        }
    }
}
