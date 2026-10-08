using Luen.LuenCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class UniqueStrike()
    : LuenCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        bool isUnique = Owner.HasUniqueDrawPile();

        await DamageCmd.Attack(10m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);

        if (isUnique)
        {
            await CardPileCmd.Draw(context, 2m, Owner);
            await Owner.TriggerUnique(context);
        }
    }
}
