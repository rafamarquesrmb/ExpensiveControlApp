using ExpensiveControlApp.Models.Expensives;

namespace ExpensiveControlApp.Services
{
    public interface IExpensiveService
    {
        Task Create(DTOs.CreateExpensiveDTO createExpensiveDTO);
        Task<List<Expensive>> FindByDate(DateTime startDate, DateTime endDate);
        Task<Expensive> FindById(int? id);

        Task Update(DTOs.UpdateExpensiveDTO updateExpensiveDTO);
        Task Delete(int? id);

    }
}
