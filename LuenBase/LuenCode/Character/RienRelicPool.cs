using BaseLib.Abstracts;
using Rien.RienCode.Extensions;
using Godot;

namespace Rien.RienCode.Character;

public class RienRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Rien.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
