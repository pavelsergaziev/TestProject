using System;
using System.Collections.Generic;

namespace Codebase.Logic.Bots.Commands
{
  public class MacroCommand : ICommand
  {
    private List<ICommand> _commandsList;
    private int _currentCommandIndex;
    
    public event Action OnExecutionComplete;


    public void Execute()
    {
      _currentCommandIndex = 0;
      _commandsList[0].Execute();
    }

    public void StopExecution()
    {
      _commandsList[_currentCommandIndex].StopExecution();
      _currentCommandIndex = 0;
    }

    public MacroCommand AddCommand(ICommand command)
    {
      if(_commandsList == null)
        _commandsList = new List<ICommand>();
      
      _commandsList.Add(command);
      command.OnExecutionComplete += ResolveOnCurrentCommandExecutionComplete;

      return this;
    }

    private void ResolveOnCurrentCommandExecutionComplete()
    {
      if (_currentCommandIndex < _commandsList.Count - 1) 
        _commandsList[++_currentCommandIndex].Execute();
      else
      {
        _currentCommandIndex = 0;
        OnExecutionComplete?.Invoke();
      }
    }
  }
}