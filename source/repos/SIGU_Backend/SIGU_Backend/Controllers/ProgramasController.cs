using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using System.Threading.Tasks;

namespace SIGU_Backend.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class ProgramasController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public ProgramasController(ApplicationDbContext db) => _db = db;

 [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Programas.ToListAsync());
 [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _db.Programas.FindAsync(id));
 [HttpPost] public async Task<IActionResult> Post(Programa p) { _db.Programas.Add(p); await _db.SaveChangesAsync(); return Ok(p); }
 [HttpPut("{id}")] public async Task<IActionResult> Put(int id, Programa p) { _db.Entry(p).State = EntityState.Modified; await _db.SaveChangesAsync(); return Ok(); }
 [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var p = await _db.Programas.FindAsync(id); if (p != null) { _db.Programas.Remove(p); await _db.SaveChangesAsync(); } return Ok(); }
 }
}
