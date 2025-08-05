using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class OvertimeRepository(AppDbContext appDbContext) : IGenericRepository<Overtime>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.Overtimes.FirstOrDefaultAsync(o => o.EmployeeId == id);
            if (item is null) return NotFound();

            appDbContext.Overtimes.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<List<Overtime>> GetAll() => await appDbContext.Overtimes.AsNoTracking().ToListAsync();

        public async Task<Overtime?> GetById(int id) => await appDbContext.Overtimes.AsNoTracking().Include(t => t.OvertimeType).FirstOrDefaultAsync(o => o.EmployeeId == id);

        public async Task<GeneralResponse> Insert(Overtime entity)
        {
            appDbContext.Overtimes.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Overtime entity)
        {
            var item = await appDbContext.Overtimes.FirstOrDefaultAsync(o => o.EmployeeId == entity.EmployeeId);
            if (item is null) return NotFound();
            item.StartDate = entity.StartDate;
            item.EndDate = entity.EndDate;
            item.OvertimeTypeId = entity.OvertimeTypeId;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry data not found");
        private static GeneralResponse Success() => new(true, "Operation completed successfully");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
