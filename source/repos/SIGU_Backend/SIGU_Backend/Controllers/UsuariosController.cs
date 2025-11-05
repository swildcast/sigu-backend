using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGU_Backend.Data;
using SIGU_Backend.Models;
using System.Threading.Tasks;

namespace SIGU_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UsuariosController(ApplicationDbContext db) => _db = db;

        [HttpGet] public async Task<IActionResult> Get() => Ok(await _db.Usuarios.ToListAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _db.Usuarios.FindAsync(id));
        [HttpPost] public async Task<IActionResult> Post(Usuario u) { _db.Usuarios.Add(u); await _db.SaveChangesAsync(); return Ok(u); }
        [HttpPut("{id}")] public async Task<IActionResult> Put(int id, Usuario u) { _db.Entry(u).State = EntityState.Modified; await _db.SaveChangesAsync(); return Ok(); }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) { var u = await _db.Usuarios.FindAsync(id); if (u != null) { _db.Usuarios.Remove(u); await _db.SaveChangesAsync(); } return Ok(); }
    }
}