using Codebase.Logic.Clickable;
using UnityEngine;

namespace Codebase.Logic.Bots
{
  public class Bot : MonoBehaviour, ISelectable
  {
    [SerializeField] private BotNavMeshLogic _botNavMeshLogic;
    [SerializeField] private Inventory _inventory;

    public SelectableObjectTypeId SelectableObjectTypeId => SelectableObjectTypeId.Bot;
    
    public Inventory Inventory => _inventory;
    public BotNavMeshLogic NavMeshLogic => _botNavMeshLogic;
  }
}