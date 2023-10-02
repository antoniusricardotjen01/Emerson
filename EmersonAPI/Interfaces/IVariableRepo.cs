using EmersonDB.Model;

namespace EmersonAPI.Interfaces
{
    public interface IVariableRepo
    {
        IQueryable<Variable> GetAllVariable();

        Task<Variable?> GetVariableByID(int id);
    }
}
