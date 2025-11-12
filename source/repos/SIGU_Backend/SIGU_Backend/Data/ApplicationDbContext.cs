using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Models;
using System.Threading.Tasks;


namespace SIGU_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Programa> Programas { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<DetalleMatricula> DetalleMatriculas { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
    }
}