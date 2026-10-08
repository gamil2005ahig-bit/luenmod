using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Rien.RienCode.Powers;

public class AwakeningPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Side) return;
        Flash();
        var context = new ThrowingPlayerChoiceContext();
        await PlayerCmd.GainEnergy(2, Owner.Player!);
        await CardPileCmd.Draw(context, 2m, Owner.Player!);
        await PowerCmd.Apply<StrengthPower>(context, Owner, 2m, Owner, null);
        await CreatureCmd.GainBlock(Owner, 10m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered, null, false);
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext context, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || !participants.Contains(Owner)) return;
        await PowerCmd.Apply<AwakeningPower>(context, Owner, -1m, Owner, null);
    }
}
