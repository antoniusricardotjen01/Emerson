using EmersonAPI.Interfaces;
using EmersonDB;
using EmersonDB.Model;
using Microsoft.EntityFrameworkCore;

namespace EmersonAPI.Repositories
{
    public class VariableRepo : GenericRepository<Variable>, IVariableRepo
    {

        public VariableRepo(EmersonContext dbContext): base(dbContext)
        {

        }

        IQueryable<Variable> IVariableRepo.GetAllVariable()
        {
            return GetAll();
        }

        async Task<Variable?> IVariableRepo.GetVariableByID(int id)
        {
            return await GetById(id);
        }
    }
}
