using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luen.LuenCode.Cards;

public class CommandTrajectory : LuenCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public CommandTrajectory()
        : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    public override void AfterCreated()
    {
        base.AfterCreated();

    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var target = cardPlay.Target!;

        // 대상에게 피해 10을 준다.
        await DamageCmd.Attack(10m)
            .FromCard(this, cardPlay)
            .Targeting(target)
            .Execute(context);

        // 대상의 힘 파워를 제거한다.
        await PowerCmd.Remove<StrengthPower>(target);
    }
}