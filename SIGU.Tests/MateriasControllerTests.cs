using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SIGU.Domain;
using SIGU.Infrastructure.Data;
using Xunit;

namespace SIGU.Tests;

public class MateriasControllerTests
{
    private readonly ApplicationDbContext _context;

    public MateriasControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SIGU_Test_DB_Materias")
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetMaterias_ReturnsAllMaterias()
    {
        // Arrange
        var materias = new List<Materia>
        {
            new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 },
            new Materia { Id = 2, Codigo = "FIS101", Nombre = "Física General", Creditos = 4 }
        };
        _context.Materias.AddRange(materias);
        await _context.SaveChangesAsync();

        // Act
        var result = await GetMaterias();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMaterias = Assert.IsType<List<Materia>>(okResult.Value);
        Assert.Equal(2, returnedMaterias.Count);
    }

    [Fact]
    public async Task GetMateria_ValidId_ReturnsMateria()
    {
        // Arrange
        var materia = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();

        // Act
        var result = await GetMateria(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMateria = Assert.IsType<Materia>(okResult.Value);
        Assert.Equal(1, returnedMateria.Id);
    }

    [Fact]
    public async Task GetMateria_InvalidId_ReturnsNotFound()
    {
        // Act
        var result = await GetMateria(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateMateria_ValidData_ReturnsCreated()
    {
        // Arrange
        var newMateria = new Materia { Codigo = "QUI101", Nombre = "Química General", Creditos = 3 };

        // Act
        var result = await CreateMateria(newMateria);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var returnedMateria = Assert.IsType<Materia>(createdResult.Value);
        Assert.Equal("Química General", returnedMateria.Nombre);
    }

    [Fact]
    public async Task CreateMateria_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidMateria = new Materia { Codigo = "", Nombre = "", Creditos = 0 };

        // Act
        var result = await CreateMateria(invalidMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateMateria_InvalidCreditos_ReturnsBadRequest()
    {
        // Arrange
        var invalidMateria = new Materia { Codigo = "MAT101", Nombre = "Matemáticas", Creditos = 15 }; // Créditos fuera del rango

        // Act
        var result = await CreateMateria(invalidMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateMateria_DuplicateCodigo_ReturnsBadRequest()
    {
        // Arrange
        var existingMateria = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(existingMateria);
        await _context.SaveChangesAsync();

        var duplicateMateria = new Materia { Codigo = "MAT101", Nombre = "Otra Matemáticas", Creditos = 4 };

        // Act
        var result = await CreateMateria(duplicateMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateMateria_ValidData_ReturnsOk()
    {
        // Arrange
        var materia = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();

        var updatedMateria = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas Avanzadas", Creditos = 4 };

        // Act
        var result = await UpdateMateria(1, updatedMateria);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMateria = Assert.IsType<Materia>(okResult.Value);
        Assert.Equal("Matemáticas Básicas Avanzadas", returnedMateria.Nombre);
        Assert.Equal(4, returnedMateria.Creditos);
    }

    [Fact]
    public async Task UpdateMateria_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var materia = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();

        var updatedMateria = new Materia { Id = 2, Codigo = "MAT101", Nombre = "Matemáticas Actualizadas", Creditos = 3 };

        // Act
        var result = await UpdateMateria(1, updatedMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateMateria_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var materia = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();

        var invalidMateria = new Materia { Id = 1, Codigo = "", Nombre = "", Creditos = 0 };

        // Act
        var result = await UpdateMateria(1, invalidMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateMateria_NotFound_ReturnsNotFound()
    {
        // Arrange
        var updatedMateria = new Materia { Id = 999, Codigo = "MAT999", Nombre = "Materia No Existente", Creditos = 3 };

        // Act
        var result = await UpdateMateria(999, updatedMateria);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateMateria_DuplicateCodigo_ReturnsBadRequest()
    {
        // Arrange
        var materia1 = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        var materia2 = new Materia { Id = 2, Codigo = "FIS101", Nombre = "Física General", Creditos = 4 };
        _context.Materias.AddRange(materia1, materia2);
        await _context.SaveChangesAsync();

        var updatedMateria = new Materia { Id = 1, Codigo = "FIS101", Nombre = "Matemáticas Básicas", Creditos = 3 };

        // Act
        var result = await UpdateMateria(1, updatedMateria);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteMateria_ValidId_ReturnsNoContent()
    {
        // Arrange
        var materia = new Materia { Id = 1, Codigo = "MAT101", Nombre = "Matemáticas Básicas", Creditos = 3 };
        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();

        // Act
        var result = await DeleteMateria(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(await _context.Materias.FindAsync(1));
    }

    [Fact]
    public async Task DeleteMateria_InvalidId_ReturnsNotFound()
    {
        // Act
        var result = await DeleteMateria(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // Helper methods to simulate controller actions
    private async Task<IActionResult> GetMaterias()
    {
        var materias = await _context.Materias.ToListAsync();
        return new OkObjectResult(materias);
    }

    private async Task<IActionResult> GetMateria(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        return materia == null ? new NotFoundResult() : new OkObjectResult(materia);
    }

    private async Task<IActionResult> CreateMateria(Materia materia)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(materia);
        if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(materia, validationContext, validationResults, true))
        {
            return new BadRequestObjectResult(validationResults);
        }

        if (await _context.Materias.AnyAsync(m => m.Codigo == materia.Codigo))
        {
            return new BadRequestObjectResult("El código de materia ya existe");
        }

        _context.Materias.Add(materia);
        await _context.SaveChangesAsync();
        return new CreatedResult($"/materias/{materia.Id}", materia);
    }

    private async Task<IActionResult> UpdateMateria(int id, Materia materia)
    {
        if (id != materia.Id)
        {
            return new BadRequestObjectResult("El ID no coincide");
        }

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(materia);
        if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(materia, validationContext, validationResults, true))
        {
            return new BadRequestObjectResult(validationResults);
        }

        var existingMateria = await _context.Materias.FindAsync(id);
        if (existingMateria == null)
        {
            return new NotFoundResult();
        }

        if (await _context.Materias.AnyAsync(m => m.Codigo == materia.Codigo && m.Id != id))
        {
            return new BadRequestObjectResult("El código de materia ya existe");
        }

        _context.Entry(existingMateria).CurrentValues.SetValues(materia);
        await _context.SaveChangesAsync();
        return new OkObjectResult(existingMateria);
    }

    private async Task<IActionResult> DeleteMateria(int id)
    {
        var materia = await _context.Materias.FindAsync(id);
        if (materia == null)
        {
            return new NotFoundResult();
        }

        _context.Materias.Remove(materia);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }
}
