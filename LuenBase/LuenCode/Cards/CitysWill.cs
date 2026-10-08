using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class CitysWill()
    : RienCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await DamageCmd.Attack(8m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);

        await CreatureCmd.GainBlock(
            Owner.Creature,
            8m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );

        await CardPileCmd.Draw(context, 1m, Owner);

        if (Owner.HasUniqueDrawPile())
        {
            await PlayerCmd.GainEnergy(1, Owner);
            await Owner.TriggerUnique(context);
        }
    }
}
