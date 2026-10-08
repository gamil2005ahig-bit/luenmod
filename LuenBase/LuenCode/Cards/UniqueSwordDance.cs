using Luen.LuenCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class UniqueSwordDance()
    : LuenCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        bool isUnique = Owner.HasUniqueDrawPile();

        await DamageCmd.Attack(13m)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState!)
            .Execute(context);

        if (isUnique)
        {
            await DamageCmd.Attack(13m)
                .FromCard(this, cardPlay)
                .TargetingAllOpponents(CombatState!)
                .Execute(context);

            await Owner.TriggerUnique(context);
        }
    }
}
