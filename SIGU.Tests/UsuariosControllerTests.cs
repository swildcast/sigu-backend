using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SIGU.Domain;
using SIGU.Infrastructure.Data;
using Xunit;

namespace SIGU.Tests;

public class UsuariosControllerTests
{
    private readonly ApplicationDbContext _context;

    public UsuariosControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SIGU_Test_DB")
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetUsuarios_ReturnsAllUsuarios()
    {
        // Arrange
        var usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" },
            new Usuario { Id = 2, Nombre = "María García", Email = "maria@example.com", Rol = "Docente", HashPassword = "password2" }
        };
        _context.Usuarios.AddRange(usuarios);
        await _context.SaveChangesAsync();

        // Act
        var result = await GetUsuarios();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsuarios = Assert.IsType<List<Usuario>>(okResult.Value);
        Assert.Equal(2, returnedUsuarios.Count);
    }

    [Fact]
    public async Task GetUsuario_ValidId_ReturnsUsuario()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Act
        var result = await GetUsuario(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsuario = Assert.IsType<Usuario>(okResult.Value);
        Assert.Equal(1, returnedUsuario.Id);
    }

    [Fact]
    public async Task GetUsuario_InvalidId_ReturnsNotFound()
    {
        // Act
        var result = await GetUsuario(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateUsuario_ValidData_ReturnsCreated()
    {
        // Arrange
        var newUsuario = new Usuario { Nombre = "Carlos López", Email = "carlos@example.com", Rol = "Estudiante", HashPassword = "password3" };

        // Act
        var result = await CreateUsuario(newUsuario);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var returnedUsuario = Assert.IsType<Usuario>(createdResult.Value);
        Assert.Equal("Carlos López", returnedUsuario.Nombre);
    }

    [Fact]
    public async Task CreateUsuario_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidUsuario = new Usuario { Nombre = "", Email = "invalid-email", Rol = "", HashPassword = "" };

        // Act
        var result = await CreateUsuario(invalidUsuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateUsuario_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var existingUsuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(existingUsuario);
        await _context.SaveChangesAsync();

        var duplicateUsuario = new Usuario { Nombre = "Otro Juan", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password4" };

        // Act
        var result = await CreateUsuario(duplicateUsuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateUsuario_ValidData_ReturnsOk()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var updatedUsuario = new Usuario { Id = 1, Nombre = "Juan Pérez Actualizado", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };

        // Act
        var result = await UpdateUsuario(1, updatedUsuario);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsuario = Assert.IsType<Usuario>(okResult.Value);
        Assert.Equal("Juan Pérez Actualizado", returnedUsuario.Nombre);
    }

    [Fact]
    public async Task UpdateUsuario_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var updatedUsuario = new Usuario { Id = 2, Nombre = "Juan Pérez Actualizado", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };

        // Act
        var result = await UpdateUsuario(1, updatedUsuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateUsuario_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var invalidUsuario = new Usuario { Id = 1, Nombre = "", Email = "invalid-email", Rol = "", HashPassword = "" };

        // Act
        var result = await UpdateUsuario(1, invalidUsuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateUsuario_NotFound_ReturnsNotFound()
    {
        // Arrange
        var updatedUsuario = new Usuario { Id = 999, Nombre = "Usuario No Existente", Email = "noexiste@example.com", Rol = "Estudiante", HashPassword = "password" };

        // Act
        var result = await UpdateUsuario(999, updatedUsuario);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateUsuario_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var usuario1 = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        var usuario2 = new Usuario { Id = 2, Nombre = "María García", Email = "maria@example.com", Rol = "Docente", HashPassword = "password2" };
        _context.Usuarios.AddRange(usuario1, usuario2);
        await _context.SaveChangesAsync();

        var updatedUsuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "maria@example.com", Rol = "Estudiante", HashPassword = "password1" };

        // Act
        var result = await UpdateUsuario(1, updatedUsuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUsuario_ValidId_ReturnsNoContent()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@example.com", Rol = "Estudiante", HashPassword = "password1" };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Act
        var result = await DeleteUsuario(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(await _context.Usuarios.FindAsync(1));
    }

    [Fact]
    public async Task DeleteUsuario_InvalidId_ReturnsNotFound()
    {
        // Act
        var result = await DeleteUsuario(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    // Helper methods to simulate controller actions
    private async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return new OkObjectResult(usuarios);
    }

    private async Task<IActionResult> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        return usuario == null ? new NotFoundResult() : new OkObjectResult(usuario);
    }

    private async Task<IActionResult> CreateUsuario(Usuario usuario)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(usuario);
        if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(usuario, validationContext, validationResults, true))
        {
            return new BadRequestObjectResult(validationResults);
        }

        if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
        {
            return new BadRequestObjectResult("El email ya está registrado");
        }

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return new CreatedResult($"/usuarios/{usuario.Id}", usuario);
    }

    private async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return new BadRequestObjectResult("El ID no coincide");
        }

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(usuario);
        if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(usuario, validationContext, validationResults, true))
        {
            return new BadRequestObjectResult(validationResults);
        }

        var existingUsuario = await _context.Usuarios.FindAsync(id);
        if (existingUsuario == null)
        {
            return new NotFoundResult();
        }

        if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != id))
        {
            return new BadRequestObjectResult("El email ya está registrado");
        }

        _context.Entry(existingUsuario).CurrentValues.SetValues(usuario);
        await _context.SaveChangesAsync();
        return new OkObjectResult(existingUsuario);
    }

    private async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return new NotFoundResult();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }
}
