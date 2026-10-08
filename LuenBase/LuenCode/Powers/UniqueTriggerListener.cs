using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Powers;

public interface IUniqueTriggerListener
{
    Task OnUniqueTriggered(PlayerChoiceContext context);
}
