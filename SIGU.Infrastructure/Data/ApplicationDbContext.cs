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
    public DbSet<ProyectoInv> ProyectosInv => Set<ProyectoInv>();
    public DbSet<ProductoInv> ProductosInv => Set<ProductoInv>();
    public DbSet<PQRS> PQRSs => Set<PQRS>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
}
