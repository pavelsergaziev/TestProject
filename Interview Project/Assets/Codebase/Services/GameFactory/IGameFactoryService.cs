using UnityEngine;

namespace Codebase.Services.GameFactory
{
  public interface IGameFactoryService : IService
  {
    GameObject CreateBotControlScriptsGameObject();
    GameObject CreateHud();
    GameObject SpawnBot(Vector3 position, Quaternion rotation, Transform parent);
    GameObject SpawnItem(Vector3 position, Quaternion quaternion);
    void DespawnItem(GameObject item);
  }
}