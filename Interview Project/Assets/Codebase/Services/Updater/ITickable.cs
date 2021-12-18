namespace Codebase.Services.Updater
{
  public interface ITickable
  {
  }

  public interface ITickableRegular : ITickable
  {
    void Tick();
  }

  public interface ITickableFixed : ITickable
  {
    void FixedTick();
  }

  public interface ITickableLate : ITickable
  {
    void LateTick();
  }
}