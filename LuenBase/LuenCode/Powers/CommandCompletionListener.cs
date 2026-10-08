using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Luen.LuenCode.Powers;

public interface ICommandCompletionListener
{
    Task OnCommandCompleted(PlayerChoiceContext context);
}
