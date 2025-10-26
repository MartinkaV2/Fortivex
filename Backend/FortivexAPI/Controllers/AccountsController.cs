using FortivexAPI.Data;
using FortivexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using FortivexAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FortivexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FortivexContext _context;

        public AccountsController(FortivexContext context)
        {
            _context = context;
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
    }
}
