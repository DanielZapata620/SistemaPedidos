using Microsoft.EntityFrameworkCore;
using PedidoApi.Data;
using PedidoApi.Models.Entities;

namespace PedidoApi.Repositories;

public class GenericRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _set;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public List<T> GetAll()
    {
        return _set.AsNoTracking().ToList();
    }

    public IQueryable<T> Query()
    {
        return _set.AsQueryable();
    }

    public T? GetById(int id)
    {
        return _set.Find(id);
    }

    public T? FirstOrDefault(Func<T, bool> predicate)
    {
        return _set.FirstOrDefault(predicate);
    }

    public T Add(T entity)
    {
        _set.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _set.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public bool Delete(int id)
    {
        var entity = GetById(id);
        if (entity is null)
        {
            return false;
        }

        _set.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}
