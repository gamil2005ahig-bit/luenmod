using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class Engage()
    : RienCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await CardPileCmd.Draw(context, 2m, Owner);

        await PowerCmd.Apply<NextTurnDrawPower>(
            context,
            Owner.Creature,
            2m,
            Owner.Creature,
            this
        );
    }
}
