using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class Debt()
    : RienCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        if (!await Owner.Creature.TrySpendCommandProtection(context, Owner.Creature, this))
        {
            return;
        }

        await DamageCmd.Attack(15m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);
    }
}
