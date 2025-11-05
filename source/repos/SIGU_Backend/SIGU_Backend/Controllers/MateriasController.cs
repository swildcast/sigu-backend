using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using System.Threading.Tasks;

namespace SIGU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriasController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MateriasController(ApplicationDbContext db) => _db = db;

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Materias.ToListAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _db.Materias.FindAsync(id));
        [HttpPost] public async Task<IActionResult> Post(Materia m) { _db.Materias.Add(m); await _db.SaveChangesAsync(); return Ok(m); }
        [HttpPut("{id}")] public async Task<IActionResult> Put(int id, Materia m) { _db.Entry(m).State = EntityState.Modified; await _db.SaveChangesAsync(); return Ok(); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var m = await _db.Materias.FindAsync(id); if (m != null) { _db.Materias.Remove(m); await _db.SaveChangesAsync(); } return Ok(); }
    }
}