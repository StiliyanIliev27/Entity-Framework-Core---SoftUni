using EventMe.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Data.Configurations
{
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        private readonly string[] towns = new string[] { "София", "Пловдив", "Варна", "Бургас" };
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            List<Town> entities = new List<Town>();
            int id = 1;

            foreach (var item in towns)
            {
                entities.Add(new Town
                {
                    Id = id++,
                    Name = item
                });
            }

            builder.HasData(entities);
        }
    }
}
