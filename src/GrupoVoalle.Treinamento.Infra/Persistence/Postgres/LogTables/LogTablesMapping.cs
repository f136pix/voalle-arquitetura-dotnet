using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogTables
{
    public class LogTablesMapping : IEntityTypeConfiguration<LogTablesEntity>
    {
        public void Configure(EntityTypeBuilder<LogTablesEntity> entity)
        {
            entity.ToTable("log_tables");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.TransactionGuid)
                .HasColumnName("transaction_guid");

            entity.Property(e => e.TableName)
                .HasColumnName("table_name");

            entity.Property(e => e.FriendlyTableName)
                .HasColumnName("friendly_table_name");

            entity.Property(e => e.RegisterKey)
                .HasColumnName("register_key");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.UserName)
                .HasColumnName("user_name");

            entity.Property(e => e.UserLogin)
                .HasColumnName("user_login");

            entity.Property(e => e.Origin)
                .HasColumnName("origin");

            entity.Property(e => e.DateLog)
                .HasColumnName("date_log");

            entity.Property(e => e.Operation)
                .HasColumnName("operation");

            entity.Property(e => e.Application)
                .HasColumnName("application");

            entity.Property(e => e.PersonId)
                .HasColumnName("person_id");

            entity.Property(e => e.PersonName)
                .HasColumnName("person_name");
        }
    }
}