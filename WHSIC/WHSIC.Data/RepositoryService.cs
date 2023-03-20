using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class RepositoryService<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected DbSet<TEntity> Table;
        private  DbContext Database;
        public RepositoryService(DbContext dbContext)
        {
            Database = dbContext;
            Table = Database.Set<TEntity>();
        }
        public RepositoryService() { }
        public void Save(TEntity entity)
        {
            Table.Add(entity);
            Database.SaveChanges();
        }
        public List<TEntity> getAll()
        {
            return Table.ToList();
        }

        public void Delete(TEntity entity)
        {
            Table.Remove(entity);
            Database.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.Database.SaveChanges();
        }

        public TEntity GetById(object id)
        {
            return Table.Find(id);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(this.Database!=null)
                {
                    this.Database.Dispose();
                    this.Database = null;
                }
            }
        }
    }
}
