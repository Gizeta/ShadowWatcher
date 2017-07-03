using ShadowWatcher.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using Wizard;
using NetworkDataURI = RealTimeNetworkBattleAgent.NetworkDataURI;

namespace ShadowWatcher.Battle
{
    public class ReceiverMonitor
    {
        private RealTimeNetworkBattleAgent _agent;

        public ReceiverMonitor(RealTimeNetworkBattleAgent agent)
        {
            agent.OnReceivedEvent += receivedHandler;
            _agent = agent;
        }

        private void receivedHandler(Dictionary<string, object> dict)
        {
            var uri = (NetworkDataURI)Enum.Parse(typeof(NetworkDataURI), dict["uri"].ToString());

            switch (uri)
            {
                case NetworkDataURI.SpecialWin:
                case NetworkDataURI.Retire:
                    if (dict["isWin"].ToString() == "1")
                        Sender.Send("Win.");
                    else
                        Sender.Send("Lose.");
                    break;
                case NetworkDataURI.BattleFinish:
                    var code = (int)ToolboxGame.RealTimeNetworkBattle.GetBattleManager().JudgeCurrentFinishStatus();
                    if (code < 0x60 || code > 0xff)
                        break;
                    if (code % 2 == 0)
                        Sender.Send("Lose.");
                    else
                        Sender.Send("Win.");
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

        ~ReceiverMonitor()
        {
            if (_agent != null)
                _agent.OnReceivedEvent -= receivedHandler;
        }
    }
}
