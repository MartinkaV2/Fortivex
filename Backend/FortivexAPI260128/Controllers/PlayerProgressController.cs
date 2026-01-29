using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortivexAPI.Data;
using FortivexAPI.Models;
using FortivexAPI.Dtos;

namespace FortivexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerProgressController : ControllerBase
    {
        private readonly FortivexContext _context;

        public PlayerProgressController(FortivexContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerProgressDto>>> GetProgress()
        {
            var progress = await _context.PlayerProgress
                .Select(p => new PlayerProgressDto
                {
                    Id = p.Id,
                    AccountId = p.AccountId,
                    BestTime = p.BestTime
                }).ToListAsync();

            return Ok(progress);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerProgressDto>> GetProgressById(int id)
        {
            var p = await _context.PlayerProgress.FindAsync(id);
            if (p == null)
                return NotFound();

            return new PlayerProgressDto
            {
                Id = p.Id,
                AccountId = p.AccountId,
                BestTime = p.BestTime
            };
        }

        [HttpPost]
        public async Task<ActionResult<PlayerProgressDto>> CreateProgress(PlayerProgressDto dto)
        {
            var newP = new PlayerProgress
            {
                AccountId = dto.AccountId,
                BestTime = dto.BestTime
            };

            _context.PlayerProgress.Add(newP);
            await _context.SaveChangesAsync();

            dto.Id = newP.Id;
            return CreatedAtAction(nameof(GetProgressById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgress(int id, PlayerProgressDto dto)
        {
            var existing = await _context.PlayerProgress.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.BestTime = dto.BestTime;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgress(int id)
        {
            var existing = await _context.PlayerProgress.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.PlayerProgress.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
