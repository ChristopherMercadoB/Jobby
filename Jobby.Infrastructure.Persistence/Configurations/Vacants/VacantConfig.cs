using Jobby.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Persistence.Configurations.Vacants
{
    public class VacantConfig : IEntityTypeConfiguration<Vacant>
    {
        public void Configure(EntityTypeBuilder<Vacant> model)
        {
            model.ToTable("Vacantes");
            model.HasKey(v=>v.Id);
            model.HasMany<Postulation>(v=>v.Postulations)
                .WithOne(v=>v.Vacant)
                .HasForeignKey(v=>v.VacantId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
