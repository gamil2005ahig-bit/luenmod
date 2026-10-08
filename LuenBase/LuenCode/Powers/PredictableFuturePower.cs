using MegaCrit.Sts2.Core.Entities.Powers;

namespace Luen.LuenCode.Powers;

public class PredictableFuturePower : LuenPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}
