using Luen.LuenCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class Finisher()
    : LuenCard(4, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost)
    {
        if (card != this)
        {
            modifiedCost = originalCost;
            return false;
        }

        int caduceusCardsPlayed = Owner.CountCaduceusCardsPlayedThisCombat();
        modifiedCost = Math.Max(0m, originalCost - caduceusCardsPlayed);
        return caduceusCardsPlayed > 0;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await DamageCmd.Attack(20m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);
    }
}
