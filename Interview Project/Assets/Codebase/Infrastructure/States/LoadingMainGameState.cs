using Codebase.Logic.Bots;
using Codebase.Logic.Bots.Commands;
using Codebase.Logic.Buildings;
using Codebase.Services.Cameras;
using Codebase.Services.GameFactory;
using Codebase.Services.Input;
using Codebase.Services.SceneLoader;
using Codebase.UI;
using UnityEngine;

namespace Codebase.Infrastructure.States
{
  public class LoadingMainGameState : IGameState
  {
    private const int IndexOfMainGameSceneInBuild = 1;

    private readonly GameStateMachine _gameStateMachine;

    private readonly ISceneLoadingService _sceneLoadingService;
    private readonly ICameraService _cameraService;
    private readonly IInputService _inputService;
    private readonly IGameFactoryService _gameFactory;

    
    public LoadingMainGameState(GameStateMachine gameStateMachine, ISceneLoadingService sceneLoadingService,
      ICameraService cameraService, IInputService inputService, IGameFactoryService gameFactory)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoadingService = sceneLoadingService;
      _cameraService = cameraService;
      _inputService = inputService;
      _gameFactory = gameFactory;
    }
    

    public void Enter()
    {
      _sceneLoadingService.LoadScene(IndexOfMainGameSceneInBuild);
      _sceneLoadingService.OnSceneLoaded += ResolveOnSceneLoaded;
    }

    private void ResolveOnSceneLoaded()
    {
      _cameraService.AssignMainCamera(Camera.main);

      InitBuildings(out ParkingPlot corral);

      GameObject botControlScripts = InitBotsControlScripts(corral);

      InitBots(botControlScripts.transform, botControlScripts.GetComponentInChildren<BotsCommandsIssuer>());

      InitHud(botControlScripts.GetComponentInChildren<BotsAlarm>());


      _gameStateMachine.Enter<MainGameState>();
    }

    private void InitBuildings(out ParkingPlot parkingPlot)
    {
      Factory[] factories = Object.FindObjectsOfType<Factory>();
      foreach (Factory factory in factories)
        factory.Construct(_gameFactory);

      Warehouse[] warehouses = Object.FindObjectsOfType<Warehouse>();
      foreach (Warehouse warehouse in warehouses)
        warehouse.Construct(_gameFactory);

      parkingPlot = Object.FindObjectOfType<ParkingPlot>();
    }

    private GameObject InitBotsControlScripts(ParkingPlot parkingPlot)
    {
      GameObject botControlScripts = _gameFactory.CreateBotControlScriptsGameObject();
      botControlScripts.GetComponentInChildren<Selector>().Construct(_inputService);
      botControlScripts.GetComponentInChildren<InputToBotCommandsTranslator>().Construct(_inputService);
      botControlScripts.GetComponentInChildren<BotsCommandsIssuer>().Construct(parkingPlot);

      return botControlScripts;
    }

    private void InitBots(Transform parent, BotsCommandsIssuer botsCommandsIssuer)
    {
      BotSpawnPoint[] botSpawnPoints = Object.FindObjectsOfType<BotSpawnPoint>();
      foreach (BotSpawnPoint spawnPoint in botSpawnPoints)
      {
        Bot bot = _gameFactory.SpawnBot(spawnPoint.transform.position, spawnPoint.transform.rotation, parent)
          .GetComponentInChildren<Bot>();
        botsCommandsIssuer.AddBot(bot);
      }
    }

    private void InitHud(BotsAlarm botsAlarm)
    {
      GameObject hud = _gameFactory.CreateHud();
      hud.GetComponentInChildren<AlarmButton>().Construct(botsAlarm);
    }
  }
}