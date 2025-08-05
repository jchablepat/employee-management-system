using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class SanctionTypeRepository(AppDbContext appDbContext) : IGenericRepository<SanctionType>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.SanctionTypes.FirstOrDefaultAsync(o => o.Id == id);
            if (item is null) return NotFound();
            appDbContext.SanctionTypes.Remove(item);

            await Commit();
            return Success();
        }

        public async Task<List<SanctionType>> GetAll() => await appDbContext.SanctionTypes.AsNoTracking().ToListAsync();

        public async Task<SanctionType?> GetById(int id) => await appDbContext.SanctionTypes.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<GeneralResponse> Insert(SanctionType entity)
        {
            if (!await CheckName(entity.Name))
                return new GeneralResponse(false, "Sanction type with this name already exists");

            appDbContext.SanctionTypes.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(SanctionType entity)
        {
            var item = await appDbContext.SanctionTypes.FirstOrDefaultAsync(o => o.Id == entity.Id);
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
            var exists = await appDbContext.SanctionTypes.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
