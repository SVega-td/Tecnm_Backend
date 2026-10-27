using HelpDeskWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HelpDeskWeb.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public object tickets { get; internal set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>()
                    .Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");  // SQL Server
            
            builder.Entity<User>()
                    .Property(u => u.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");  // SQL Server

            builder.Entity<TicketStatus>()
                    .Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");  

            builder.Entity<TicketStatus>()
                    .Property(u => u.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()"); 
            
            builder.Entity<TicketStatus>()
                    .Property(u => u.Name)      
                    .IsRequired();
        }
    }
}