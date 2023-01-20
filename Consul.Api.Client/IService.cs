public interface IService
{
    Task<string> GetService1();
    Task<string> GetService2();
    void InitServices();
}
