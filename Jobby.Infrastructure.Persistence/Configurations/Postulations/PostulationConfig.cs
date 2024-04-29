using Jobby.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Persistence.Configurations.Postulations
{
    public class PostulationConfig : IEntityTypeConfiguration<Postulation>
    {
        public void Configure(EntityTypeBuilder<Postulation> model)
        {
            model.ToTable("Postulaciones");
            model.HasKey(e => new { e.UserId, e.VacantId });
        }
    }
}
