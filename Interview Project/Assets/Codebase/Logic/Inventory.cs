using System;
using UnityEngine;

namespace Codebase.Logic
{
  public class Inventory : MonoBehaviour
  {
    [SerializeField] private Transform _itemHoldingPoint;

    public bool IsCurrentlyHoldingAnItem => _currentlyHeldItem != null;

    private Transform _currentlyHeldItem;
    
    
    public event Action OnInventoryContentsChanged;
    

    public void PutItemIntoInventory(Transform item)
    {
      _currentlyHeldItem = item;
      _currentlyHeldItem.position = _itemHoldingPoint.position;
      _currentlyHeldItem.rotation = _itemHoldingPoint.rotation;
      _currentlyHeldItem.parent = _itemHoldingPoint;
      
      OnInventoryContentsChanged?.Invoke();
    }

    public void TakeItemOutOfInventory(out Transform item)
    {
      item = _currentlyHeldItem;
      _currentlyHeldItem = null;
      
      OnInventoryContentsChanged?.Invoke();
    }
  }
}