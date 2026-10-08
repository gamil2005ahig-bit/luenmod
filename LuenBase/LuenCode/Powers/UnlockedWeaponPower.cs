using System.Linq;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Powers;

public class UnlockedWeaponPower : LuenPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterShuffle(
        PlayerChoiceContext context,
        Player shuffler)
    {
        if (shuffler != Owner.Player)
        {
            return;
        }

        var drawPile = PileType.Draw.GetPile(shuffler);
        var discardPile = PileType.Discard.GetPile(shuffler);
        var cards = drawPile.Cards.Take(5).ToList();

        foreach (var card in cards)
        {
            await CardPileCmd.Add(card, discardPile);
        }

        if (cards.Count > 0)
        {
            Flash();
            discardPile.InvokeContentsChanged();
        }
    }
}
