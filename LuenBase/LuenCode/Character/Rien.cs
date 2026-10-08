using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using Rien.RienCode.Cards;
using Rien.RienCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using Rien.RienCode.Relics;

namespace Rien.RienCode.Character;

public class Rien : PlaceholderCharacterModel
{
    public const string CharacterId = "Rien";
    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),

        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),

        ModelDb.Card<CaduceusStart>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<TerminalRelic>()
    ];

    public override CardPoolModel CardPool =>
        ModelDb.CardPool<RienCardPool>();

    public override RelicPoolModel RelicPool =>
        ModelDb.RelicPool<RienRelicPool>();

    public override PotionPoolModel PotionPool =>
        ModelDb.PotionPool<RienPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(
                CustomIconTexturePath
            );

            icon.SetAnchorsAndOffsetsPreset(
                Control.LayoutPreset.FullRect
            );

            return icon;
        }
    }

    public override string CustomIconTexturePath =>
        "character_icon_char_name.png".CharacterUiPath();

    public override string CustomCharacterSelectIconPath =>
        "char_select_char_name.png".CharacterUiPath();

    public override string CustomCharacterSelectLockedIconPath =>
        "char_select_char_name_locked.png".CharacterUiPath();

    public override string CustomMapMarkerPath =>
        "map_marker_char_name.png".CharacterUiPath();
}