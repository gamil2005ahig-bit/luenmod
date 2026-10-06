using System.Collections.Generic;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CaduceusVersatile()
    : LuenCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    private const string EffectVarName = "CaduceusEffect";

    // true로 설정하면 카드가 X 비용으로 처리된다.
    protected override bool HasEnergyCostX => true;

    // 카드 설명에 현재 선택된 효과 번호를 표시한다.
    // 1: 피해 1 + 방어도 9
    // 2: 피해 5 + 방어도 5
    // 3: 피해 9 + 방어도 1
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

        // 카드를 뽑을 때 효과를 무작위로 결정하고 저장한다.
        DynamicVars[EffectVarName].BaseValue =
            RunState.Rng.CombatCardSelection.NextInt(3) + 1;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        int effect = (int)DynamicVars[EffectVarName].BaseValue;

        // 지불할 X 에너지 값을 반복 횟수로 사용한다.
        int repetitions = ResolveEnergyXValue();

        for (int i = 0; i < repetitions; i++)
        {
            switch (effect)
            {
                case 1:
                    await DamageCmd.Attack(1m)
                        .FromCard(this, cardPlay)
                        .Targeting(cardPlay.Target!)
                        .Execute(context);

                    await CreatureCmd.GainBlock(
                        Owner.Creature,
                        9m,
                        ValueProp.Unpowered,
                        cardPlay,
                        false
                    );
                    break;

                case 2:
                    await DamageCmd.Attack(5m)
                        .FromCard(this, cardPlay)
                        .Targeting(cardPlay.Target!)
                        .Execute(context);

                    await CreatureCmd.GainBlock(
                        Owner.Creature,
                        5m,
                        ValueProp.Unpowered,
                        cardPlay,
                        false
                    );
                    break;

                case 3:
                    await DamageCmd.Attack(9m)
                        .FromCard(this, cardPlay)
                        .Targeting(cardPlay.Target!)
                        .Execute(context);

                    await CreatureCmd.GainBlock(
                        Owner.Creature,
                        1m,
                        ValueProp.Unpowered,
                        cardPlay,
                        false
                    );
                    break;
            }
        }
    }
}