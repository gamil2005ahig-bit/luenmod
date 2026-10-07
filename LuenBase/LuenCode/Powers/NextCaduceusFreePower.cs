using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

using Luen.LuenCode.Extensions;

namespace Luen.LuenCode.Powers;

public class NextCaduceusFreePower : LuenPower
{
    private CardModel? _sourceCard;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost)
    {
        if (card.Owner?.Creature != Owner || !card.IsCaduceusCard())
        {
            modifiedCost = originalCost;
            return false;
        }

        modifiedCost = 0m;
        return true;
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        if (cardPlay.Card == _sourceCard)
        {
            return;
        }

        if (cardPlay.Card.Owner != Owner.Player || !cardPlay.Card.IsCaduceusCard())
        {
            return;
        }

        Flash();
        await PowerCmd.Remove(this);
    }

    public override Task BeforeApplied(
        Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        _sourceCard = cardSource;
        return Task.CompletedTask;
    }
}
