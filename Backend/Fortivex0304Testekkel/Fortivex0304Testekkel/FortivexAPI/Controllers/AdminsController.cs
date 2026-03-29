using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortivexAPI.Data;
using FortivexAPI.Models;
using FortivexAPI.Dtos;

namespace FortivexAPI.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly FortivexContext _context;

        public AdminsController(FortivexContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAdmins()
        {
            var admins = await _context.Admins
                .Select(a => new AdminDto
                {
                    Id = a.Id,
                    AccountId = a.AccountId,
                    Role = a.Role
                }).ToListAsync();

            return Ok(admins);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return NotFound();

            return new AdminDto
            {
                Id = admin.Id,
                AccountId = admin.AccountId,
                Role = admin.Role
            };
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> CreateAdmin(AdminDto dto)
        {
            var newAdmin = new Admin
            {
                AccountId = dto.AccountId,
                Role = dto.Role,
                AssignedAt = DateTime.Now
            };

            _context.Admins.Add(newAdmin);
            await _context.SaveChangesAsync();

            dto.Id = newAdmin.Id;
            return CreatedAtAction(nameof(GetAdmin), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
