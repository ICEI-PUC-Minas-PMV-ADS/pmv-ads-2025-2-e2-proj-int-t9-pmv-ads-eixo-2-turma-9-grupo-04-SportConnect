using CriarGrupo.Models;
using Microsoft.EntityFrameworkCore;

namespace SportConnect.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Grupo> Grupos { get; set; }

        public DbSet<Evento> Eventos { get; set; }

        public DbSet<Participacao> Participacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grupo>()
                .HasOne(g => g.Usuario)
                .WithMany(u => u.Grupos)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Evento → Grupo
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Grupo)
                .WithMany(u => u.Eventos)
                .HasForeignKey(e => e.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evento → Criador (Usuario)
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Criador)
                .WithMany(u => u.EventosCriados)
                .HasForeignKey(e => e.CriadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

    
