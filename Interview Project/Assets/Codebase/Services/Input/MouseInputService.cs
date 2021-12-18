using System;
using Codebase.Services.Cameras;
using Codebase.Services.Updater;
using UnityEngine;

namespace Codebase.Services.Input
{
  public class MouseInputService : ITickableRegular, IInputService
  {
    public Transform HitTransform { get; private set; }
    public Vector3 HitPoint { get; private set; }

    private readonly ICameraService _cameraService;

    private bool _inputIsLocked;
    private RaycastHit _hit;
    
    
    public event Action OnSelectButtonClicked;
    public event Action OnNewContextActionButtonClicked;
    public event Action OnAddContextActionButtonClicked;
    

    public void LockInput() => 
      _inputIsLocked = true;

    public void UnlockInput() => 
      _inputIsLocked = false;

    public MouseInputService(IUpdateService updateService, ICameraService cameraService)
    {
      _cameraService = cameraService;
      updateService.RegisterForUpdates(this);
    }

    public void Tick()
    {
      if(_inputIsLocked)
        return;
      
      if (UnityEngine.Input.GetMouseButtonDown(0) && TryHitWithRaycast())
      {
        SetPublicPropertiesForHitResult();

        OnSelectButtonClicked?.Invoke();
      }

      else if (UnityEngine.Input.GetMouseButtonDown(1) && UnityEngine.Input.GetKey(KeyCode.LeftShift)
                && TryHitWithRaycast())
      {
        SetPublicPropertiesForHitResult();
        
        OnAddContextActionButtonClicked?.Invoke();
      }
      
      else if (UnityEngine.Input.GetMouseButtonDown(1) && TryHitWithRaycast())
      {
        SetPublicPropertiesForHitResult();
        
        OnNewContextActionButtonClicked?.Invoke();
      }
    }


    private bool TryHitWithRaycast()
    {
      if(_cameraService.Camera == null)
        throw new Exception("Mouse Input needs camera to be assigned in the Camera Service");
      
      return Physics.Raycast(_cameraService.Camera.ScreenPointToRay(UnityEngine.Input.mousePosition), out _hit);
    }

    private void SetPublicPropertiesForHitResult()
    {
      HitPoint = _hit.point;
      HitTransform = _hit.transform;
    }
  }
}