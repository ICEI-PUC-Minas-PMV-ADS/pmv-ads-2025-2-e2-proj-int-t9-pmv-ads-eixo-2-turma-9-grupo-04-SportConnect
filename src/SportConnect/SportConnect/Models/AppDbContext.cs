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

        public DbSet<Modalidade> Modalidades { get; set; }

        public DbSet<Notificacao> Notificacoes { get; set; }
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

            modelBuilder.Entity<Modalidade>().HasData(
                new Modalidade { Id = 1, Nome = "Futebol" },
                new Modalidade { Id = 2, Nome = "Vôlei" },
                new Modalidade { Id = 3, Nome = "Futsal" },
                new Modalidade { Id = 4, Nome = "Basquete" },
                new Modalidade { Id = 5, Nome = "Natação" },
                new Modalidade { Id = 6, Nome = "Caminhada e Corrida" },
                new Modalidade { Id = 7, Nome = "Ciclismo" },
                new Modalidade { Id = 8, Nome = "Tênis de Mesa" },
                new Modalidade { Id = 9, Nome = "Musculação" },
                new Modalidade { Id = 10, Nome = "Surf" },
                new Modalidade { Id = 11, Nome = "Skate" },
                new Modalidade { Id = 12, Nome = "Judô" },
                new Modalidade { Id = 13, Nome = "Jiu-Jitsu" },
                new Modalidade { Id = 14, Nome = "Boxe" },
                new Modalidade { Id = 15, Nome = "Capoeira" },
                new Modalidade { Id = 16, Nome = "Handebol" },
                new Modalidade { Id = 17, Nome = "Vôlei de Praia" },
                new Modalidade { Id = 18, Nome = "Futevôlei" },
                new Modalidade { Id = 19, Nome = "Tênis" },
                new Modalidade { Id = 20, Nome = "Atletismo" },
                new Modalidade { Id = 21, Nome = "Ginástica Artística" },
                new Modalidade { Id = 22, Nome = "Ginástica Rítmica" },
                new Modalidade { Id = 23, Nome = "Taekwondo" },
                new Modalidade { Id = 24, Nome = "Karatê" },
                new Modalidade { Id = 25, Nome = "Mountain Bike" },
                new Modalidade { Id = 26, Nome = "Canoagem" },
                new Modalidade { Id = 27, Nome = "Remo" },
                new Modalidade { Id = 28, Nome = "Polo Aquático" },
                new Modalidade { Id = 29, Nome = "Halterofilismo" },
                new Modalidade { Id = 30, Nome = "Golfe" },
                new Modalidade { Id = 31, Nome = "Beisebol" },
                new Modalidade { Id = 32, Nome = "Softbol" },
                new Modalidade { Id = 33, Nome = "Futebol Americano" },
                new Modalidade { Id = 34, Nome = "Rugby" },
                new Modalidade { Id = 35, Nome = "Esgrima" },
                new Modalidade { Id = 36, Nome = "Tiro Esportivo" },
                new Modalidade { Id = 37, Nome = "Hipismo" },
                new Modalidade { Id = 38, Nome = "Badminton" },
                new Modalidade { Id = 39, Nome = "Squash" },
                new Modalidade { Id = 40, Nome = "Peteca" },
                new Modalidade { Id = 41, Nome = "Beach Tennis" },
                new Modalidade { Id = 42, Nome = "Padel" },
                new Modalidade { Id = 43, Nome = "Pesca Esportiva" },
                new Modalidade { Id = 44, Nome = "Triatlo" },
                new Modalidade { Id = 45, Nome = "Muay Thai" },
                new Modalidade { Id = 46, Nome = "Iatismo/Vela" },
                new Modalidade { Id = 47, Nome = "Automobilismo" },
                new Modalidade { Id = 48, Nome = "Motociclismo" },
                new Modalidade { Id = 49, Nome = "Dança Esportiva" },
                new Modalidade { Id = 50, Nome = "Xadrez" }
            );
        }
    }
}