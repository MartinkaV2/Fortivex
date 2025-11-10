using FortivexAPI.Data;
using FortivexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using FortivexAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using FortivexAPI.Helpers;

namespace FortivexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FortivexContext _context;
        private readonly JwtHelper _jwt;

        public AccountsController(FortivexContext context, JwtHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        // GET: api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            var accounts = await _context.Accounts
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Username = a.Username,
                    Email = a.Email
                }).ToListAsync();

            return Ok(accounts);
        }

        // GET: api/accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return new AccountDto
            {
                Id = account.Id,
                Username = account.Username,
                Email = account.Email
            };
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount(AccountDto dto)
        {
            var newAccount = new Account
            {
                Username = dto.Username,
                PasswordHash = "default123",
                Email = dto.Email,
                CreatedAt = DateTime.Now
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            dto.Id = newAccount.Id;
            return CreatedAtAction(nameof(GetAccount), new { id = dto.Id }, dto);
        }

        // PUT: api/accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDto dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            account.Username = dto.Username;
            account.PasswordHash = dto.PasswordHash;
            account.Email = dto.Email;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // ✅ LOGIN: POST /api/accounts/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == dto.UserName && a.PasswordHash == dto.PasswordHash);

            if (account == null)
                return Unauthorized("Hibás felhasználónév vagy jelszó.");

            // Szerepkör meghatározása
            var isAdmin = await _context.Admins.AnyAsync(a => a.AccountId == account.Id);
            var role = isAdmin ? "admin" : "user";

            // JWT token generálás
            
            var token = _jwt.GenerateToken(account.Username, role);

            return Ok(new
            {
                token,
                role,
                username = account.Username
            });
        }

        // ✅ REGISTER: POST /api/accounts/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (await _context.Accounts.AnyAsync(a => a.Username == dto.UserName))
                return BadRequest("A felhasználónév már foglalt.");

            var account = new Account
            {
                Username = dto.UserName,
                PasswordHash = dto.PasswordHash,
                Email = dto.Email,
                CreatedAt = DateTime.Now
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Ha szerepe admin, itt adjuk hozzá az admins táblába
            if (dto.Role.ToLower() == "admin")
            {
                _context.Admins.Add(new Admin
                {
                    AccountId = account.Id,
                    Role = "admin",
                    AssignedAt = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            return Ok("Sikeres regisztráció!");
        }


    }
}
