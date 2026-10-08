using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class TemporaryCommand()
    : RienCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await PowerCmd.Apply<TemporaryCommandProtectionPower>(
            context,
            Owner.Creature,
            1m,
            Owner.Creature,
            this
        );
    }
}
