using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class Prepare()
    : RienCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(
            Owner.Creature,
            2m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );

        await PowerCmd.Apply<NextTurnEnergyPower>(
            context,
            Owner.Creature,
            2m,
            Owner.Creature,
            this
        );
    }
}
