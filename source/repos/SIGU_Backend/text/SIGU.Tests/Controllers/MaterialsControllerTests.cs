using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Controllers;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using Xunit;

namespace SIGU.Tests.Controllers
{
 public class MaterialsControllerTests
 {
 private ApplicationDbContext CreateContext(string dbName)
 {
 var options = new DbContextOptionsBuilder<ApplicationDbContext>()
 .UseInMemoryDatabase(dbName)
 .Options;
 return new ApplicationDbContext(options);
 }

 [Fact]
 public async Task Get_ReturnsSeeded_Materias()
 {
 await using var db = CreateContext("mat_get_seed");
 db.Materias.AddRange(new Materia { Codigo = "A1", Nombre = "Test1", Creditos =1 }, new Materia { Codigo = "B2", Nombre = "Test2", Creditos =2 });
 await db.SaveChangesAsync();

 var controller = new MateriasController(db);
 var result = await controller.Get();
 var ok = Assert.IsType<OkObjectResult>(result);
 var list = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Materia>>(ok.Value);
 Assert.Equal(2, System.Linq.Enumerable.Count(list));
 }

 [Fact]
 public async Task Post_AddsMateria()
 {
 await using var db = CreateContext("mat_post");
 var controller = new MateriasController(db);
 var m = new Materia { Codigo = "C3", Nombre = "New", Creditos =3 };
 var res = await controller.Post(m);
 var ok = Assert.IsType<OkObjectResult>(res);
 var returned = Assert.IsType<Materia>(ok.Value);
 Assert.Equal(m.Codigo, returned.Codigo);
 Assert.Equal(1, db.Materias.Count());
 }

 [Fact]
 public async Task Put_UpdatesMateria()
 {
 await using var db = CreateContext("mat_put");
 var m = new Materia { Codigo = "D4", Nombre = "Old", Creditos =4 };
 db.Materias.Add(m);
 await db.SaveChangesAsync();

 var controller = new MateriasController(db);
 m.Nombre = "Updated";
 var res = await controller.Put(m.Id, m);
 Assert.IsType<OkResult>(res);
 var updated = db.Materias.Find(m.Id);
 Assert.Equal("Updated", updated.Nombre);
 }

 [Fact]
 public async Task Delete_RemovesMateria()
 {
 await using var db = CreateContext("mat_delete");
 var m = new Materia { Codigo = "E5", Nombre = "ToDelete", Creditos =5 };
 db.Materias.Add(m);
 await db.SaveChangesAsync();

 var controller = new MateriasController(db);
 var res = await controller.Delete(m.Id);
 Assert.IsType<OkResult>(res);
 Assert.False(db.Materias.Any(x => x.Id == m.Id));
 }
 }
}
