using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class CaduceusPlan()
    : LuenCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await DamageCmd.Attack(15m)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(context);

        await PowerCmd.Apply<NextCaduceusFreePower>(
            context,
            Owner.Creature,
            1m,
            Owner.Creature,
            this
        );
    }
}
