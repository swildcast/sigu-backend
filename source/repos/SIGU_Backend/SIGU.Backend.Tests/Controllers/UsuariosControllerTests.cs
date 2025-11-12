using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Controllers;
using SIGU_Backend.Models;
using SIGU_Backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SIGU.Backend.Tests.Controllers
{
    public class UsuariosControllerTests
    {
        private readonly Mock<DbSet<Usuario>> _mockSet;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly UsuariosController _controller;

        public UsuariosControllerTests()
        {
            // Configurar datos de prueba
            var data = new List<Usuario>
            {
                new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@email.com", Rol = "Estudiante" },
                new Usuario { Id = 2, Nombre = "María García", Email = "maria@email.com", Rol = "Profesor" }
            }.AsQueryable();

            // Configurar mock de DbSet
            _mockSet = new Mock<DbSet<Usuario>>();
            _mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Configurar mock de Context
            _mockContext = new Mock<ApplicationDbContext>();
            _mockContext.Setup(c => c.Usuarios).Returns(_mockSet.Object);

            // Crear controller
            _controller = new UsuariosController(_mockContext.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllUsuarios()
        {
            // Act
            var result = await _controller.Get();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var usuarios = Assert.IsType<List<Usuario>>(actionResult.Value);
            Assert.Equal(2, usuarios.Count);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsUsuario()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@email.com" };
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(usuario);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsuario = Assert.IsType<Usuario>(actionResult.Value);
            Assert.Equal(1, returnedUsuario.Id);
            Assert.Equal("Juan Pérez", returnedUsuario.Nombre);
        }

        [Fact]
        public async Task Post_ValidUsuario_ReturnsOkWithUsuario()
        {
            // Arrange
            var nuevoUsuario = new Usuario { Nombre = "Carlos López", Email = "carlos@email.com", Rol = "Estudiante" };
            _mockSet.Setup(m => m.Add(It.IsAny<Usuario>())).Callback<Usuario>(u => u.Id = 3);

            // Act
            var result = await _controller.Post(nuevoUsuario);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsuario = Assert.IsType<Usuario>(actionResult.Value);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Put_ValidUsuario_ReturnsOk()
        {
            // Arrange
            var usuarioActualizado = new Usuario { Id = 1, Nombre = "Juan Pérez Actualizado", Email = "juan@email.com" };

            // Act
            var result = await _controller.Put(1, usuarioActualizado);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingUsuario_ReturnsOk()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Nombre = "Juan Pérez", Email = "juan@email.com" };
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(usuario);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockSet.Verify(m => m.Remove(usuario), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingUsuario_ReturnsOk()
        {
            // Arrange
            _mockSet.Setup(m => m.FindAsync(999)).ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockSet.Verify(m => m.Remove(It.IsAny<Usuario>()), Times.Never);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }
}