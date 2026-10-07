using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luen.LuenCode.Cards;

public class PanicRecovery()
    : LuenCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override void AfterCreated()
    {
        base.AfterCreated();
        AddKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await PowerCmd.Apply<VulnerablePower>(
            context,
            Owner.Creature,
            -4m,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<WeakPower>(
            context,
            Owner.Creature,
            -4m,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<WoundPower>(
            context,
            Owner.Creature,
            -4m,
            Owner.Creature,
            this
        );
    }
}
