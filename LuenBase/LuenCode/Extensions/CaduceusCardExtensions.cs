using System.Linq;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Luen.LuenCode.Extensions;

public static class CaduceusCardExtensions
{
    public static bool IsCaduceusCard(this CardModel card)
    {
        return card.Id.Entry.StartsWith(
            "LUEN-CADUCEUS_",
            StringComparison.Ordinal
        );
    }

    public static int CountCaduceusCardsInDeck(this Player player)
    {
        return player.Deck.Cards.Count(card => card.IsCaduceusCard());
    }

    public static int CountCaduceusCardsPlayedThisCombat(this Player player)
    {
        return CombatManager.Instance.History.CardPlaysFinished.Count(entry =>
            entry.CardPlay.Card.Owner == player &&
            entry.CardPlay.Card.IsCaduceusCard());
    }
}
