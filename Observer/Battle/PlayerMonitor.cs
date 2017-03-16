using System.Collections.Generic;
using System.Linq;
using CardParameter = Wizard.CardMaster.CardParameter;
using CharaType = CardBasePrm.CharaType;

namespace ShadowWatcher.Battle
{
    public class PlayerMonitor
    {
        private static BattleEnemy _enemy;
        private static BattlePlayer _player;
        private static bool _hasPlayerDrawn = false;
        
        public void CheckReference(BattlePlayer player, BattleEnemy enemy)
        {
            if (_player != player)
            {
                if (_player != null) unbindPlayerEvent();
                _player = player;
                _hasPlayerDrawn = false;
                bindPlayerEvent();
            }
            if (_enemy != enemy)
            {
                if (_enemy != null) unbindEnemyEvent();
                _enemy = enemy;
                bindEnemyEvent();
            }
        }

        private void bindPlayerEvent()
        {
            _player.OnAddHandCardEvent += Player_OnAddHandCardEvent;
            _player.OnMulliganEnd += Player_OnMulliganEnd;
        }

        private void unbindPlayerEvent()
        {
            _player.OnAddHandCardEvent -= Player_OnAddHandCardEvent;
            _player.OnMulliganEnd -= Player_OnMulliganEnd;
        }

        private void bindEnemyEvent()
        {
            _enemy.OnAddHandCardEvent += Enemy_OnAddHandCardEvent;
            _enemy.OnAddPlayCardEvent += Enemy_OnAddPlayCardEvent;
            _enemy.OnSpellPlayEvent += Enemy_OnSpellPlayEvent;
        }

        private void unbindEnemyEvent()
        {
            _enemy.OnAddHandCardEvent -= Enemy_OnAddHandCardEvent;
            _enemy.OnAddPlayCardEvent -= Enemy_OnAddPlayCardEvent;
            _enemy.OnSpellPlayEvent -= Enemy_OnSpellPlayEvent;
        }

        #region BattleEnemy Events

        private void Enemy_OnAddHandCardEvent(BattleCardBase obj)
        {
            if (obj.IsTokenLoad && !obj.IsInDeck)
            {
                var param = obj.BaseParameter;
                Sender.Send($"EnemyAdd:{convertCardId(param)},{param.CardName},{obj.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"EnemyAddHandCard:{obj.BaseParameter.CardName}");
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

        private void Player_OnAddHandCardEvent(BattleCardBase obj)
        {
            if (obj.IsInDeck && _hasPlayerDrawn)
            {
                var param = obj.BaseParameter;
                Sender.Send($"PlayerDraw:{convertCardId(param)},{param.CardName},{param.Cost}{(param.CharType == CharaType.NORMAL ? $",{param.Atk},{param.Life}" : "")}");
            }
#if DEBUG
            else
            {
                Sender.Send($"PlayerAddHand:{obj.BaseParameter.CardName}");
            }
#endif
        }

        private void Player_OnMulliganEnd(IEnumerable<BattleCardBase> arg1, IEnumerable<int> arg2)
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
