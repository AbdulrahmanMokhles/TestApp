using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Interfaces
{
    public interface IGenericRepo <T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(int id);
    }
}
