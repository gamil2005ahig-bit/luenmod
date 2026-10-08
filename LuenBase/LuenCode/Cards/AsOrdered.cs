using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class AsOrdered()
    : RienCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        // 피해 2와 방어도 1을 세 번 반복한다.
        for (int i = 0; i < 3; i++)
        {
            await DamageCmd.Attack(2m)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target!)
                .Execute(context);

            await CreatureCmd.GainBlock(
                Owner.Creature,
                1m,
                ValueProp.Unpowered,
                cardPlay,
                false
            );
        }
    }
}
