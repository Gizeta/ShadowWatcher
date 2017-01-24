using System;
using System.Collections.Generic;
using System.Linq;
using Wizard;

using BattleFinishStatus = TaskManager.BattleFinishStatus;
using NetworkDataURI = RealTimeNetworkBattleAgent.NetworkDataURI;
using ReceiveData = NetworkBattleReceiver.ReceiveData;

namespace ShadowWatcher.Battle
{
    public static class ReceiveDataHandler
    {
        private static ReceiveData lastReceiveData = new ReceiveData();

        public static void Deal(RealTimeNetworkBattleAgent agent, Dictionary<string, object> dict)
        {
            var uri = (NetworkDataURI)Enum.Parse(typeof(NetworkDataURI), dict["uri"].ToString());
            
            switch (uri)
            {
                case NetworkDataURI.Ready:
                    Sender.Send("Ready.");
                    break;
                case NetworkDataURI.SpecialWin:
                case NetworkDataURI.Retire:
                    if (dict["isWin"].ToString() == "1")
                        Sender.Send("Win.");
                    else
                        Sender.Send("Lose.");
                    break;
                case NetworkDataURI.BattleFinish:
                    switch (agent.GetWinStatus())
                    {
                        case BattleFinishStatus.Disconnecte_Win:
                        case BattleFinishStatus.Life_Win:
                        case BattleFinishStatus.Mulligan_Win:
                        case BattleFinishStatus.Retire_Win:
                        case BattleFinishStatus.ShortageDeck_Win:
                        case BattleFinishStatus.Special_Win:
                        case BattleFinishStatus.TurnStart_Win:
                        case BattleFinishStatus.TurnEnd_Win:
                            Sender.Send("Win.");
                            break;
                        case BattleFinishStatus.Disconnecte_Lose:
                        case BattleFinishStatus.Life_Lose:
                        case BattleFinishStatus.Mulligan_Lose:
                        case BattleFinishStatus.Retire_Lose:
                        case BattleFinishStatus.ShortageDeck_Lose:
                        case BattleFinishStatus.Special_Lose:
                            Sender.Send("Lose.");
                            break;
                    }
                    break;
                case NetworkDataURI.PlayHand:
                case NetworkDataURI.PlayHandActions:
                    var str = new List<string>();
                    if (dict.ContainsKey("knownList"))
                    {
                        var list = dict["knownList"] as List<object>;
                        foreach (var info in list)
                        {
                            var cardInfo = info as Dictionary<string, object>;
                            var cardId = cardInfo["card_id"].ToInt();
                            var cardParam = CardMaster.GetInstance().GetCardParameterFromId(cardId);
                            str.Add($"{cardParam.CardId + (int)cardParam.CharType * 10000 + (int)cardParam.Clan * 100000},{cardParam.CardName},{cardParam.Cost}");
                        }
                    }
                    Sender.Send($"PlayHand:{str.Aggregate((a, b) => $"{a};{b}")}");
                    break;
                case NetworkDataURI.OppoDisconnect:
                    Sender.Send("OppoDisconnect.");
                    break;
                case NetworkDataURI.Alive:
                case NetworkDataURI.Loaded:
                case NetworkDataURI.Judge:
                case NetworkDataURI.TurnStart:
                case NetworkDataURI.TurnEnd:
                case NetworkDataURI.PlayActions:
                    break;
                default:
                    break;
            }
        }
    }
}
