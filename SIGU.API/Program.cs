using Microsoft.EntityFrameworkCore;
using SIGU.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Usar SQLite (fácil para pruebas iniciales)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=SIGU.db"));

var app = builder.Build();

// Migrar y sembrar datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Programas.Any())
    {
        context.Programas.AddRange(
            new Domain.Programa { Nombre = "Ingeniería de Sistemas", Facultad = "Ingeniería", CreditosTotales = 160 },
            new Domain.Programa { Nombre = "Derecho", Facultad = "Ciencias Sociales", CreditosTotales = 140 }
        );
        context.SaveChanges();
    }
}

// Endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/programas", async (ApplicationDbContext db) =>
{
    return Results.Ok(await db.Programas.ToListAsync());
});

app.MapPost("/programas", async (ApplicationDbContext db, Domain.Programa programa) =>
{
    db.Programas.Add(programa);
    await db.SaveChangesAsync();
    return Results.Created($"/programas/{programa.Id}", programa);
});

app.Run();