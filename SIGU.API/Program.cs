using Microsoft.EntityFrameworkCore;
using SIGU.Infrastructure.Data;
using SIGU.Domain;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174", 
                          "http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configuración de BD desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    // Fallback a SQLite si no hay conexión configurada
    connectionString = "Data Source=SIGU.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
else if (connectionString.Contains("Host="))
{
    // PostgreSQL
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    // SQLite por defecto
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}

var app = builder.Build();

// Usar CORS
app.UseCors("AllowFrontend");

// Migrar y sembrar datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Programas.Any())
    {
        context.Programas.AddRange(
            new Programa { Nombre = "Ingeniería de Sistemas", Facultad = "Ingeniería", CreditosTotales = 160 },
            new Programa { Nombre = "Derecho", Facultad = "Ciencias Sociales", CreditosTotales = 140 }
        );
        context.SaveChanges();
    }
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agrupar todos los endpoints bajo /api
var apiGroup = app.MapGroup("/api");

// ==================== ENDPOINTS DE PROGRAMAS ====================
apiGroup.MapGet("/programas", async (ApplicationDbContext db) =>
{
    var programas = await db.Programas.ToListAsync();
    return Results.Ok(programas);
})
.WithName("GetProgramas")
.WithTags("Programas")
.Produces<List<Programa>>(StatusCodes.Status200OK);

apiGroup.MapGet("/programas/{id}", async (int id, ApplicationDbContext db) =>
{
    var programa = await db.Programas.FindAsync(id);
    return programa == null ? Results.NotFound() : Results.Ok(programa);
})
.WithName("GetPrograma")
.WithTags("Programas")
.Produces<Programa>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapPost("/programas", async (Programa programa, ApplicationDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(programa);
    if (!Validator.TryValidateObject(programa, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    db.Programas.Add(programa);
    await db.SaveChangesAsync();
    return Results.Created($"/programas/{programa.Id}", programa);
})
.WithName("CreatePrograma")
.WithTags("Programas")
.Produces<Programa>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

apiGroup.MapPut("/programas/{id}", async (int id, Programa programa, ApplicationDbContext db) =>
{
    if (id != programa.Id)
    {
        return Results.BadRequest("El ID no coincide");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(programa);
    if (!Validator.TryValidateObject(programa, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    var existingPrograma = await db.Programas.FindAsync(id);
    if (existingPrograma == null)
    {
        return Results.NotFound();
    }

    db.Entry(existingPrograma).CurrentValues.SetValues(programa);
    await db.SaveChangesAsync();
    return Results.Ok(existingPrograma);
})
.WithName("UpdatePrograma")
.WithTags("Programas")
.Produces<Programa>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapDelete("/programas/{id}", async (int id, ApplicationDbContext db) =>
{
    var programa = await db.Programas.FindAsync(id);
    if (programa == null)
    {
        return Results.NotFound();
    }

    db.Programas.Remove(programa);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeletePrograma")
.WithTags("Programas")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// ==================== ENDPOINTS DE USUARIOS ====================
apiGroup.MapGet("/usuarios", async (ApplicationDbContext db) =>
{
    var usuarios = await db.Usuarios.ToListAsync();
    return Results.Ok(usuarios);
})
.WithName("GetUsuarios")
.WithTags("Usuarios")
.Produces<List<Usuario>>(StatusCodes.Status200OK);

apiGroup.MapGet("/usuarios/{id}", async (int id, ApplicationDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    return usuario == null ? Results.NotFound() : Results.Ok(usuario);
})
.WithName("GetUsuario")
.WithTags("Usuarios")
.Produces<Usuario>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapPost("/usuarios", async (Usuario usuario, ApplicationDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(usuario);
    if (!Validator.TryValidateObject(usuario, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    // Verificar si el email ya existe
    if (await db.Usuarios.AnyAsync(u => u.Email == usuario.Email))
    {
        return Results.BadRequest("El email ya está registrado");
    }

    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
})
.WithName("CreateUsuario")
.WithTags("Usuarios")
.Produces<Usuario>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

apiGroup.MapPut("/usuarios/{id}", async (int id, Usuario usuario, ApplicationDbContext db) =>
{
    if (id != usuario.Id)
    {
        return Results.BadRequest("El ID no coincide");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(usuario);
    if (!Validator.TryValidateObject(usuario, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    var existingUsuario = await db.Usuarios.FindAsync(id);
    if (existingUsuario == null)
    {
        return Results.NotFound();
    }

    // Verificar si el email ya existe en otro usuario
    if (await db.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != id))
    {
        return Results.BadRequest("El email ya está registrado");
    }

    db.Entry(existingUsuario).CurrentValues.SetValues(usuario);
    await db.SaveChangesAsync();
    return Results.Ok(existingUsuario);
})
.WithName("UpdateUsuario")
.WithTags("Usuarios")
.Produces<Usuario>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapDelete("/usuarios/{id}", async (int id, ApplicationDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario == null)
    {
        return Results.NotFound();
    }

    db.Usuarios.Remove(usuario);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteUsuario")
.WithTags("Usuarios")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// ==================== ENDPOINTS DE MATERIAS (MATERIALS) ====================
apiGroup.MapGet("/materias", async (ApplicationDbContext db) =>
{
    var materias = await db.Materias.ToListAsync();
    return Results.Ok(materias);
})
.WithName("GetMaterias")
.WithTags("Materias")
.Produces<List<Materia>>(StatusCodes.Status200OK);

apiGroup.MapGet("/materias/{id}", async (int id, ApplicationDbContext db) =>
{
    var materia = await db.Materias.FindAsync(id);
    return materia == null ? Results.NotFound() : Results.Ok(materia);
})
.WithName("GetMateria")
.WithTags("Materias")
.Produces<Materia>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapPost("/materias", async (Materia materia, ApplicationDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(materia);
    if (!Validator.TryValidateObject(materia, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    // Verificar si el código ya existe
    if (await db.Materias.AnyAsync(m => m.Codigo == materia.Codigo))
    {
        return Results.BadRequest("El código de materia ya existe");
    }

    db.Materias.Add(materia);
    await db.SaveChangesAsync();
    return Results.Created($"/materias/{materia.Id}", materia);
})
.WithName("CreateMateria")
.WithTags("Materias")
.Produces<Materia>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

apiGroup.MapPut("/materias/{id}", async (int id, Materia materia, ApplicationDbContext db) =>
{
    if (id != materia.Id)
    {
        return Results.BadRequest("El ID no coincide");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(materia);
    if (!Validator.TryValidateObject(materia, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    var existingMateria = await db.Materias.FindAsync(id);
    if (existingMateria == null)
    {
        return Results.NotFound();
    }

    // Verificar si el código ya existe en otra materia
    if (await db.Materias.AnyAsync(m => m.Codigo == materia.Codigo && m.Id != id))
    {
        return Results.BadRequest("El código de materia ya existe");
    }

    db.Entry(existingMateria).CurrentValues.SetValues(materia);
    await db.SaveChangesAsync();
    return Results.Ok(existingMateria);
})
.WithName("UpdateMateria")
.WithTags("Materias")
.Produces<Materia>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapDelete("/materias/{id}", async (int id, ApplicationDbContext db) =>
{
    var materia = await db.Materias.FindAsync(id);
    if (materia == null)
    {
        return Results.NotFound();
    }

    db.Materias.Remove(materia);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteMateria")
.WithTags("Materias")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// ==================== ENDPOINTS DE MATRICULAS (INSCRIPTIONS) ====================
apiGroup.MapGet("/matriculas", async (ApplicationDbContext db) =>
{
    var matriculas = await db.Matriculas
        .ToListAsync();
    return Results.Ok(matriculas);
})
.WithName("GetMatriculas")
.WithTags("Matriculas")
.Produces<List<Matricula>>(StatusCodes.Status200OK);

apiGroup.MapGet("/matriculas/{id}", async (int id, ApplicationDbContext db) =>
{
    var matricula = await db.Matriculas.FindAsync(id);
    return matricula == null ? Results.NotFound() : Results.Ok(matricula);
})
.WithName("GetMatricula")
.WithTags("Matriculas")
.Produces<Matricula>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapGet("/matriculas/estudiante/{estudianteId}", async (int estudianteId, ApplicationDbContext db) =>
{
    var matriculas = await db.Matriculas
        .Where(m => m.EstudianteId == estudianteId)
        .ToListAsync();
    return Results.Ok(matriculas);
})
.WithName("GetMatriculasByEstudiante")
.WithTags("Matriculas")
.Produces<List<Matricula>>(StatusCodes.Status200OK);

apiGroup.MapPost("/matriculas", async (Matricula matricula, ApplicationDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(matricula);
    if (!Validator.TryValidateObject(matricula, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    // Verificar que el estudiante existe
    if (!await db.Usuarios.AnyAsync(u => u.Id == matricula.EstudianteId))
    {
        return Results.BadRequest("El estudiante no existe");
    }

    // Verificar que el periodo existe
    if (!await db.Periodos.AnyAsync(p => p.Id == matricula.IdPeriodo))
    {
        return Results.BadRequest("El periodo no existe");
    }

    db.Matriculas.Add(matricula);
    await db.SaveChangesAsync();
    return Results.Created($"/matriculas/{matricula.Id}", matricula);
})
.WithName("CreateMatricula")
.WithTags("Matriculas")
.Produces<Matricula>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

apiGroup.MapPut("/matriculas/{id}", async (int id, Matricula matricula, ApplicationDbContext db) =>
{
    if (id != matricula.Id)
    {
        return Results.BadRequest("El ID no coincide");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(matricula);
    if (!Validator.TryValidateObject(matricula, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    var existingMatricula = await db.Matriculas.FindAsync(id);
    if (existingMatricula == null)
    {
        return Results.NotFound();
    }

    // Verificar que el estudiante existe
    if (!await db.Usuarios.AnyAsync(u => u.Id == matricula.EstudianteId))
    {
        return Results.BadRequest("El estudiante no existe");
    }

    // Verificar que el periodo existe
    if (!await db.Periodos.AnyAsync(p => p.Id == matricula.IdPeriodo))
    {
        return Results.BadRequest("El periodo no existe");
    }

    db.Entry(existingMatricula).CurrentValues.SetValues(matricula);
    await db.SaveChangesAsync();
    return Results.Ok(existingMatricula);
})
.WithName("UpdateMatricula")
.WithTags("Matriculas")
.Produces<Matricula>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapDelete("/matriculas/{id}", async (int id, ApplicationDbContext db) =>
{
    var matricula = await db.Matriculas.FindAsync(id);
    if (matricula == null)
    {
        return Results.NotFound();
    }

    db.Matriculas.Remove(matricula);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteMatricula")
.WithTags("Matriculas")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// ==================== ENDPOINTS DE DETALLES DE MATRICULA ====================
apiGroup.MapGet("/detalles-matricula", async (ApplicationDbContext db) =>
{
    var detalles = await db.DetallesMatricula.ToListAsync();
    return Results.Ok(detalles);
})
.WithName("GetDetallesMatricula")
.WithTags("DetallesMatricula")
.Produces<List<DetalleMatricula>>(StatusCodes.Status200OK);

apiGroup.MapGet("/detalles-matricula/{id}", async (int id, ApplicationDbContext db) =>
{
    var detalle = await db.DetallesMatricula.FindAsync(id);
    return detalle == null ? Results.NotFound() : Results.Ok(detalle);
})
.WithName("GetDetalleMatricula")
.WithTags("DetallesMatricula")
.Produces<DetalleMatricula>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapGet("/detalles-matricula/matricula/{matriculaId}", async (int matriculaId, ApplicationDbContext db) =>
{
    var detalles = await db.DetallesMatricula
        .Where(d => d.IdMatricula == matriculaId)
        .ToListAsync();
    return Results.Ok(detalles);
})
.WithName("GetDetallesByMatricula")
.WithTags("DetallesMatricula")
.Produces<List<DetalleMatricula>>(StatusCodes.Status200OK);

apiGroup.MapPost("/detalles-matricula", async (DetalleMatricula detalle, ApplicationDbContext db) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(detalle);
    if (!Validator.TryValidateObject(detalle, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    // Verificar que la matrícula existe
    if (!await db.Matriculas.AnyAsync(m => m.Id == detalle.IdMatricula))
    {
        return Results.BadRequest("La matrícula no existe");
    }

    // Verificar que el grupo existe
    if (!await db.Grupos.AnyAsync(g => g.Id == detalle.IdGrupo))
    {
        return Results.BadRequest("El grupo no existe");
    }

    db.DetallesMatricula.Add(detalle);
    await db.SaveChangesAsync();
    return Results.Created($"/detalles-matricula/{detalle.Id}", detalle);
})
.WithName("CreateDetalleMatricula")
.WithTags("DetallesMatricula")
.Produces<DetalleMatricula>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

apiGroup.MapPut("/detalles-matricula/{id}", async (int id, DetalleMatricula detalle, ApplicationDbContext db) =>
{
    if (id != detalle.Id)
    {
        return Results.BadRequest("El ID no coincide");
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(detalle);
    if (!Validator.TryValidateObject(detalle, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    var existingDetalle = await db.DetallesMatricula.FindAsync(id);
    if (existingDetalle == null)
    {
        return Results.NotFound();
    }

    // Verificar que la matrícula existe
    if (!await db.Matriculas.AnyAsync(m => m.Id == detalle.IdMatricula))
    {
        return Results.BadRequest("La matrícula no existe");
    }

    // Verificar que el grupo existe
    if (!await db.Grupos.AnyAsync(g => g.Id == detalle.IdGrupo))
    {
        return Results.BadRequest("El grupo no existe");
    }

    db.Entry(existingDetalle).CurrentValues.SetValues(detalle);
    await db.SaveChangesAsync();
    return Results.Ok(existingDetalle);
})
.WithName("UpdateDetalleMatricula")
.WithTags("DetallesMatricula")
.Produces<DetalleMatricula>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

apiGroup.MapDelete("/detalles-matricula/{id}", async (int id, ApplicationDbContext db) =>
{
    var detalle = await db.DetallesMatricula.FindAsync(id);
    if (detalle == null)
    {
        return Results.NotFound();
    }

    db.DetallesMatricula.Remove(detalle);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteDetalleMatricula")
.WithTags("DetallesMatricula")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
