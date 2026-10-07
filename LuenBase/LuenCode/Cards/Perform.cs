using System.Linq;

using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class Perform()
    : LuenCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var protection = Owner.Creature.Powers
            .OfType<CommandProtectionPower>()
            .FirstOrDefault();

        decimal block = (protection?.Amount ?? 0m) + 3m;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Unpowered,
            cardPlay,
            false
        );
    }
}
