using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Luen.LuenCode.Character;
using Luen.LuenCode.Extensions;

namespace Luen.LuenCode.Potions;

[Pool(typeof(LuenPotionPool))]
public abstract class LuenPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}
