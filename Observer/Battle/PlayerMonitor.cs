using System.Collections.Generic;
using System.Linq;
using CardParameter = Wizard.CardMaster.CardParameter;
using CharaType = CardBasePrm.CharaType;
using NetworkCardPlaceState = RealTimeNetworkBattleAgent.NetworkCardPlaceState;

namespace ShadowWatcher.Battle
{
    public class PlayerMonitor
    {
        private static BattleEnemy _enemy;
        private static BattlePlayer _player;
        private static bool _hasPlayerDrawn = false;
        
        public void CheckReference(BattlePlayer player, BattleEnemy enemy)
        {
            if (player != null && _player != player)
            {
                _player = player;
                _hasPlayerDrawn = false;

                _player.OnAddHandCardEvent += Player_OnAddHandCardEvent;
                _player.OnAddPlayCardEvent += Player_OnAddPlayCardEvent;
            }
            if (enemy != null && _enemy != enemy)
            {
                _enemy = enemy;

                _enemy.OnAddHandCardEvent += Enemy_OnAddHandCardEvent;
                _enemy.OnAddPlayCardEvent += Enemy_OnAddPlayCardEvent;
                _enemy.OnSpellPlayEvent += Enemy_OnSpellPlayEvent;
            }
        }

        #region BattleEnemy Events

        private void Enemy_OnAddHandCardEvent(BattleCardBase card, NetworkCardPlaceState fromState)
        {
            if (fromState == NetworkCardPlaceState.None || fromState == NetworkCardPlaceState.Field)
            {
                var param = card.BaseParameter;
                Sender.Send($"EnemyAdd:{convertCardId(param)},{param.CardName},{card.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyAddHandCard:{card.BaseParameter.CardName},{fromState.ToString()}");
            }
#endif
        }

        private void Enemy_OnAddPlayCardEvent(BattleCardBase obj)
        {
            if (obj.IsInHand || obj.IsInDeck)
            {
                var param = obj.BaseParameter;
                Sender.Send($"EnemyPlay:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyPlayCard:{obj.BaseParameter.CardName}");
            }
#endif
        }

        private void Enemy_OnSpellPlayEvent(BattleCardBase obj)
        {
            if (obj.IsInHand || obj.IsInDeck)
            {
                var param = obj.BaseParameter;
                Sender.Send($"EnemyPlay:{convertCardId(param)},{param.CardName},{param.Cost}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemySpellPlay:{obj.BaseParameter.CardName}");
            }
#endif
        }

        #endregion

        #region BattlePlayer Events

        private void Player_OnAddHandCardEvent(BattleCardBase card, NetworkCardPlaceState fromState)
        {
            if (!_hasPlayerDrawn && _player.Turn == 0)
            {
                var cardList = new List<string>();
                foreach (var c in _player.DeckCardList)
                {
                    var p = c.BaseParameter;
                    cardList.Add($"{convertCardId(p)},{p.CardName},{p.Cost}{(p.CharType == CharaType.NORMAL ? $",{p.Atk},{p.Life}" : "")}");
                }
                var param = card.BaseParameter;
                cardList.Add($"{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
                Sender.Send($"PlayerDeck:{cardList.Aggregate((sum, s) => $"{sum};{s}")}");

                _hasPlayerDrawn = true;
            }
            else if (fromState == NetworkCardPlaceState.Stock && _player.Turn == 1)
            {
                foreach (var c in _player.HandCardList)
                {
                    var p = c.BaseParameter;
                    Sender.Send($"PlayerDraw:{convertCardId(p)},{p.CardName},{p.Cost}{(p.CharType == CharaType.NORMAL ? $",{p.Atk},{p.Life}" : "")}");
                }
                var param = card.BaseParameter;
                Sender.Send($"PlayerDraw:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
            else if (fromState == NetworkCardPlaceState.Stock && _player.Turn > 1)
            {
                var param = card.BaseParameter;
                Sender.Send($"PlayerDraw:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"PlayerAddHand:{card.BaseParameter.CardName},{fromState.ToString()}");
            }
#endif
        }

        private void Player_OnAddPlayCardEvent(BattleCardBase card)
        {
            if (card.IsInDeck)
            {
                var param = card.BaseParameter;
                Sender.Send($"PlayerDraw:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"PlayerPlayCard:{card.BaseParameter.CardName}");
            }
#endif
        }

        #endregion

        private int convertCardId(CardParameter card)
        {
            var d = card.CardId.ToString();
            return int.Parse($"{d[3]}{d[5]}{d[2]}{d[6]}{d[7]}");
        }
    }
}
