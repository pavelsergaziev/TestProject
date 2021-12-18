namespace Codebase.Services
{
  public class ServicesContainer
  {
    public void RegisterService<TService>(TService implementation) where TService : IService =>
      Implementation<TService>.ServiceInstance = implementation;

    public TService Service<TService>() where TService : IService => 
      Implementation<TService>.ServiceInstance;

    
    private class Implementation<TService> where TService : IService
    {
      public static TService ServiceInstance;
    }
  }
}