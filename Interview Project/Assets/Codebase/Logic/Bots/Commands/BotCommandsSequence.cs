using System.Collections.Generic;

namespace Codebase.Logic.Bots.Commands
{
  public class BotCommandsSequence
  {
    private readonly List<ICommand> _commandsList;
    
    private int _currentCommandIndex;
    private bool _isCurrentlyExecutingACommand;


    public BotCommandsSequence() => 
      _commandsList = new List<ICommand>();

    
    public void ClearCommands()
    {
      StopExecutingCurrentCommand();
      
      _commandsList.Clear();
      _currentCommandIndex = 0;
    }

    public void AddCommand(ICommand newCommand)
    {
      newCommand.OnExecutionComplete += ResolveOnCommandExecuted;
      _commandsList.Add(newCommand);
    }

    public void StartCommandsExecution()
    {
      if (_isCurrentlyExecutingACommand || _commandsList.Count == 0)
        return;
      
      _isCurrentlyExecutingACommand = true;
      
      _commandsList[_currentCommandIndex].Execute();
    }

    public void StopExecutingCurrentCommand()
    {
      if (!_isCurrentlyExecutingACommand)
        return;
      
      _commandsList[_currentCommandIndex].StopExecution();
      _isCurrentlyExecutingACommand = false;
    }

    private void ResolveOnCommandExecuted()
    {
      if (_commandsList.Count == 1)
      {
        ClearCommands();
        return;
      }
      
      _currentCommandIndex++;
      
      if (_currentCommandIndex > _commandsList.Count - 1) 
        _currentCommandIndex = 0;
      
      _commandsList[_currentCommandIndex].Execute();
    }
  }
}