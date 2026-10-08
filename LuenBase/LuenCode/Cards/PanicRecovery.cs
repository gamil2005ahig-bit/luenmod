using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Rien.RienCode.Cards;

public class PanicRecovery()
    : RienCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override void AfterCreated()
    {
        base.AfterCreated();
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

        await PowerCmd.Apply<FrailPower>(
            context,
            Owner.Creature,
            -4m,
            Owner.Creature,
            this
        );
    }
}
