using System.Collections.Generic;
using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Luen.LuenCode.Cards;

public class PredictableFuture() : LuenCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const string EffectVarName = "CaduceusEffect";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar(EffectVarName, 1m)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var source = ModelDb.Card<CaduceusStrategy>();
        var choices = new List<CardModel>
        {
            source.CreateCloneForPlayer(Owner),
            source.CreateCloneForPlayer(Owner),
            source.CreateCloneForPlayer(Owner)
        };
        for (var index = 0; index < choices.Count; index++)
        {
            choices[index].DynamicVars[EffectVarName].BaseValue = index + 1;
        }

        var selected = await CardSelectCmd.FromChooseACardScreen(context, choices, Owner, false);
        await source.ExecuteEffect(context, cardPlay, (int)selected.DynamicVars[EffectVarName].BaseValue);
    }
}
