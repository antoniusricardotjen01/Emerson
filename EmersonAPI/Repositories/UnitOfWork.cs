using EmersonAPI.Interfaces;
using EmersonDB;

namespace EmersonAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmersonContext _context;

        public ICityRepo iCityRepo { get; }

        public IVariableRepo iVariableRepo { get; }

        public UnitOfWork(EmersonContext context, ICityRepo _cityRepo,
            IVariableRepo _variableRepo)
        {
            _context = context;
            iCityRepo = _cityRepo;
            iVariableRepo = _variableRepo;
        }

        //for a centralized commit, a business service can call many repo and do add/update/delete but do a single command of commit
        //hence the rollback
        public bool Commit()
        {
            bool result = false;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges() > 0;
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    //Logging execution to be put here;
                }
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
