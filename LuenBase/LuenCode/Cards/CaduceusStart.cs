using System.Collections.Generic;

using Luen.LuenCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luen.LuenCode.Cards;

public class CaduceusStart()
    : LuenCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    private const string EffectVarName = "CaduceusEffect";

    // 카드 설명에 표시할 현재 효과 번호
    // 1: 피해 6 + 방어도 6
    // 2: 피해 6 + 취약 1
    // 3: 피해 6 + 모든 적에게 피해 4
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar(EffectVarName, 1m)
    ];

    // 새 카드가 생성될 때 이벤트를 연결한다.
    public override void AfterCreated()
    {
        base.AfterCreated();

        Drawn += SelectEffect;
    }

    // 카드가 복제된 뒤에는 기본 클래스가 Drawn 이벤트를 초기화하므로,
    // 복제 후 다시 이벤트를 연결해야 한다.
    protected override void AfterCloned()
    {
        base.AfterCloned();

        Drawn += SelectEffect;
    }

    // 카드를 뽑을 때마다 효과를 새로 결정한다.
    private void SelectEffect()
    {
        if (RunState == null)
        {
            return;
        }

        int effect = RunState.Rng.CombatCardSelection.NextInt(3) + 1;

        DynamicVars[EffectVarName].BaseValue = effect;

        MainFile.Logger.Info(
            $"CaduceusStart 효과 변경: {effect}"
        );
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        MainFile.Logger.Info($"CaduceusStart 실제 ID: {Id.Entry}");
        // 설명에 저장된 효과 번호와 실제 효과를 동일하게 사용한다.
        int effect =
            (int)DynamicVars[EffectVarName].BaseValue;

        switch (effect)
        {
            case 1:
                // 피해 6 + 방어도 6
                await DamageCmd.Attack(6m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);

                await CreatureCmd.GainBlock(
                    Owner.Creature,
                    6m,
                    ValueProp.Unpowered,
                    cardPlay,
                    false
                );
                break;

            case 2:
                // 피해 6 + 취약 1
                await DamageCmd.Attack(6m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);

                await PowerCmd.Apply<VulnerablePower>(
                    context,
                    cardPlay.Target!,
                    1m,
                    Owner.Creature,
                    this
                );
                break;

            case 3:
                // 피해 6 + 모든 적에게 피해 4
                await DamageCmd.Attack(6m)
                    .FromCard(this, cardPlay)
                    .Targeting(cardPlay.Target!)
                    .Execute(context);

                await DamageCmd.Attack(4m)
                    .FromCard(this, cardPlay)
                    .TargetingAllOpponents(CombatState!)
                    .Execute(context);
                break;
        }
    }
}