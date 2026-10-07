using Luen.LuenCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class MultiSlash()
    : LuenCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        // 대상에게 피해 7을 주고 카드를 한 장 뽑는다.
        await DamageCmd.Attack(7m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);

        await CardPileCmd.Draw(context, 1m, Owner);

        // 카드를 뽑은 뒤의 뽑을 카드 더미를 기준으로 유일을 판정한다.
        if (Owner.HasUniqueDrawPile())
        {
            await PlayerCmd.GainEnergy(1, Owner);
        }
    }
}
