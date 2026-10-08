using Rien.RienCode.Extensions;
using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class UniqueDefense()
    : RienCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        bool isUnique = Owner.HasUniqueDrawPile();

        await CreatureCmd.GainBlock(
            Owner.Creature,
            8m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );

        if (isUnique)
        {
            await PowerCmd.Apply<TemporaryStrengthReductionPower>(
                context,
                cardPlay.Target!,
                3m,
                Owner.Creature,
                this
            );

            await Owner.TriggerUnique(context);
        }
    }
}
