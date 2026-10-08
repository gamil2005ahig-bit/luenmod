using System.Collections.Generic;

using Rien.RienCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class CaduceusLink()
    : RienCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
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
                await CardPileCmd.Draw(context, 4m, Owner);
                break;

            case 2:
                foreach (var enemy in CombatState!.HittableEnemies)
                {
                    await PowerCmd.Apply<VulnerablePower>(
                        context,
                        enemy,
                        2m,
                        Owner.Creature,
                        this
                    );

                    await PowerCmd.Apply<WeakPower>(
                        context,
                        enemy,
                        2m,
                        Owner.Creature,
                        this
                    );
                }
                break;

            case 3:
                await CreatureCmd.GainBlock(
                    Owner.Creature,
                    3m,
                    ValueProp.Unpowered,
                    cardPlay,
                    false
                );

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
