using UnityEngine;

namespace Codebase.Infrastructure
{
  public class GameStarterFromAnyScene : MonoBehaviour
  {
    [SerializeField] private GameBootstrapper _bootstrapperPrefab;

    private void Awake()
    {
      if (FindObjectOfType<GameBootstrapper>() == null)
        Instantiate(_bootstrapperPrefab);
    }
  }
}