using System.Linq;

using Luen.LuenCode.Extensions;
using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CommandGuidance()
    : LuenCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
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
        var protection = Owner.Creature.Powers
            .OfType<CommandProtectionPower>()
            .FirstOrDefault();
        decimal amount = Math.Min(3m, protection?.Amount ?? 0m);

        if (amount == 0m)
        {
            return;
        }

        await Owner.Creature.TrySpendCommandProtection(
            context,
            Owner.Creature,
            this,
            amount
        );
        await PlayerCmd.GainEnergy(amount, Owner);
        await CardPileCmd.Draw(context, amount, Owner);
        await CreatureCmd.GainBlock(
            Owner.Creature,
            amount,
            ValueProp.Unpowered,
            cardPlay,
            false
        );
    }
}
