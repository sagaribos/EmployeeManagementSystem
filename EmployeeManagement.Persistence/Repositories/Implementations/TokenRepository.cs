using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;

    public TokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BlacklistedToken token)
    {
        await _context.BlacklistedTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsBlacklistedAsync(string token)
        => await _context.BlacklistedTokens
            .AnyAsync(t => t.Token == token);
}