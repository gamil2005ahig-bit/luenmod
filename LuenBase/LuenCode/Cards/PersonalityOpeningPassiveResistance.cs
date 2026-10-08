using System.Linq;
using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Cards;

public class PersonalityOpeningPassiveResistance() : RienCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var protection = Owner.Creature.Powers.OfType<CommandProtectionPower>().FirstOrDefault();
        decimal amount = protection?.Amount ?? 0m;
        if (amount > 0)
        {
            await PowerCmd.Apply<AwakeningPower>(context, Owner.Creature, amount, Owner.Creature, this);
            await PowerCmd.Apply<CommandProtectionPower>(context, Owner.Creature, -amount, Owner.Creature, this);
        }
        await PowerCmd.Apply<TerminalSuppressionPower>(context, Owner.Creature, 1m, Owner.Creature, this);
    }
}
