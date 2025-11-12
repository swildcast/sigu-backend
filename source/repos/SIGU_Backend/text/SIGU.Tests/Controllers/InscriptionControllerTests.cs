using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SIGU_Backend.Controllers;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using Xunit;

namespace SIGU.Tests.Controllers
{
 public class InscriptionControllerTests
 {
 private ApplicationDbContext CreateContext(string dbName)
 {
 var options = new DbContextOptionsBuilder<ApplicationDbContext>()
 .UseInMemoryDatabase(dbName)
 .Options;
 return new ApplicationDbContext(options);
 }

 [Fact]
 public async Task Get_ReturnsEmpty_WhenNoInscripciones()
 {
 await using var db = CreateContext("insc_get_empty");
 var controller = new InscripcionController(db);

 var result = await controller.Get();

 var ok = Assert.IsType<OkObjectResult>(result);
 var list = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Inscripcion>>(ok.Value);
 Assert.Empty(list);
 }

 [Fact]
 public async Task Post_AddsInscripcion()
 {
 await using var db = CreateContext("insc_post_add");
 var controller = new InscripcionController(db);

 var insc = new Inscripcion { EstudianteId =1, MateriaId =2, Fecha = DateTime.UtcNow };
 var postResult = await controller.Post(insc);

 var ok = Assert.IsType<OkObjectResult>(postResult);
 var returned = Assert.IsType<Inscripcion>(ok.Value);
 Assert.Equal(insc.EstudianteId, returned.EstudianteId);

 // Verify persisted
 var saved = db.Inscripciones.FirstOrDefault();
 Assert.NotNull(saved);
 Assert.Equal(insc.MateriaId, saved.MateriaId);
 }

 [Fact]
 public async Task GetById_ReturnsInscripcion_WhenExists()
 {
 await using var db = CreateContext("insc_get_by_id");
 var insc = new Inscripcion { EstudianteId =3, MateriaId =4, Fecha = DateTime.UtcNow };
 db.Inscripciones.Add(insc);
 await db.SaveChangesAsync();

 var controller = new InscripcionController(db);
 var result = await controller.Get(insc.Id);

 var ok = Assert.IsType<OkObjectResult>(result);
 var returned = Assert.IsType<Inscripcion>(ok.Value);
 Assert.Equal(insc.Id, returned.Id);
 }

 [Fact]
 public async Task Put_UpdatesInscripcion()
 {
 await using var db = CreateContext("insc_put");
 var insc = new Inscripcion { EstudianteId =5, MateriaId =6, Fecha = DateTime.UtcNow };
 db.Inscripciones.Add(insc);
 await db.SaveChangesAsync();

 var controller = new InscripcionController(db);
 insc.MateriaId =99;
 var putResult = await controller.Put(insc.Id, insc);

 Assert.IsType<OkResult>(putResult);
 var updated = db.Inscripciones.Find(insc.Id);
 Assert.Equal(99, updated.MateriaId);
 }

 [Fact]
 public async Task Delete_RemovesInscripcion()
 {
 await using var db = CreateContext("insc_delete");
 var insc = new Inscripcion { EstudianteId =7, MateriaId =8, Fecha = DateTime.UtcNow };
 db.Inscripciones.Add(insc);
 await db.SaveChangesAsync();

 var controller = new InscripcionController(db);
 var delResult = await controller.Delete(insc.Id);

 Assert.IsType<OkResult>(delResult);
 Assert.False(db.Inscripciones.Any(i => i.Id == insc.Id));
 }
 }
}
