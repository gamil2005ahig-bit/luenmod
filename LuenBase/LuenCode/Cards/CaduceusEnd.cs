using System.Collections.Generic;
using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Rien.RienCode.Cards;

public class CaduceusEnd() : RienCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    private const string EffectVarName = "CaduceusEffect";
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(EffectVarName, 1m)];

    private CardModel CreateChoiceCard(int effect)
    {
        var choice = CreateCloneForPlayer(Owner);
        choice.DynamicVars[EffectVarName].BaseValue = effect;
        return choice;
    }

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var choices = new List<CardModel>
        {
            CreateChoiceCard(1),
            CreateChoiceCard(2),
            CreateChoiceCard(3)
        };
        var selected = await CardSelectCmd.FromChooseACardScreen(context, choices, Owner, false);
        DynamicVars[EffectVarName].BaseValue = selected.DynamicVars[EffectVarName].BaseValue;
        await PowerCmd.Apply<CaduceusEndPower>(context, Owner.Creature, 1m, Owner.Creature, this);
    }
}
