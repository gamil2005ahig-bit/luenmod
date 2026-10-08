using System.Linq;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Rien.RienCode.Extensions;

public static class CardUniquenessExtensions
{
    /// <summary>
    /// Returns whether every non-Status card currently in this player's draw pile
    /// has a distinct card model ID. Upgraded copies still share their base ID.
    /// </summary>
    public static bool HasUniqueDrawPile(this Player player)
    {
        var nonStatusCards = PileType.Draw.GetPile(player)
            .Cards
            .Where(card => card.Type != CardType.Status)
            .ToList();

        return nonStatusCards
            .Select(card => card.Id)
            .Distinct()
            .Count() == nonStatusCards.Count;
    }
}
