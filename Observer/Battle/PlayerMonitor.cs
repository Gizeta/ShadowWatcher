using ShadowWatcher.Socket;
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
                Sender.Send($"EnemyAdd:{getCardInfo(card, true)}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyAddHandCard:{card.BaseParameter.CardName},{fromState.ToString()}");
            }
#endif
        }

        private void Enemy_OnAddPlayCardEvent(BattleCardBase card)
        {
            if (card.IsInHand || card.IsInDeck)
            {
                Sender.Send($"EnemyPlay:{getCardInfo(card)}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyPlayCard:{card.BaseParameter.CardName}");
            }
#endif
        }

        private void Enemy_OnSpellPlayEvent(BattleCardBase card)
        {
            if (card.IsInHand || card.IsInDeck)
            {
                Sender.Send($"EnemyPlay:{getCardInfo(card)}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemySpellPlay:{card.BaseParameter.CardName}");
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
                    cardList.Add($"{getCardInfo(c)}");
                }
                cardList.Add($"{getCardInfo(card)}");
                Sender.Send($"PlayerDeck:{cardList.Aggregate((sum, s) => $"{sum};{s}")}");

                _hasPlayerDrawn = true;
            }
            else if (fromState == NetworkCardPlaceState.Stock && _player.Turn == 1)
            {
                foreach (var c in _player.HandCardList)
                {
                    Sender.Send($"PlayerDraw:{getCardInfo(c)}");
                }
                Sender.Send($"PlayerDraw:{getCardInfo(card)}");
            }
            else if (fromState == NetworkCardPlaceState.Stock && _player.Turn > 1)
            {
                Sender.Send($"PlayerDraw:{getCardInfo(card)}");
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
                Sender.Send($"PlayerDraw:{getCardInfo(card)}");
            }
#if DEBUG
            else
            {
                Sender.Send($"PlayerPlayCard:{card.BaseParameter.CardName}");
            }
#endif
        }

        #endregion

        private string getCardInfo(BattleCardBase card, bool realCost = false)
        {
            var param = card.BaseParameter;
            return $"{convertCardId(param)},{param.CardName},{(realCost ? card.Cost : param.Cost)}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}";
        }

        private int convertCardId(CardParameter card)
        {
            var d = card.CardId.ToString();
            return int.Parse($"{d[3]}{d[5]}{d[2]}{d[6]}{d[7]}");
        }
    }
}
