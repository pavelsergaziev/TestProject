using System;

namespace Codebase.Logic.Bots.Commands
{
  public interface ICommand
  {
    event Action OnExecutionComplete;
    void Execute();
    void StopExecution();
  }
}