using System;
using System.Collections.Generic;
using System.Linq;
using Wizard;
using BattleFinishStatus = TaskManager.BattleFinishStatus;
using NetworkDataURI = RealTimeNetworkBattleAgent.NetworkDataURI;

namespace ShadowWatcher.Battle
{
    public class ReceiverMonitor
    {
        public ReceiverMonitor(RealTimeNetworkBattleAgent agent)
        {
            agent.OnReceivedEvent += receivedHandler;
        }

        private void receivedHandler(Dictionary<string, object> dict)
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
#if DEBUG
                case NetworkDataURI.PlayHand:
                case NetworkDataURI.PlayHandActions:
                    var str = new List<string>();
                    if (dict.ContainsKey("knownList"))
                    {
                        var list = dict["knownList"] as List<Object>;
                        foreach (var info in list)
                        {
                            var cardInfo = info as Dictionary<string, object>;
                            var cardId = cardInfo["card_id"].ToInt();
                            str.Add(CardMaster.GetInstance().GetCardParameterFromId(cardId).CardName);
                        }
                    }
                    Sender.Send($"PlayHand:{str.Aggregate((a, b) => $"{a},{b}")}");
                    break;
                default:
                    Sender.Send($"ReceiveMsg:{uri.ToString()}");
                    break;
#endif
            }
        }
    }
}
