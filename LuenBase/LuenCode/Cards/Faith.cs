using System.Linq;

using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class Faith()
    : LuenCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
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
