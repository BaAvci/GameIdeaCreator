using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameIdeaCreator
{
    public class GameContext : DbContext
    {
        public DbSet<GameInfo> Game { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_Connection");
            optionsBuilder.UseSqlServer(sqlConnectionString);
        }
    }
}
