using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Rien.RienCode.Powers;

public class CaduceusEndPower : RienPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        switch ((int)Amount)
        {
            case 1:
                await CreatureCmd.Heal(Owner, 10m);
                break;
            case 2:
                await PotionCmd.TryToProcure<EnergyPotion>(Owner.Player!);
                await PotionCmd.TryToProcure<EnergyPotion>(Owner.Player!);
                break;
            case 3:
                var cards = Owner.Player!.Deck.Cards
                    .Where(card => card.MaxUpgradeLevel > card.CurrentUpgradeLevel)
                    .ToList();
                if (cards.Count > 0)
                {
                    var card = cards[Owner.Player.RunState!.Rng.CombatCardSelection.NextInt(cards.Count)];
                    CardCmd.Upgrade(card, CardPreviewStyle.None);
                }
                break;
        }
    }
}
