using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Rien.RienCode.Powers;

public class TemporaryStrengthReductionPower : RienPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        await PowerCmd.Apply<StrengthPower>(
            new ThrowingPlayerChoiceContext(),
            target,
            -amount,
            applier ?? target,
            cardSource
        );
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext context,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power != this || amount == Amount)
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(
            context,
            Owner,
            -amount,
            applier ?? Owner,
            cardSource
        );
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext context,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        // This effect lasts through the player's turn and expires when the
        // other side's turn ends. The owner is the enemy being weakened.
        if (side == Owner.Side)
        {
            return;
        }

        Flash();
        await PowerCmd.Apply<StrengthPower>(
            context,
            Owner,
            Amount,
            Owner,
            null
        );
        await PowerCmd.Remove(this);
    }
}
