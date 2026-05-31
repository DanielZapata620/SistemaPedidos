using API.Models.Entities;

namespace API.Repositories
{
    public class Repository<T> where T : class
    {
        public Repository(PlataformaalimentosContext context)
        {
            Context = context;
        }
        public PlataformaalimentosContext Context { get; }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public T? Get(object id)
        {
            return Context.Find<T>(id);
        }
        public void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            Context.AddRange(entities);
            Context.SaveChanges();
        }
        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
        public void Delete(object id)
        {
            T? entity = Get(id);
            if (entity != null)
            {
                Context.Remove(entity);
                Context.SaveChanges();
            }
        }
        public IQueryable<T> Query()
        {
            return Context.Set<T>().AsQueryable();
        }
    }
}
