namespace Codebase.Services.Updater
{
  public interface IUpdateService : IService
  {
    void RegisterForUpdates(ITickable tickable);
    void UnregisterFromUpdates(ITickable tickable);
  }
}