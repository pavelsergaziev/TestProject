using Codebase.Logic.Bots;
using Codebase.Logic.Bots.Commands;
using Codebase.Services.StaticDataProvider;
using UnityEngine;

namespace Codebase.Services.GameFactory
{
  public class GameFactoryService : IGameFactoryService
  {
    private readonly IStaticDataService _staticDataService;

    private GameObject _botControlScriptsGameObject;
    private BotsCommandsIssuer _botsCommandsIssuer;
    private BotsAlarm _botsAlarm;

    
    public GameFactoryService(IStaticDataService staticDataService) => 
      _staticDataService = staticDataService;
    

    public GameObject CreateBotControlScriptsGameObject() => 
      Object.Instantiate(_staticDataService.Prefabs().BotsCommandScriptsGameObject);

    public GameObject CreateHud() =>
      Object.Instantiate(_staticDataService.Prefabs().UI);

    public GameObject SpawnBot(Vector3 position, Quaternion rotation, Transform parent) =>
      Object.Instantiate(_staticDataService.Prefabs().Bot, position, rotation, parent);

    public GameObject SpawnItem(Vector3 position, Quaternion rotation) =>
      Object.Instantiate(_staticDataService.Prefabs().Item, position, rotation);

    public void DespawnItem(GameObject item) =>
      Object.Destroy(item);
  }
}