using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class SacrificialDefense()
    : RienCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        if (!await Owner.Creature.TrySpendCommandProtection(context, Owner.Creature, this))
        {
            return;
        }

        await CreatureCmd.GainBlock(
            Owner.Creature,
            13m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );
    }
}
