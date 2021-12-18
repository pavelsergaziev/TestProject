using UnityEngine;

namespace Codebase.StaticData
{
  [CreateAssetMenu(fileName = "Prefabs", menuName = "Game Settings/Prefabs For Runtime Spawning")]
  public class PrefabsStaticData : ScriptableObject
  {
    [SerializeField] private GameObject _botsCommandScriptsGameObject;
    [SerializeField] private GameObject _bot;
    [SerializeField] private GameObject _item;
    [SerializeField] private GameObject _ui;

    public GameObject BotsCommandScriptsGameObject => _botsCommandScriptsGameObject;
    public GameObject Bot => _bot;
    public GameObject Item => _item;
    public GameObject UI => _ui;
  }
}