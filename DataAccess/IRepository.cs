using System;
using System.Collections.Generic;
using System.Text;
using FinanceManager.Domain;

namespace FinanceManager.DataAccess
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);

    }
}
