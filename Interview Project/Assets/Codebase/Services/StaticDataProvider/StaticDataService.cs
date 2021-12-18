using Codebase.StaticData;
using UnityEngine;

namespace Codebase.Services.StaticDataProvider
{
  public class StaticDataService : IStaticDataService
  {
    
    private const string PrefabsStaticDataPath = "Static Data/Prefabs/Prefabs";
    
    private PrefabsStaticData _prefabsData;
    
    public void Load() => 
      _prefabsData = Resources.Load(PrefabsStaticDataPath) as PrefabsStaticData;

    public PrefabsStaticData Prefabs() => _prefabsData;
  }
}