using Codebase.StaticData;

namespace Codebase.Services.StaticDataProvider
{
  public interface IStaticDataService : IService
  {
    void Load();
    PrefabsStaticData Prefabs();
  }
}