using System;
using UnityEngine;

namespace Codebase.Services.Input
{
  public interface IInputService : IService
  {
    Transform HitTransform { get; }
    Vector3 HitPoint { get; }
    
    event Action OnSelectButtonClicked;
    event Action OnNewContextActionButtonClicked;
    event Action OnAddContextActionButtonClicked;

    void LockInput();
    void UnlockInput();
  }
}