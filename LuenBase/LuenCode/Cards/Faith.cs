using System.Linq;

using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class Faith()
    : RienCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
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
        int amount = ResolveEnergyXValue();
        if (amount <= 0)
        {
            return;
        }

        await PowerCmd.Apply<FaithCostReductionPower>(
            context,
            Owner.Creature,
            amount,
            Owner.Creature,
            this
        );
        await CardPileCmd.Draw(context, amount, Owner);
    }
}
