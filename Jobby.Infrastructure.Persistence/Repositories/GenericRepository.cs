using Ardalis.Specification.EntityFrameworkCore;
using Jobby.Core.Application.Interfaces.Repositories;
using Jobby.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
