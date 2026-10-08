using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Rien.RienCode.Powers;

public class CommandProtectionPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    public override async Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        await ApplyStatDelta(
            new ThrowingPlayerChoiceContext(),
            target,
            amount,
            applier,
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
        // The initial application is handled by BeforeApplied. For later
        // changes, mirror only the signed change in the Protection amount.
        if (power != this || amount == Amount)
        {
            return;
        }

        await ApplyStatDelta(context, Owner, amount, applier, cardSource);
    }

    private static async Task ApplyStatDelta(
        PlayerChoiceContext context,
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount == 0)
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(
            context,
            target,
            amount,
            applier ?? target,
            cardSource
        );

        await PowerCmd.Apply<DexterityPower>(
            context,
            target,
            amount,
            applier ?? target,
            cardSource
        );
    }
}
