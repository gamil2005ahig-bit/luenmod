using System.Linq;

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;

namespace Rien.RienCode.Cards;

public class Redeploy()
    : RienCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var drawPile = PileType.Draw.GetPile(Owner);
        var cards = drawPile.Cards.ToList();

        if (cards.Count == 0 || Owner.Creature.CombatState == null)
        {
            return;
        }

        var prefs = new CardSelectorPrefs(
            CardSelectorPrefs.DiscardSelectionPrompt,
            0,
            Math.Min(2, cards.Count)
        );

        var selected = (await CardSelectCmd.FromSimpleGrid(
            context,
            cards,
            Owner,
            prefs
        )).ToList();

        var discardPile = PileType.Discard.GetPile(Owner);
        var combatState = Owner.Creature.CombatState;

        foreach (var card in selected)
        {
            await CardPileCmd.Add(card, discardPile);
            CombatManager.Instance.History.CardDiscarded(combatState, card);
            await Hook.AfterCardDiscarded(combatState, context, card);
        }

        discardPile.InvokeContentsChanged();
    }
}
