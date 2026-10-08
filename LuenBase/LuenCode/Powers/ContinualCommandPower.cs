using System.Linq;
using Rien.RienCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Powers;

public class ContinualCommandPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext context, MegaCrit.Sts2.Core.Entities.Players.Player player)
    {
        if (player != Owner.Player) return;
        var terminal = player.Relics.OfType<TerminalRelic>().FirstOrDefault();
        if (terminal != null)
        {
            await terminal.CompleteExtraCommand(context);
        }
    }
}
