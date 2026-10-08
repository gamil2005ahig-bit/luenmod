using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace Rien.RienCode.Cards;

public class ReadyStance()
    : RienCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private static readonly LocString SelectionPrompt =
        new("card_selection", "RIEN-READY_STANCE_SELECT");

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        var selected = (await CommonActions.SelectCards(
            this,
            SelectionPrompt,
            context,
            PileType.Draw,
            0,
            3
        )).ToList();

        var exhaustPile = PileType.Exhaust.GetPile(Owner);

        foreach (var card in selected)
        {
            await CardPileCmd.Add(card, exhaustPile);
        }

        exhaustPile.InvokeContentsChanged();
        await CardPileCmd.Draw(context, 1m, Owner);
    }
}
