using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class OvertimeTypeRepository(AppDbContext appDbContext) : IGenericRepository<OvertimeType>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.OvertimeTypes.FirstOrDefaultAsync(o => o.Id == id);
            if (item is null) return NotFound();
            appDbContext.OvertimeTypes.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<List<OvertimeType>> GetAll() => await appDbContext.OvertimeTypes.AsNoTracking().ToListAsync();

        public async Task<OvertimeType?> GetById(int id) => await appDbContext.OvertimeTypes.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<GeneralResponse> Insert(OvertimeType entity)
        {
            if(!await CheckName(entity.Name))
                return new GeneralResponse(false, "Overtime type with this name already exists");

            appDbContext.OvertimeTypes.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(OvertimeType entity)
        {
            var item = await appDbContext.OvertimeTypes.FirstOrDefaultAsync(o => o.Id == entity.Id);
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
            var exists = await appDbContext.OvertimeTypes.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
