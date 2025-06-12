using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class TownRepository(AppDbContext appDbContext) : IGenericRepository<Town>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var town = await appDbContext.Towns.FindAsync(id);
            if (town is null) return NotFound();

            appDbContext.Towns.Remove(town);
            await Commit();
            return Success();
        }

        public async Task<List<Town>> GetAll() => await appDbContext.Towns.AsNoTracking().Include(t => t.City).ToListAsync();

        public async Task<Town?> GetById(int id) => await appDbContext.Towns.FindAsync(id);

        public async Task<GeneralResponse> Insert(Town entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "Town already exists");

            await appDbContext.Towns.AddAsync(entity);
            await Commit();

            return Success();
        }

        public async Task<GeneralResponse> Update(Town entity)
        {
            var town = await appDbContext.Towns.FindAsync(entity.Id);

            if (town is null) return NotFound();
            town.Name = entity.Name;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry town not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.Towns.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
