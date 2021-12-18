using UnityEngine;

namespace Codebase.Services.Cameras
{
  public class CameraService : ICameraService
  {
    public Camera Camera { get; private set; }

    public void AssignMainCamera(Camera camera) => 
      Camera = camera;
  }
}