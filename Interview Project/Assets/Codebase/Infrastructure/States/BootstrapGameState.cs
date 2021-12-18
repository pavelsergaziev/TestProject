using Codebase.Services;
using Codebase.Services.Cameras;
using Codebase.Services.CoroutineRunner;
using Codebase.Services.GameFactory;
using Codebase.Services.Input;
using Codebase.Services.SceneLoader;
using Codebase.Services.StaticDataProvider;
using Codebase.Services.Updater;

namespace Codebase.Infrastructure.States
{
  public class BootstrapGameState : IGameState
  {
    private const int IndexOfBootstrapSceneInBuild = 0;

    private readonly GameStateMachine _gameStateMachine;
    private readonly ServicesContainer _services;

    private readonly ICoroutineRunningService _coroutineRunner;
    private readonly IUpdateService _updateService;
    private readonly ISceneLoadingService _sceneLoader;

    
    public BootstrapGameState(GameStateMachine gameStateMachine, ICoroutineRunningService coroutineRunner,
      IUpdateService updateService, ServicesContainer services)
    {
      _gameStateMachine = gameStateMachine;
      _coroutineRunner = coroutineRunner;
      _updateService = updateService;
      _services = services;

      RegisterServices();

      _sceneLoader = _services.Service<ISceneLoadingService>();
    }
    

    public void Enter()
    {
      _sceneLoader.OnSceneLoaded += ResolveBootstrapSceneLoaded;
      
      _sceneLoader.LoadScene(IndexOfBootstrapSceneInBuild);
    }

    private void RegisterServices()
    {
      _services.RegisterService(_coroutineRunner);
      _services.RegisterService(_updateService);

      _services.RegisterService<ISceneLoadingService>(new SceneLoadingService());
      _services.RegisterService<ICameraService>(new CameraService());

      RegisterInputService();

      RegisterStaticDataService();

      _services.RegisterService<IGameFactoryService>(new GameFactoryService(_services.Service<IStaticDataService>()));
    }

    private void RegisterInputService()
    {
      IInputService inputService = new MouseInputService(_updateService, _services.Service<ICameraService>());
      inputService.LockInput();
      _services.RegisterService(inputService);
    }

    private void RegisterStaticDataService()
    {
      StaticDataService staticDataService = new StaticDataService();
      staticDataService.Load();
      _services.RegisterService<IStaticDataService>(staticDataService);
    }

    private void ResolveBootstrapSceneLoaded()
    {
      _sceneLoader.OnSceneLoaded -= ResolveBootstrapSceneLoaded;
      SwitchToNextState();
    }

    private void SwitchToNextState() =>
      _gameStateMachine.Enter<LoadingMainGameState>();
  }
}