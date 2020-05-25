using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity>
        where TEntity : class , IEntity , new()
        where TContext: DbContext,new()
        

    {
        
        public TEntity Get(Expression<Func<TEntity,bool>> filter)
        {
            using(var context=new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);

            }
        }
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();

            }
        }
        public void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }
        public void Update(TEntity entity)
        {
            UsersContext context = new UsersContext();
            var updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            context.SaveChanges();
            /*using ()
            {
                
            }*/
        }
        public async void UpdateAsync(TEntity entity)
        {
            UsersContext context = new UsersContext();
            var updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            await context.SaveChangesAsync();
            /*using ()
            {
                
            }*/
        }


        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deleteEntity = context.Entry(entity);
                deleteEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
        
       

    }
}
