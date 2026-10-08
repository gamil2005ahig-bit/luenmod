using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class RepeatingCommand()
    : RienCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override bool HasEnergyCostX => true;

    public override void AfterCreated()
    {
        base.AfterCreated();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        int repetitions = ResolveEnergyXValue();

        if (repetitions > 0)
        {
            await PowerCmd.Apply<CommandProtectionPower>(
                context,
                Owner.Creature,
                repetitions,
                Owner.Creature,
                this
            );
        }
    }
}
