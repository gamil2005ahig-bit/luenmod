using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Powers;

public class CommandGuardPower : RienPower, ICommandCompletionListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnCommandCompleted(PlayerChoiceContext context)
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
