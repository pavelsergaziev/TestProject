using UnityEngine;

namespace Codebase.Services.Cameras
{
  public interface ICameraService : IService
  {
    void AssignMainCamera(Camera camera);
    Camera Camera { get; }
  }
}