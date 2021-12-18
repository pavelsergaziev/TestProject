using Codebase.Infrastructure.States;
using Codebase.Services;
using Codebase.Services.CoroutineRunner;
using Codebase.Services.Updater;
using UnityEngine;

namespace Codebase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunningService
  {
    private GameStateMachine _gameStateMachine;

    private void Awake()
    {
      DontDestroyOnLoad(gameObject);

      _gameStateMachine = new GameStateMachine(this, gameObject.AddComponent<UpdateService>(), new ServicesContainer());
      _gameStateMachine.Enter<BootstrapGameState>();
    }
  }
}