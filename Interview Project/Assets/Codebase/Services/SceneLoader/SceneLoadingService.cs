using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Services.SceneLoader
{
  public class SceneLoadingService : ISceneLoadingService
  {
    public event Action OnSceneLoaded;

    public void LoadScene(int indexOfSceneInBuild)
    {
      if (SceneManager.GetActiveScene().buildIndex == indexOfSceneInBuild)
      {
        OnSceneLoaded?.Invoke();
        return;
      }

      SceneManager.LoadSceneAsync(indexOfSceneInBuild).completed += ResolveSceneLoadingCompleted;
    }

    public void LoadScene(string sceneName)
    {
      if (SceneManager.GetActiveScene().name == sceneName)
      {
        OnSceneLoaded?.Invoke();
        return;
      }

      SceneManager.LoadSceneAsync(sceneName).completed += ResolveSceneLoadingCompleted;
    }

    private void ResolveSceneLoadingCompleted(AsyncOperation sceneLoadingOperation)
    {
      sceneLoadingOperation.completed -= ResolveSceneLoadingCompleted;
      OnSceneLoaded?.Invoke();
    }
  }
}