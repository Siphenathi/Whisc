using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById (object id);
        void Save(TEntity entity);
        List<TEntity> getAll();
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
