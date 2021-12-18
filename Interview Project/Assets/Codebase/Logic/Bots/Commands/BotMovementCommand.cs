using System;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public class BotMovementCommand : ICommand
  {
    private readonly BotNavMeshLogic _bot;
    private readonly Vector3 _targetPosition;
    
    public event Action OnExecutionComplete;


    public BotMovementCommand(BotNavMeshLogic bot, Vector3 targetPosition)
    {
      _bot = bot;
      _targetPosition = targetPosition;
    }
    

    public void Execute()
    {
      _bot.SetDestination(_targetPosition);
      _bot.StartMoving();
      _bot.OnDestinationReached += ResolveOnDestinationReached;
    }

    public void StopExecution()
    {
      _bot.StopMoving();
      _bot.OnDestinationReached -= ResolveOnDestinationReached;
    }

    private void ResolveOnDestinationReached()
    {
      StopExecution();
      OnExecutionComplete?.Invoke();
    }
  }
}