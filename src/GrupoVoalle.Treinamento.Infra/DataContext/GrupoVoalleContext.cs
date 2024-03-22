using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GrupoVoalle.Treinamento.Infra.DataContext
{
    public partial class GrupoVoalleContext : DbContext
    {
        public GrupoVoalleContext(DbContextOptions<GrupoVoalleContext> options) : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromHours(1));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Model Builder

            Creating(modelBuilder);

            #endregion Model Builder

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug, DbContextLoggerOptions.None);
        }

        public void SetDetached<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Unchanged;
        }

        /// <summary>
        /// Desatacha todas as entidades do contexto
        /// </summary>
        public void DetachAll()
        {
            foreach (var entityEntry in ChangeTracker.Entries().ToArray())
            {
                entityEntry.State = EntityState.Detached;
            }
        }
    }
}