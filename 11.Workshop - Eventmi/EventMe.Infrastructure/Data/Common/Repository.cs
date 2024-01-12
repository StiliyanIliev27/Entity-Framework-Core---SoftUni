


using EventMe.Infrastructure.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EventMe.Infrastructure.Data.Common
{
    /// <summary>
    /// Методи за достъп до данни
    /// </summary>
    public class Repository : IRepository
    {
        private readonly EventMeDbContext dbContext;

        /// <summary>
        /// Конструктор за инжектиране на контекста на базата данни
        /// </summary>
        /// <param name="dbContext">Контекст на базата данни</param>
        public Repository(EventMeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Връща за даден тип
        /// </summary>
        /// <typeparam name="T">Тип на Entity</typeparam>
        /// <returns></returns>
        private DbSet<T> DbSet<T>() where T : class => dbContext.Set<T>();

        /// <summary>
        /// Добавяне на елемент в базата данни
        /// </summary>
        /// <typeparam name="T">Тип на елемента</typeparam>
        /// <param name="entity">Елемент</param>
        /// <returns></returns>
        public async Task AddAsync<T>(T entity) where T : class
        {
            await DbSet<T>().AddAsync(entity);
        }

        /// <summary>
        /// Извличане на всички елементи от таблицата
        /// </summary>
        /// <typeparam name="T">Тип на елементите</typeparam>
        /// <returns></returns>
        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>();
        }

        /// <summary>
        /// Извличане на всички елементи от таблицата само за четене
        /// </summary>
        /// <typeparam name="T">Тип на елементите</typeparam>
        /// <returns></returns>
        public IQueryable<T> AllReadOnly<T>() where T : class
        {
            return DbSet<T>().AsNoTracking();
        }

        /// <summary>
        /// Извличане на елемент по идентификатор
        /// </summary>
        /// <typeparam name="T">Тип на елемента</typeparam>
        /// <param name="id">Идентификатор на елемента</param>
        /// <returns></returns>
        public async Task<T?> GetById<T>(int id) where T : class
        {
            return await DbSet<T>()
                .FindAsync();
        }

        /// <summary>
        /// Запис на промените в базата данни
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Извличане на всички елементи от таблицата, включително изтритите
        /// </summary>
        /// <typeparam name="T">Тип на елементите</typeparam>
        /// <returns></returns>
        IQueryable<T> IRepository.AllWithDeleted<T>()
        {
            return DbSet<T>()
                .IgnoreQueryFilters();
        }

        /// <summary>
        /// Извличане на всички елементи от таблицата, включително изтритите, само за четене
        /// </summary>
        /// <typeparam name="T">Тип на елементите</typeparam>
        /// <returns></returns>
        IQueryable<T> IRepository.AllWithDeletedReadOnly<T>()
        {
            return DbSet<T>()
                .IgnoreQueryFilters()
                .AsNoTracking();
        }

        /// <summary>
        /// Изтриване на елемент от базата данни
        /// </summary>
        /// <typeparam name="T">Тип на елемента</typeparam>
        /// <param name="entity">Елемент</param>
        void IRepository.Delete<T>(T entity) 
        {
            entity.IsActive = false;
            entity.DeletedOn = DateTime.Now;
        }
    }
}
