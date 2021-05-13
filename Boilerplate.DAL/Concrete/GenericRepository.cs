using Boilerplate.DAL.Abstract;
using Boilerplate.DAL.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Boilerplate.DAL.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BoilerplateContext Context;
        protected readonly ILogger<dynamic> Logger;

        public GenericRepository(BoilerplateContext context, ILogger<dynamic> logger)
        {
            Context = context;
            Logger = logger;
        }

        #region Create
        public async Task<bool> Create(T model)
        {
            await Context.Set<T>().AddAsync(model);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // ignored
            }
            return true;
        }
        #endregion

        #region Update
        public async Task<bool> Update(T model)
        {
            Context.Set<T>().Update(model);
            await Context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
