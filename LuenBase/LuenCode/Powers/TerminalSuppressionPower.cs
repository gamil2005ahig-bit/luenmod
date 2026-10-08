using MegaCrit.Sts2.Core.Entities.Powers;

namespace Rien.RienCode.Powers;

public class TerminalSuppressionPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}
