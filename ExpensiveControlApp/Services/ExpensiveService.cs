using ExpensiveControlApp.Infra.Database;
using ExpensiveControlApp.Models.Expensives;
using Microsoft.EntityFrameworkCore;

namespace ExpensiveControlApp.Services
{
    public class ExpensiveService : IExpensiveService
    {
        private readonly ExpensiveControlContext _dbContext;
        public ExpensiveService(ExpensiveControlContext context)
        {
            _dbContext = context;
        }

        public async Task Create(DTOs.CreateExpensiveDTO createExpensiveDTO)
        {
            await _dbContext.Expensives.AddAsync(new Expensive{
                Description = createExpensiveDTO.Description,
                Value = createExpensiveDTO.Value,
                Date = createExpensiveDTO.Date
            });
            await _dbContext.SaveChangesAsync();

        }

        public async Task<List<Expensive>> FindBy(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Data inicial não pode ser maior que a data final");
            }
            var items = await _dbContext.Expensives.Where(e => e.Date >= startDate && e.Date <= endDate).AsNoTracking().ToListAsync();
            return items;
        }

    }
}
