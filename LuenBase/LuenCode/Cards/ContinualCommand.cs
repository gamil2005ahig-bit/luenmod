using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class ContinualCommand() : RienCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ContinualCommandPower>(context, Owner.Creature, 1m, Owner.Creature, this);
    }
}
