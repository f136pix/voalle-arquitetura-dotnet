using GrupoVoalle.Base.Business.Primitives.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrupoVoalle.Base.Infra.Persistence.Postgres.People
{
    public class PeopleMapping : IEntityTypeConfiguration<PeopleEntity>
    {
        public void Configure(EntityTypeBuilder<PeopleEntity> entity)
        {
            entity.ToTable("people");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.TxId)
                .HasColumnName("tx_id");

            entity.Property(e => e.TypeTxId)
                .HasColumnName("type_tx_id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");

            entity.Property(e => e.Name2)
                .HasColumnName("name_2");

            entity.Property(e => e.Deleted)
                .HasColumnName("deleted");

            entity.Property(e => e.Created)
                .HasColumnName("created");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            entity.Property(e => e.Modified)
                .HasColumnName("modified");

            entity.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");
        }
    }
}