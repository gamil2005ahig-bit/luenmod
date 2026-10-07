using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class Coercion()
    : LuenCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var target = cardPlay.Target!;
        bool hadBlock = target.Block > 0;

        await DamageCmd.Attack(8m)
            .FromCard(this, cardPlay)
            .Targeting(target)
            .Execute(context);

        if (hadBlock)
        {
            await DamageCmd.Attack(8m)
                .FromCard(this, cardPlay)
                .Targeting(target)
                .Execute(context);
        }
    }
}
