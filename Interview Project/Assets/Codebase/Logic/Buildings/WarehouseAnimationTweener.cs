using System;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Logic.Buildings
{
  public class WarehouseAnimationTweener : MonoBehaviour
  {
    private const float ItemSpawnAnimationDuration = 1.5f;

    public void AnimateItemSpawn(Transform item, Vector3 targetPosition, Action callback) => 
      item.DOLocalMove(targetPosition, ItemSpawnAnimationDuration).OnComplete(new TweenCallback(callback));
  }
}