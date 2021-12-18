using System;
using System.Collections;
using Codebase.Logic.Clickable;
using Codebase.Services.GameFactory;
using UnityEngine;

namespace Codebase.Logic.Buildings
{
  public class Factory : MonoBehaviour, IContextActionTarget
  {
    [SerializeField] private Transform _characterInteractionPointForItemDelivery;
    
    [SerializeField] private FactoryAnimationTweener _tweener;
    [SerializeField] private VfxPlayer _vfxPlayer;
    [SerializeField] private Inventory _inventory;


    public ContextActionTargetTypeId ContextActionTargetTypeId => ContextActionTargetTypeId.Factory;
    
    public Transform CharacterInteractionPointForItemSupplying => _characterInteractionPointForItemDelivery;
    public bool InventoryIsCurrentlyHoldingAnItem { get; private set; }


    private const float _delayBeforeVacantInventoryIsAvailable = .3f;

    private IGameFactoryService _gameFactory;
    
    
    public event Action OnInventoryHasBecomeVacant;


    public void Construct(IGameFactoryService gameFactory) => 
      _gameFactory = gameFactory;

    
    private void Start() => 
      _inventory.OnInventoryContentsChanged += ResolveInventoryContentsChanged;

    private void OnDestroy()
    {
      if(_inventory)
        _inventory.OnInventoryContentsChanged -= ResolveInventoryContentsChanged;
    }


    public void PutItemIntoFactoryInventory(Transform item)
    {
      _inventory.PutItemIntoInventory(item);
      StartItemDestructionSequence(item);
    }

    private void ResolveInventoryContentsChanged()
    {
      if (!_inventory.IsCurrentlyHoldingAnItem)
        StartCoroutine(DelayBeforeMakingInventoryAvailableCoroutine());
      else
        InventoryIsCurrentlyHoldingAnItem = true;
    }

    private IEnumerator DelayBeforeMakingInventoryAvailableCoroutine()
    {
      yield return new WaitForSeconds(_delayBeforeVacantInventoryIsAvailable);
      
      InventoryIsCurrentlyHoldingAnItem = false;
      OnInventoryHasBecomeVacant?.Invoke();
    }

    private void StartItemDestructionSequence(Transform item) => 
      _tweener.AnimateItemDestruction(item, DestroyItem);

    private void DestroyItem()
    {
      _inventory.TakeItemOutOfInventory(out Transform item);
      _gameFactory.DespawnItem(item.gameObject);
      _vfxPlayer.PlayVFX();
    }
  }
}