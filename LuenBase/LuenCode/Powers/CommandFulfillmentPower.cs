using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Powers;

public class CommandFulfillmentPower : LuenPower, ICommandCompletionListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnCommandCompleted(PlayerChoiceContext context)
    {
        Flash();
        await PowerCmd.Apply<NextTurnEnergyPower>(
            context,
            Owner,
            1m,
            Owner,
            null
        );
    }
}
