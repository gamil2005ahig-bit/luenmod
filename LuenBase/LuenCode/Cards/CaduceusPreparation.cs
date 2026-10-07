using System.Collections.Generic;

using Luen.LuenCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CaduceusPreparation()
    : LuenCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
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

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        int effect = (int)DynamicVars[EffectVarName].BaseValue;

        switch (effect)
        {
            case 1:
                await DamageCmd.Attack(5m)
                    .FromCard(this, cardPlay)
                    .Targeting(Owner.Creature)
                    .Execute(context);

                await CreatureCmd.Heal(Owner.Creature, 3m);

                await DamageCmd.Attack(16m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);
                break;

            case 2:
                await DamageCmd.Attack(14m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);
                break;

            case 3:
                await DamageCmd.Attack(3m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);

                await PowerCmd.Apply<CommandProtectionPower>(
                    context,
                    Owner.Creature,
                    1m,
                    Owner.Creature,
                    this
                );
                break;
        }
    }
}
