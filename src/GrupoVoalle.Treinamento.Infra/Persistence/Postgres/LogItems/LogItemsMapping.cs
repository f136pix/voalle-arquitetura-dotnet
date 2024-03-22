using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogItems
{
    public class LogItemsMapping : IEntityTypeConfiguration<LogItemsEntity>
    {
        public void Configure(EntityTypeBuilder<LogItemsEntity> entity)
        {
            entity.ToTable("log_items");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.LogTablesId)
                .HasColumnName("log_table_id");

            entity.Property(e => e.ColumnName)
                .HasColumnName("column_name");

            entity.Property(e => e.FriendlyColumnName)
                .HasColumnName("friendly_column_name");

            entity.Property(e => e.OldValue)
                .HasColumnName("old_value");

            entity.Property(e => e.FriendlyOldValue)
                .HasColumnName("friendly_old_value");

            entity.Property(e => e.NewValue)
                .HasColumnName("new_value");

            entity.Property(e => e.FriendlyNewValue)
                .HasColumnName("friendly_new_value");

            entity.Property(e => e.ColumnType)
                .HasColumnName("column_type");

            entity.HasOne(d => d.LogTable)
                .WithMany(p => p.LogItems)
                .HasForeignKey(d => d.LogTablesId);
        }
    }
}