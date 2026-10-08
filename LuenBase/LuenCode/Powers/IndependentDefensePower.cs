using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Powers;

public class IndependentDefensePower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext context,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || Owner.Player is not { } player)
        {
            return;
        }

        if (player.HasUniqueDrawPile())
        {
            Flash();
            await CreatureCmd.GainBlock(
                Owner,
                10m,
                ValueProp.Unpowered,
                null,
                false
            );
        }
    }
}
