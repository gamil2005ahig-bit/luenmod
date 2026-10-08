using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Powers;

public class TorrentPower : LuenPower, IUniqueTriggerListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnUniqueTriggered(PlayerChoiceContext context)
    {
        Flash();
        await CardPileCmd.Draw(context, 1m, Owner.Player!);
    }
}
