using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luen.LuenCode.Cards;

public class Stab()
    : LuenCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var target = cardPlay.Target!;

        await DamageCmd.Attack(15m)
            .FromCard(this, cardPlay)
            .Targeting(target)
            .Execute(context);

        await PowerCmd.Apply<StrengthPower>(
            context,
            target,
            -2m,
            Owner.Creature,
            this
        );

        await PowerCmd.Apply<WeakPower>(
            context,
            target,
            1m,
            Owner.Creature,
            this
        );
    }
}
