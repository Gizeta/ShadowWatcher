using ShadowWatcher.Contract;
using ShadowWatcher.Socket;
using System.Collections.Generic;
using System.Linq;
using NetworkCardPlaceState = RealTimeNetworkBattleAgent.NetworkCardPlaceState;

namespace ShadowWatcher.Battle
{
    public class PlayerMonitor
    {
        private static BattleEnemy _enemy;
        private static BattlePlayer _player;
        private static bool _hasMulligan = false;
        private static bool _hasPlayerDrawn = false;
        
        public void CheckReference(BattlePlayer player, BattleEnemy enemy)
        {
            if (player != null && _player != player && Settings.RecordPlayerCard)
            {
                _player = player;
                _hasMulligan = false;
                _hasPlayerDrawn = false;

                _player.OnAddHandCardEvent += Player_OnAddHandCardEvent;
                _player.OnAddPlayCardEvent += Player_OnAddPlayCardEvent;
                _player.OnAddBanishEvent += Player_OnAddBanishEvent;
                _player.OnAddCemeteryEvent += Player_OnAddCemeteryEvent;
            }
            if (enemy != null && _enemy != enemy && Settings.RecordEnemyCard)
            {
                _enemy = enemy;

                _enemy.OnAddHandCardEvent += Enemy_OnAddHandCardEvent;
                _enemy.OnAddPlayCardEvent += Enemy_OnAddPlayCardEvent;
                _enemy.OnSpellPlayEvent += Enemy_OnSpellPlayEvent;
            }
        }

        ~PlayerMonitor()
        {
            if (_player != null)
            {
                _player.OnAddHandCardEvent -= Player_OnAddHandCardEvent;
                _player.OnAddPlayCardEvent -= Player_OnAddPlayCardEvent;
                _player.OnAddBanishEvent -= Player_OnAddBanishEvent;
                _player.OnAddCemeteryEvent -= Player_OnAddCemeteryEvent;
            }
            if (_enemy != null)
            {
                _enemy.OnAddHandCardEvent -= Enemy_OnAddHandCardEvent;
                _enemy.OnAddPlayCardEvent -= Enemy_OnAddPlayCardEvent;
                _enemy.OnSpellPlayEvent -= Enemy_OnSpellPlayEvent;
            }
        }

        #region BattleEnemy Events

        private void Enemy_OnAddHandCardEvent(BattleCardBase card, NetworkCardPlaceState fromState, bool isOpen)
        {
            if (fromState == NetworkCardPlaceState.None || fromState == NetworkCardPlaceState.Field)
            {
                Sender.Send($"EnemyAdd:{CardData.Parse(card, true)}");
            }
        }

        private void Enemy_OnAddPlayCardEvent(BattleCardBase card)
        {
            if (card.IsInHand || card.IsInDeck)
            {
                Sender.Send($"EnemyPlay:{CardData.Parse(card)}");
            }
        }

        private void Enemy_OnSpellPlayEvent(BattleCardBase card)
        {
            if (card.IsInHand || card.IsInDeck)
            {
                Sender.Send($"EnemyPlay:{CardData.Parse(card)}");
            }
        }

        #endregion

        #region BattlePlayer Events

        private void Player_OnAddHandCardEvent(BattleCardBase card, NetworkCardPlaceState fromState, bool isOpen)
        {
            if (!_hasMulligan)
            {
                _hasMulligan = true;

                var cardList = new List<string>();
                foreach (var c in _player.DeckCardList)
                {
                    cardList.Add($"{CardData.Parse(c)}");
                }
                cardList.Add($"{CardData.Parse(card)}");

                if (_player.Turn == 0)
                {
                    foreach (var c in _player.HandCardList)
                    {
                        cardList.Add($"{CardData.Parse(c)}");
                    }
                }

                Sender.Send($"PlayerDeck:{cardList.Aggregate((sum, s) => $"{sum}\n{s}")}");
            }
            else if (fromState == NetworkCardPlaceState.Stock && _player.Turn > 0)
            {
                if (!_hasPlayerDrawn && _player.Turn == 1)
                {
                    _hasPlayerDrawn = true;

                    foreach (var c in _player.HandCardList)
                    {
                        Sender.Send($"PlayerDraw:{CardData.Parse(c)}");
                    }
                }

                Sender.Send($"PlayerDraw:{CardData.Parse(card)}");
            }
        }

        private void Player_OnAddPlayCardEvent(BattleCardBase card)
        {
            if (card.IsInDeck)
            {
                Sender.Send($"PlayerDraw:{CardData.Parse(card)}");
            }
        }


        private void Player_OnAddBanishEvent(BattleCardBase card)
        {
            if (card.IsInDeck)
            {
                Sender.Send($"PlayerDraw:{CardData.Parse(card)}");
            }
        }

        private void Player_OnAddCemeteryEvent(BattleCardBase card, BattlePlayerBase.CEMETERY_TYPE cemeteryType, bool isOpen)
        {
            if (card.IsInDeck)
            {
                Sender.Send($"PlayerDraw:{CardData.Parse(card)}");
            }
        }

        #endregion
    }
}
