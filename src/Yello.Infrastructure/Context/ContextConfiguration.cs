using Microsoft.EntityFrameworkCore;
using Yello.Core.Models;
using Yello.Infrastructure.EntityConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Infrastructure.Context
{
    public static class ContextConfiguration
    {
        public static void AddConfigurations(this ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            //modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            //modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            //modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleApplicationConfiguration());
            //modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

        }
    }
}
