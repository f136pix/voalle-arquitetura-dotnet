using GrupoVoalle.Treinamento.Business.Business.DTO.Functions;
using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogItems;
using GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogTables;
using Microsoft.EntityFrameworkCore;

namespace GrupoVoalle.Treinamento.Infra.DataContext
{
    /// <summary>
    /// ================================== A t e n ç ã o ==================================
    ///
    /// Ao fazer a adição de uma nova entidade, deverá ser realizado a ordenação dos itens
    /// contidos dentro da region "DbSet", do método "private void Creating....." e também
    /// dos "using's".
    ///
    /// ================================== A t e n ç ã o ==================================
    /// </summary>
    public sealed partial class GrupoVoalleContext
    {
        #region DbSet

        public DbSet<LogItemsEntity> LogItems { get; set; }
        public DbSet<LogTablesEntity> LogTables { get; set; }

        #endregion DbSet

        #region DbSet Functions
        public DbSet<FunctionFriendlyName> GetFriendlyName { get; set; }

        #endregion DbSet Functions

        private void Creating(ModelBuilder modelBuilder)
        {
            #region Model Builder

            modelBuilder.ApplyConfiguration(new LogItemsMapping());
            modelBuilder.ApplyConfiguration(new LogTablesMapping());

            modelBuilder.Entity<FunctionFriendlyName>().HasNoKey().ToFunction("vll_get_friendly_name");

            #endregion Model Builder
        }
    }
}