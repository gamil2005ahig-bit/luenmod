using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class Slash()
    : RienCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        // 대상에게 피해 12를 준다.
        await DamageCmd.Attack(12m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);
    }
}