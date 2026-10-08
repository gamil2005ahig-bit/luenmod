using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class DoubleSlash()
    : LuenCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override void AfterCreated()
    {
        base.AfterCreated();

    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        // 대상에게 피해 4를 두 번 준다.
        for (int i = 0; i < 2; i++)
        {
            await DamageCmd.Attack(4m)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target!)
                .Execute(context);
        }
    }
}