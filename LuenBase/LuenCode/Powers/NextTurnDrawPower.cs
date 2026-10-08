using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Powers;

public class NextTurnDrawPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        Flash();
        await CardPileCmd.Draw(
            new ThrowingPlayerChoiceContext(),
            Amount,
            Owner.Player!
        );
        await PowerCmd.Remove(this);
    }
}
