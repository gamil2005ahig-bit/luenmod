using System.Linq;

using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Powers;

public class ResolvePower : LuenPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
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
