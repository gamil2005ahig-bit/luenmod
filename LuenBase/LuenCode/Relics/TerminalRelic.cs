using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Relics;

public class TerminalRelic : LuenRelic
{
    // 0: 피해 15 주기
    // 1: 피해 받지 않기
    // 2: 피해 5 이상 받기
    // 3: 카드 2장 이상 뽑기
    // 4: 턴 종료 시 에너지 1
    // 5: 파워 카드 사용
    private int _currentQuest = -1;

    private decimal _damageGivenThisTurn;
    private decimal _damageReceivedThisTurn;
    private int _extraCardsDrawnThisTurn;
    private bool _powerCardPlayedThisTurn;
    private bool _questCompletedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner)
        {
            return;
        }

        ResetTurnProgress();

        // 매 턴 시작 시 새로운 지령을 무작위로 정한다.
        _currentQuest = Owner.RunState!.Rng.CombatCardSelection.NextInt(6);

        MainFile.Logger.Info(
            $"단말기 새 지령: {_currentQuest}"
        );

        await Task.CompletedTask;
    }

    public override Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer == Owner.Creature &&
            props.IsPoweredAttack() &&
            result.TotalDamage > 0)
        {
            _damageGivenThisTurn += result.TotalDamage;
            CheckQuest();
        }

        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target == Owner.Creature &&
            result.TotalDamage > 0)
        {
            _damageReceivedThisTurn += result.TotalDamage;
            CheckQuest();
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner == Owner &&
            !fromHandDraw)
        {
            _extraCardsDrawnThisTurn++;
            CheckQuest();
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner &&
            cardPlay.Card.Type == CardType.Power)
        {
            _powerCardPlayedThisTurn = true;
            CheckQuest();
        }

        return Task.CompletedTask;
    }

    public override async Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
        {
            return;
        }

        if (_currentQuest == 1 &&
            _damageReceivedThisTurn == 0)
        {
            await CompleteQuest(choiceContext);
            return;
        }

        if (_currentQuest == 4 &&
            Owner.PlayerCombatState.Energy == 1)
        {
            await CompleteQuest(choiceContext);
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ResetTurnProgress();
        _currentQuest = -1;
        return Task.CompletedTask;
    }

    private void ResetTurnProgress()
    {
        _damageGivenThisTurn = 0;
        _damageReceivedThisTurn = 0;
        _extraCardsDrawnThisTurn = 0;
        _powerCardPlayedThisTurn = false;
        _questCompletedThisTurn = false;
    }

    private void CheckQuest()
    {
        if (_questCompletedThisTurn)
        {
            return;
        }

        bool completed = _currentQuest switch
        {
            // 한 턴 동안 피해 15 이상 주기
            0 => _damageGivenThisTurn >= 15,

            // 한 턴 동안 피해를 받지 않기
            // 이 조건은 턴 종료 시 별도 판정이 필요하다.
            1 => false,

            // 피해 5 이상 받기
            2 => _damageReceivedThisTurn >= 5,

            // 시작 드로우를 제외하고 카드 2장 이상 뽑기
            3 => _extraCardsDrawnThisTurn >= 2,

            // 턴 종료 시 에너지 1
            4 => false,

            // 파워 카드 사용
            5 => _powerCardPlayedThisTurn,

            _ => false
        };

        if (completed)
        {
            // 비동기 보상 처리는 별도 작업으로 실행한다.
            _ = CompleteQuest(null);
        }
    }

    private async Task CompleteQuest(PlayerChoiceContext? choiceContext)
    {
        if (_questCompletedThisTurn)
        {
            return;
        }

        _questCompletedThisTurn = true;
        Flash();

        // 지령의 가호 1:
        // 힘 1과 민첩 1을 각각 얻는다.
        if (choiceContext != null)
        {
            await PowerCmd.Apply<StrengthPower>(
                choiceContext,
                Owner.Creature,
                1m,
                Owner.Creature,
                null
            );

            await PowerCmd.Apply<DexterityPower>(
                choiceContext,
                Owner.Creature,
                1m,
                Owner.Creature,
                null
            );
        }

        MainFile.Logger.Info("단말기 지령 완료: 지령의 가호 +1");
    }
}