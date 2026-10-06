using BaseLib.Abstracts;
using Luen.LuenCode.Extensions;
using Godot;

namespace Luen.LuenCode.Character;

public class LuenPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Luen.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
