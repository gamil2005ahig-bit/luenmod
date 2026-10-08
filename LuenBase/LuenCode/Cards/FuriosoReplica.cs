using System.Linq;

using Rien.RienCode.Extensions;
using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Rien.RienCode.Cards;

public class FuriosoReplica()
    : RienCard(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var target = cardPlay.Target!;

        await DamageCmd.Attack(30m)
            .FromCard(this, cardPlay)
            .Targeting(target)
            .Execute(context);

        await PowerCmd.Apply<VulnerablePower>(
            context,
            target,
            3m,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<TemporaryStrengthReductionPower>(
            context,
            target,
            5m,
            Owner.Creature,
            this
        );

        if (Owner.CountCaduceusCardsInDeck() >= 5)
        {
            var crescendo = ModelDb.Card<FuriosoCrescendo>();
            await CardPileCmd.Add(crescendo, PileType.Hand.GetPile(Owner));
        }
    }
}
