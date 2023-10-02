namespace EmersonAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICityRepo iCityRepo { get; }
        IVariableRepo iVariableRepo { get; }
        bool Commit();
    }
}
