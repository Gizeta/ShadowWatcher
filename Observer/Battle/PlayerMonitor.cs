using System.Collections.Generic;
using System.Linq;
using Wizard.Battle.Mulligan;
using CardParameter = Wizard.CardMaster.CardParameter;
using CharaType = CardBasePrm.CharaType;
using NetworkCardPlaceState = RealTimeNetworkBattleAgent.NetworkCardPlaceState;

namespace ShadowWatcher.Battle
{
    public class PlayerMonitor
    {
        private static BattleEnemy _enemy;
        private static BattlePlayer _player;
        private static IMulliganMgr _mulliganMgr;
        private static bool _hasPlayerDrawn = false;
        
        public void CheckReference(BattlePlayer player, BattleEnemy enemy, IMulliganMgr mulliganMgr)
        {
            if (player != null && _player != player)
            {
                _player = player;
                _hasPlayerDrawn = false;

                _player.OnAddHandCardEvent += Player_OnAddHandCardEvent;
            }
            if (enemy != null && _enemy != enemy)
            {
                _enemy = enemy;

                _enemy.OnAddHandCardEvent += Enemy_OnAddHandCardEvent;
                _enemy.OnAddPlayCardEvent += Enemy_OnAddPlayCardEvent;
                _enemy.OnSpellPlayEvent += Enemy_OnSpellPlayEvent;
            }
            if (mulliganMgr != null && _mulliganMgr != mulliganMgr)
            {
                _mulliganMgr = mulliganMgr;

                _mulliganMgr.OnSubmit += MulliganMgr_OnSubmit;
            }
        }

        #region BattleEnemy Events

        private void Enemy_OnAddHandCardEvent(BattleCardBase card, NetworkCardPlaceState fromState)
        {
            if (card.IsTokenLoad && !card.IsInDeck)
            {
                var param = card.BaseParameter;
                Sender.Send($"EnemyAdd:{convertCardId(param)},{param.CardName},{card.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyAddHandCard:{card.BaseParameter.CardName}");
            }
            Sender.Send($"EnemyAddHandCardFromState:{fromState.ToString()}");
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
            if (card.IsInDeck && _hasPlayerDrawn)
            {
                var param = card.BaseParameter;
                Sender.Send($"PlayerDraw:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"PlayerAddHand:{card.BaseParameter.CardName}");
            }
            Sender.Send($"PlayerAddHandCardFromState:{fromState.ToString()}");
#endif
        }

        #endregion

        #region MulliganMgr Event

        private void MulliganMgr_OnSubmit()
        {
            var cardList = new List<string>();
            foreach (var card in _player.DeckCardList)
            {
                var param = card.BaseParameter;
                cardList.Add($"{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
            Sender.Send($"PlayerDeck:{cardList.Aggregate((sum, s) => $"{sum};{s}")}");
            _hasPlayerDrawn = true;
        }

        #endregion

        private int convertCardId(CardParameter card)
        {
            var isClass = card.Clan > 0 && (int)card.Clan < 8;
            return (card.CardId / 10) % 100000000 + ((int)card.CharType + (isClass ? 1 : 0) * 10) * 100000000;
        }
    }
}
