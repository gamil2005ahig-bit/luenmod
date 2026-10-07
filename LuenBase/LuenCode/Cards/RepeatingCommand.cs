using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Cards;

public class RepeatingCommand()
    : LuenCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;

    public override void AfterCreated()
    {
        base.AfterCreated();
        AddKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        int repetitions = ResolveEnergyXValue();

        if (repetitions > 0)
        {
            await PowerCmd.Apply<CommandProtectionPower>(
                context,
                Owner.Creature,
                repetitions,
                Owner.Creature,
                this
            );
        }
    }
}
