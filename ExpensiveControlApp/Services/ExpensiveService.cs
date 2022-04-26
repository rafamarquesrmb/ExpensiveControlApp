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
            await _dbContext.Expensives.AddAsync(new Expensive
            {
                Description = createExpensiveDTO.Description,
                Value = createExpensiveDTO.Value,
                Date = createExpensiveDTO.Date
            });
            await _dbContext.SaveChangesAsync();

        }

        public async Task<List<Expensive>> FindByDate(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Data inicial não pode ser maior que a data final");
            }
            var items = await _dbContext.Expensives.Where(e => e.Date >= startDate && e.Date <= endDate).AsNoTracking().ToListAsync();
            return items;
        }

        public async Task<Expensive> FindById(int? id)
        {
            if (!id.HasValue)
                throw new Exception("É necessário um ID");
            var item = await _dbContext.Expensives.Where(e => e.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (item == null)
                throw new Exception("Não encontrado");
            return item;
        }

        public async Task Update(DTOs.UpdateExpensiveDTO updateExpensiveDTO)
        {
            if (updateExpensiveDTO == null)
                throw new Exception("Parametros necessários");
            Expensive expensiveToUpdate = await FindById(updateExpensiveDTO.Id);
            if (expensiveToUpdate == null)
            {
                throw new Exception("Inexistente");
            }
            if (updateExpensiveDTO.Date != null && updateExpensiveDTO.Date != expensiveToUpdate.Date)
                expensiveToUpdate.Date = updateExpensiveDTO.Date;
            if (updateExpensiveDTO.Value != null && updateExpensiveDTO.Value != expensiveToUpdate.Value)
                expensiveToUpdate.Value = updateExpensiveDTO.Value;
            if (updateExpensiveDTO.Description != null && updateExpensiveDTO.Description != expensiveToUpdate.Description)
                expensiveToUpdate.Description = updateExpensiveDTO.Description;

            try
            {
                _dbContext.Expensives.Update(expensiveToUpdate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Falha ao atualizar");
            }
            


        }

        public async Task Delete(int? id)
        {
            if (id == null)
                throw new Exception("ID necessário");

            Expensive expensive = await FindById(id);
            if (expensive == null)
            {
                throw new Exception("Inexistente");
            }
            try
            {
                _dbContext.Expensives.Remove(expensive);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Falha ao Deletar");
            }



        }



    }
}
