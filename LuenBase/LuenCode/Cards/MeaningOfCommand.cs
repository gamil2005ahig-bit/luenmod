using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class MeaningOfCommand()
    : RienCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await DamageCmd.Attack(20m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);

        await CardPileCmd.Draw(context, 3m, Owner);

        if (Owner.HasUniqueDrawPile())
        {
            await PlayerCmd.GainEnergy(2, Owner);
            await Owner.TriggerUnique(context);
        }
    }
}
