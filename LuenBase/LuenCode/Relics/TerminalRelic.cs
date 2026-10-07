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
    private enum Quest
    {
        DealFifteenDamage,
        TakeNoDamage,
        TakeFiveDamage,
        DrawTwoCards,
        EndWithOneEnergy,
        PlayPowerCard
    }

    private const int QuestCount = 6;

    private Quest? _currentQuest;
    private decimal _damageGivenThisTurn;
    private decimal _damageReceivedThisTurn;
    private int _extraCardsDrawnThisTurn;
    private bool _powerCardPlayedThisTurn;
    private bool _questCompletedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Starter;

    public override Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner)
        {
            return Task.CompletedTask;
        }

        ResetTurnProgress();
        _currentQuest = (Quest)Owner.RunState!.Rng.CombatCardSelection.NextInt(QuestCount);

        MainFile.Logger.Info($"Terminal new command: {_currentQuest}");
        return Task.CompletedTask;
    }

    public override async Task AfterDamageGiven(
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
            await CheckQuest(choiceContext);
        }
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target == Owner.Creature && result.TotalDamage > 0)
        {
            _damageReceivedThisTurn += result.TotalDamage;
            await CheckQuest(choiceContext);
        }
    }

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner == Owner && !fromHandDraw)
        {
            _extraCardsDrawnThisTurn++;
            await CheckQuest(choiceContext);
        }
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Power)
        {
            _powerCardPlayedThisTurn = true;
            await CheckQuest(choiceContext);
        }
    }

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature) || _questCompletedThisTurn)
        {
            return;
        }

        if (_currentQuest == Quest.TakeNoDamage && _damageReceivedThisTurn == 0)
        {
            await CompleteQuest(choiceContext);
        }
        else if (_currentQuest == Quest.EndWithOneEnergy && Owner.PlayerCombatState.Energy == 1)
        {
            await CompleteQuest(choiceContext);
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ResetTurnProgress();
        _currentQuest = null;
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

    private async Task CheckQuest(PlayerChoiceContext choiceContext)
    {
        if (_questCompletedThisTurn)
        {
            return;
        }

        bool completed = _currentQuest switch
        {
            Quest.DealFifteenDamage => _damageGivenThisTurn >= 15,
            Quest.TakeFiveDamage => _damageReceivedThisTurn >= 5,
            Quest.DrawTwoCards => _extraCardsDrawnThisTurn >= 2,
            Quest.PlayPowerCard => _powerCardPlayedThisTurn,
            _ => false
        };

        if (completed)
        {
            await CompleteQuest(choiceContext);
        }
    }

    private async Task CompleteQuest(PlayerChoiceContext choiceContext)
    {
        if (_questCompletedThisTurn)
        {
            return;
        }

        _questCompletedThisTurn = true;
        Flash();

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

        MainFile.Logger.Info("Terminal command completed: gained 1 Strength and 1 Dexterity.");
    }
}
