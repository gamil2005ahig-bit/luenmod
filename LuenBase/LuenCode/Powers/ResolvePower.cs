using System.Linq;

using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Powers;

public class ResolvePower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer != Owner)
        {
            return 1m;
        }

        var protection = Owner.Powers
            .OfType<CommandProtectionPower>()
            .FirstOrDefault();
        var protectionAmount = protection?.Amount ?? 0m;

        return 1m + protectionAmount * 0.10m;
    }
}
