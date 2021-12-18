using System;

namespace Codebase.Services.SceneLoader
{
  public interface ISceneLoadingService : IService
  {
    void LoadScene(int indexOfSceneInBuild);
    void LoadScene(string sceneName);
    event Action OnSceneLoaded;
  }
}