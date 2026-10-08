using System.Collections.Generic;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Rien.RienCode.Powers;

public class FaithCostReductionPower : RienPower
{
    private readonly HashSet<CardModel> _cards = [];

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost)
    {
        if (!_cards.Contains(card))
        {
            modifiedCost = originalCost;
            return false;
        }

        modifiedCost = Math.Max(0m, originalCost - Amount);
        return modifiedCost != originalCost;
    }

    public override Task AfterCardDrawn(
        PlayerChoiceContext context,
        CardModel card,
        bool fromHandDraw)
    {
        if (card.Owner == Owner.Player && fromHandDraw)
        {
            _cards.Add(card);
        }

        return Task.CompletedTask;
    }
}
