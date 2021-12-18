namespace Codebase.Infrastructure.States
{
  public interface IGameState
  {
    void Enter();
  }

  public interface IGameStateWithExitMethod : IGameState
  {
    void Exit();
  }
}