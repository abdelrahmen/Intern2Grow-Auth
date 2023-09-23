namespace Intern2Grow_Auth.Models
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Configuration;

    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(){}
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = Configuration.GetConnectionString("local");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}
