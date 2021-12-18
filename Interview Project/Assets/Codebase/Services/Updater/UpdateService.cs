using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Services.Updater
{
  public class UpdateService : MonoBehaviour, IUpdateService
  {
    private List<ITickableRegular> _updatedObjects;
    private List<ITickableFixed> _fixedUpdatedObjects;
    private List<ITickableLate> _lateUpdatedObjects;

    private void Awake()
    {
      _updatedObjects = new List<ITickableRegular>();
      _fixedUpdatedObjects = new List<ITickableFixed>();
      _lateUpdatedObjects = new List<ITickableLate>();
    }

    private void Update()
    {
      foreach (ITickableRegular tickable in _updatedObjects) 
        tickable.Tick();
    }

    private void FixedUpdate()
    {
      foreach (ITickableFixed tickable in _fixedUpdatedObjects) 
        tickable.FixedTick();
    }

    private void LateUpdate()
    {
      foreach (ITickableLate tickable in _lateUpdatedObjects) 
        tickable.LateTick();
    }
    

    public void RegisterForUpdates(ITickable tickable)
    {
      TryAddNewObjectToList(tickable, _updatedObjects);
      TryAddNewObjectToList(tickable, _fixedUpdatedObjects);
      TryAddNewObjectToList(tickable, _lateUpdatedObjects);
    }

    public void UnregisterFromUpdates(ITickable tickable)
    {
      TryRemoveObjectFromList(tickable, _updatedObjects);
      TryRemoveObjectFromList(tickable, _fixedUpdatedObjects);
      TryRemoveObjectFromList(tickable, _lateUpdatedObjects);
    }

    private void TryAddNewObjectToList<TTickable>(ITickable tickable, List<TTickable> list) where TTickable : ITickable
    {
      if (tickable is TTickable tickableOfTargetType && !list.Contains(tickableOfTargetType))
        list.Add(tickableOfTargetType);
    }

    private void TryRemoveObjectFromList<TTickable>(ITickable tickable, List<TTickable> list) where  TTickable : ITickable
    {
      if (tickable is TTickable tickableOfTargetType)
        list.Remove(tickableOfTargetType);
    }
  }
}