using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Rien.RienCode.Character;
using Rien.RienCode.Extensions;

namespace Rien.RienCode.Potions;

[Pool(typeof(RienPotionPool))]
public abstract class RienPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}
