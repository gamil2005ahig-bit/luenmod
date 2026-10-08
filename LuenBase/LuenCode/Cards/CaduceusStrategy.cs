using System.Collections.Generic;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CaduceusStrategy()
    : LuenCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Innate,
        CardKeyword.Retain
    ];

    private const string EffectVarName = "CaduceusEffect";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar(EffectVarName, 1m)
    ];

    public override void AfterCreated()
    {
        base.AfterCreated();
        Drawn += SelectEffect;
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        Drawn += SelectEffect;
    }

    private void SelectEffect()
    {
        if (RunState == null)
        {
            return;
        }

        DynamicVars[EffectVarName].BaseValue =
            RunState.Rng.CombatCardSelection.NextInt(3) + 1;
    }

    private CardModel CreateChoiceCard(int effect)
    {
        CardModel choice = CreateCloneForPlayer(Owner);
        choice.DynamicVars[EffectVarName].BaseValue = effect;
        return choice;
    }

    public async Task ExecuteEffect(
        PlayerChoiceContext context,
        CardPlay cardPlay,
        int effect)
    {
        switch (effect)
        {
            case 1:
                await DamageCmd.Attack(14m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);
                break;
            case 2:
                await DamageCmd.Attack(1m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);
                await CreatureCmd.GainBlock(Owner.Creature, 12m, ValueProp.Unpowered, cardPlay, false);
                break;
            case 3:
                await DamageCmd.Attack(1m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);
                await PlayerCmd.GainEnergy(1, Owner);
                await CardPileCmd.Draw(context, 1m, Owner);
                break;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState?.TurnNumber == 1)
        {
            IReadOnlyList<CardModel> choices =
            [
                CreateChoiceCard(1),
                CreateChoiceCard(2),
                CreateChoiceCard(3)
            ];

            CardModel selected = await CardSelectCmd.FromChooseACardScreen(
                context,
                choices,
                Owner,
                false
            );

            DynamicVars[EffectVarName].BaseValue =
                selected.DynamicVars[EffectVarName].BaseValue;
        }

        await ExecuteEffect(context, cardPlay, (int)DynamicVars[EffectVarName].BaseValue);
    }
}
