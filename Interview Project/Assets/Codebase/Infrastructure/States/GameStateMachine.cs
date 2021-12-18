using System;
using System.Collections.Generic;
using Codebase.Services;
using Codebase.Services.Cameras;
using Codebase.Services.CoroutineRunner;
using Codebase.Services.GameFactory;
using Codebase.Services.Input;
using Codebase.Services.SceneLoader;
using Codebase.Services.Updater;

namespace Codebase.Infrastructure.States
{
  public class GameStateMachine
  {
    private readonly ServicesContainer _servicesContainer;

    private Dictionary<Type, IGameState> _states;
    private IGameState _currentState;

    
    public GameStateMachine(ICoroutineRunningService coroutineRunner, IUpdateService updateService,
      ServicesContainer servicesContainer)
    {
      _servicesContainer = servicesContainer;
      _states = new Dictionary<Type, IGameState>
      {
        [typeof(BootstrapGameState)] = new BootstrapGameState
          (
            gameStateMachine: this,
            coroutineRunner: coroutineRunner,
            updateService: updateService,
            services: servicesContainer
          ),
        [typeof(LoadingMainGameState)] = new LoadingMainGameState
          (
            gameStateMachine: this,
            sceneLoadingService: _servicesContainer.Service<ISceneLoadingService>(),
            cameraService: _servicesContainer.Service<ICameraService>(), 
            inputService: _servicesContainer.Service<IInputService>(),
            gameFactory: _servicesContainer.Service<IGameFactoryService>()
          ),
        [typeof(MainGameState)] = new MainGameState
          (
            gameStateMachine: this,
            inputService: _servicesContainer.Service<IInputService>()
          )
      };
    }
    

    public void Enter<TState>() where TState : class, IGameState
    {
      if (_currentState != null
          &&
          _currentState is IGameStateWithExitMethod currentStateWithExit)
      {
        currentStateWithExit.Exit();
      }

      _currentState = _states[typeof(TState)] as TState;
      _currentState.Enter();
    }
  }
}