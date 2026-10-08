using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Powers;

public interface IUniqueTriggerListener
{
    Task OnUniqueTriggered(PlayerChoiceContext context);
}
