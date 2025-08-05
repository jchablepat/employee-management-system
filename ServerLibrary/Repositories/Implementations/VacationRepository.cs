using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class VacationRepository(AppDbContext appDbContext) : IGenericRepository<Vacation>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.Vacations.FirstOrDefaultAsync(v => v.EmployeeId == id);
            if (item is null) return NotFound();
            appDbContext.Vacations.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<List<Vacation>> GetAll() => await appDbContext.Vacations.AsNoTracking().Include(t => t.VacationType).ToListAsync();

        public async Task<Vacation?> GetById(int id) => await appDbContext.Vacations.FirstOrDefaultAsync(v => v.EmployeeId == id);

        public async Task<GeneralResponse> Insert(Vacation entity)
        {
            appDbContext.Vacations.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Vacation entity)
        {
            var item = await appDbContext.Vacations.FirstOrDefaultAsync(v => v.EmployeeId == entity.EmployeeId);
            if (item is null) return NotFound();
            item.StartDate = entity.StartDate;
            item.NumberOfDays = entity.NumberOfDays;
            item.VacationTypeId = entity.VacationTypeId;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry data not found");
        private static GeneralResponse Success() => new(true, "Operation completed successfully");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
