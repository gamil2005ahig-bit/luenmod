using System.Linq;

using Luen.LuenCode.Extensions;
using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CommandProtection()
    : LuenCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
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
        decimal amount = protection?.Amount ?? 0m;

        if (amount > 0m)
        {
            await Owner.Creature.TrySpendCommandProtection(
                context,
                Owner.Creature,
                this,
                amount
            );
        }

        await CreatureCmd.GainBlock(
            Owner.Creature,
            amount * 9m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );
        await CreatureCmd.Heal(Owner.Creature, 5m);
    }
}
