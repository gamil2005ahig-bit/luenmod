using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class Engage()
    : LuenCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
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
