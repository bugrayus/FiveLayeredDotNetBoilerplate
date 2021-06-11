using System.Threading.Tasks;

namespace Boilerplate.DAL.Abstract
{
    public interface IGenericRepository<in T> where T : class
    {
        Task<bool> Create(T model);
        Task<bool> Update(T model);
    }
}