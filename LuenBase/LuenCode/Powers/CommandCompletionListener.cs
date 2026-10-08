using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Rien.RienCode.Powers;

public interface ICommandCompletionListener
{
    Task OnCommandCompleted(PlayerChoiceContext context);
}
