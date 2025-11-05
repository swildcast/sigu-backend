using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using System.Threading.Tasks;


namespace SIGU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public InscripcionController(ApplicationDbContext db) => _db = db;

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Inscripciones.ToListAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _db.Inscripciones.FindAsync(id));
        [HttpPost] public async Task<IActionResult> Post(Inscripcion i) { _db.Inscripciones.Add(i); await _db.SaveChangesAsync(); return Ok(i); }
        [HttpPut("{id}")] public async Task<IActionResult> Put(int id, Inscripcion i) { _db.Entry(i).State = EntityState.Modified; await _db.SaveChangesAsync(); return Ok(); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var i = await _db.Inscripciones.FindAsync(id); if (i != null) { _db.Inscripciones.Remove(i); await _db.SaveChangesAsync(); } return Ok(); }
    }
}