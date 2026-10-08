using System.Linq;

using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Extensions;

public static class UniqueTriggerExtensions
{
    public static async Task TriggerUnique(
        this Player player,
        PlayerChoiceContext context)
    {
        foreach (var listener in player.Creature.Powers
                     .OfType<IUniqueTriggerListener>()
                     .ToList())
        {
            await listener.OnUniqueTriggered(context);
        }
    }
}
