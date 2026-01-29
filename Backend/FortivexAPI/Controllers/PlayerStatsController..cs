using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortivexAPI.Data;
using FortivexAPI.Models;
using FortivexAPI.Dtos;

namespace FortivexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatsController : ControllerBase
    {
        private readonly FortivexContext _context;

        public PlayerStatsController(FortivexContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStatsDto>>> GetStats()
        {
            var stats = await _context.PlayerStats
                .Select(s => new PlayerStatsDto
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    EnemiesKilled = s.EnemiesKilled,
                    TimePlayed = s.TimePlayed
                }).ToListAsync();

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerStatsDto>> GetStatsById(int id)
        {
            var s = await _context.PlayerStats.FindAsync(id);
            if (s == null)
                return NotFound();

            return new PlayerStatsDto
            {
                Id = s.Id,
                AccountId = s.AccountId,
                EnemiesKilled = s.EnemiesKilled,
                TimePlayed = s.TimePlayed
            };
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStats(UpdatePlayerStatsDto dto)
        {
            var stats = await _context.PlayerStats
                .FirstOrDefaultAsync(x => x.AccountId == dto.AccountId);

            if (stats == null)
            {
                stats = new PlayerStat
                {
                    AccountId = dto.AccountId,
                    EnemiesKilled = dto.EnemiesKilled,
                    TimePlayed = dto.TimePlayed
                };
                _context.PlayerStats.Add(stats);
            }
            else
            {
                stats.EnemiesKilled += dto.EnemiesKilled;
                stats.TimePlayed += dto.TimePlayed;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStats(int id, PlayerStatsDto dto)
        {
            var existing = await _context.PlayerStats.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.EnemiesKilled = dto.EnemiesKilled;
            existing.TimePlayed = dto.TimePlayed;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStats(int id)
        {
            var existing = await _context.PlayerStats.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.PlayerStats.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
