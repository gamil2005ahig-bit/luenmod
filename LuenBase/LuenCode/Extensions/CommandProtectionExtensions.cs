using System.Linq;

using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Rien.RienCode.Extensions;

public static class CommandProtectionExtensions
{
    public static async Task<bool> TrySpendCommandProtection(
        this Creature creature,
        PlayerChoiceContext context,
        Creature applier,
        CardModel? cardSource,
        decimal amount = 1m)
    {
        var protection = creature.Powers
            .OfType<CommandProtectionPower>()
            .FirstOrDefault();

        if (protection == null || protection.Amount < amount)
        {
            return false;
        }

        await PowerCmd.Apply<CommandProtectionPower>(
            context,
            creature,
            -amount,
            applier,
            cardSource
        );

        return true;
    }
}
