using GabrielConstrucaoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GabrielConstrucaoMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ContatoWhatsApp> ContatosWhatsApp { get; set; }
        public DbSet<RegistroFaturamento> RegistrosFaturamento { get; set; }
    }
}