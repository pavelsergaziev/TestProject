using Codebase.Services.Input;

namespace Codebase.Infrastructure.States
{
  public class MainGameState : IGameState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IInputService _inputService;
    
    public MainGameState(GameStateMachine gameStateMachine, IInputService inputService)
    {
      _gameStateMachine = gameStateMachine;
      _inputService = inputService;
    }
    
    public void Enter() => 
      _inputService.UnlockInput();
  }
}