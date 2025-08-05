using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class VacationTypeRepository(AppDbContext appDbContext) : IGenericRepository<VacationType>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.VacationTypes.FirstOrDefaultAsync(o => o.Id == id);
            if (item is null) return NotFound();
            appDbContext.VacationTypes.Remove(item);

            await Commit();
            return Success();
        }

        public async Task<List<VacationType>> GetAll() => await appDbContext.VacationTypes.AsNoTracking().ToListAsync();

        public async Task<VacationType?> GetById(int id) => await appDbContext.VacationTypes.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<GeneralResponse> Insert(VacationType entity)
        {
            if (!await CheckName(entity.Name))
                return new GeneralResponse(false, "Vacation type with this name already exists");

            appDbContext.VacationTypes.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(VacationType entity)
        {
            var item = await appDbContext.VacationTypes.FirstOrDefaultAsync(o => o.Id == entity.Id);
            if (item is null) return NotFound();
            item.Name = entity.Name;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry data not found");
        private static GeneralResponse Success() => new(true, "Operation completed successfully");
        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.VacationTypes.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
