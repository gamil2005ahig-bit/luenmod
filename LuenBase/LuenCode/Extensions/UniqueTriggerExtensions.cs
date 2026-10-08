using System.Linq;

using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Extensions;

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
