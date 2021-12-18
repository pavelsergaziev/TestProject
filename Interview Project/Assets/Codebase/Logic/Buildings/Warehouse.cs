using System.Collections;
using Codebase.Logic.Clickable;
using Codebase.Services.GameFactory;
using UnityEngine;

namespace Codebase.Logic.Buildings
{
  public class Warehouse : MonoBehaviour, IContextActionTarget
  {
    [SerializeField] private Transform _itemSpawnPoint;
    [SerializeField] private Transform _itemPointForGivingOut;
    [SerializeField] private Transform _characterInteractionPointForItemSupplying;

    [SerializeField] private WarehouseAnimationTweener _tweener;
    [SerializeField] private VfxPlayer _vfxPlayer;
    [SerializeField] private Inventory _inventory;

    
    public ContextActionTargetTypeId ContextActionTargetTypeId => ContextActionTargetTypeId.Warehouse;

    public Transform CharacterInteractionPointForItemSupplying => _characterInteractionPointForItemSupplying;
    public Inventory Inventory => _inventory;

    
    private const int DelayBeforeNewItemSpawns = 2;
    
    private IGameFactoryService _gameFactory;
    private Transform _currentlyAnimatingItem;
    

    public void Construct(IGameFactoryService gameFactoryService) => 
      _gameFactory = gameFactoryService;

    
    private void Start()
    {
      _inventory.OnInventoryContentsChanged += ResolveInventoryContentsChanged;
      StartItemSpawningSequence();
    }

    private void OnDestroy() => 
      _inventory.OnInventoryContentsChanged -= ResolveInventoryContentsChanged;

    
    private void ResolveInventoryContentsChanged()
    {
      if(!_inventory.IsCurrentlyHoldingAnItem)
        StartItemSpawningSequence();
    }

    private void StartItemSpawningSequence() => 
      StartCoroutine(ItemSpawningCoroutine());

    private IEnumerator ItemSpawningCoroutine()
    {
      yield return new WaitForSeconds(DelayBeforeNewItemSpawns);
      
      _currentlyAnimatingItem = _gameFactory.SpawnItem(_itemSpawnPoint.position, _itemPointForGivingOut.rotation).transform;
      _tweener.AnimateItemSpawn(_currentlyAnimatingItem, _itemPointForGivingOut.position, ResolveItemSpawnedAndFinishedAnimation);
    }

    private void ResolveItemSpawnedAndFinishedAnimation()
    {
      _inventory.PutItemIntoInventory(_currentlyAnimatingItem);
      _currentlyAnimatingItem = null;

      _vfxPlayer.PlayVFX();
    }
  }
}