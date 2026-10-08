using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Rien.RienCode.Cards;

public class FuriosoCrescendo()
    : RienCard(1, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override void AfterCreated()
    {
        base.AfterCreated();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay cardPlay)
    {
        await DamageCmd.Attack(30m)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState!)
            .Execute(context);

        await CreatureCmd.GainBlock(
            Owner.Creature,
            30m,
            ValueProp.Unpowered,
            cardPlay,
            false
        );
    }
}
