using Microsoft.EntityFrameworkCore;
using SIGU.Domain;

namespace SIGU.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Programa> Programas => Set<Programa>();
    public DbSet<Materia> Materias => Set<Materia>();
    public DbSet<Prerrequisito> Prerrequisitos => Set<Prerrequisito>();
    public DbSet<Periodo> Periodos => Set<Periodo>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<DetalleMatricula> DetallesMatricula => Set<DetalleMatricula>();
    public DbSet<Nota> Notas => Set<Nota>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Presupuesto> Presupuestos => Set<Presupuesto>();
    public DbSet<Nomina> Nominas => Set<Nomina>();
    public DbSet<PQRS> PQRS => Set<PQRS>();
    public DbSet<ProyectoInv> ProyectosInv => Set<ProyectoInv>();
    public DbSet<ProductoInv> ProductosInv => Set<ProductoInv>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rol).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configuración de Programa
        modelBuilder.Entity<Programa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Facultad).IsRequired().HasMaxLength(100);
        });

        // Configuración de Materia
        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Codigo).IsUnique();
        });

        // Configuración de Periodo
        modelBuilder.Entity<Periodo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Estado).HasMaxLength(20);
        });

        // Configuración de Grupo
        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Horario).HasMaxLength(100);
            
            entity.HasOne(g => g.Materia)
                .WithMany(m => m.Grupos)
                .HasForeignKey(g => g.IdMateria)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Periodo)
                .WithMany(p => p.Grupos)
                .HasForeignKey(g => g.IdPeriodo)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Docente)
                .WithMany(u => u.GruposDocente)
                .HasForeignKey(g => g.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Matricula
        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Estado).HasMaxLength(20);
            
            entity.HasOne(m => m.Estudiante)
                .WithMany(u => u.Matriculas)
                .HasForeignKey(m => m.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Periodo)
                .WithMany(p => p.Matriculas)
                .HasForeignKey(m => m.IdPeriodo)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de DetalleMatricula
        modelBuilder.Entity<DetalleMatricula>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Estado).HasMaxLength(20);
            
            entity.HasOne(d => d.Matricula)
                .WithMany(m => m.DetallesMatricula)
                .HasForeignKey(d => d.IdMatricula)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Grupo)
                .WithMany(g => g.DetallesMatricula)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // Configuración de Prerrequisito
        modelBuilder.Entity<Prerrequisito>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(p => p.Materia)
                .WithMany(m => m.Prerrequisitos)
                .HasForeignKey(p => p.IdMateria)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(p => p.MateriaRequisito)
                .WithMany()
                .HasForeignKey(p => p.IdMateriaReq)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}