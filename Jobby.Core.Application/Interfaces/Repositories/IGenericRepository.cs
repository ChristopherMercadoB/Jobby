using Ardalis.Specification;

namespace Jobby.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> : IRepositoryBase<T> where T : class
    {
    }

    public interface IGenericReadRepository<T> : IReadRepositoryBase<T> where T : class
    {
    }
}
