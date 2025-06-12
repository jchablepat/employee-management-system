using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Implementations
{
    public class CityRepository(AppDbContext appDbContext) : IGenericRepository<City>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var city = await appDbContext.Cities.FindAsync(id);
            if (city is null) return NotFound();

            appDbContext.Cities.Remove(city);
            await Commit();
            return Success();
        }

        public async Task<List<City>> GetAll() => await appDbContext.Cities.AsNoTracking().Include(c => c.Country).ToListAsync();

        public async Task<City?> GetById(int id) => await appDbContext.Cities.FindAsync(id);

        public async Task<GeneralResponse> Insert(City entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "City already exists");

            await appDbContext.Cities.AddAsync(entity);
            await Commit();

            return Success();
        }

        public async Task<GeneralResponse> Update(City entity)
        {
            var city = await appDbContext.Cities.FindAsync(entity.Id);

            if (city is null) return NotFound();
            city.Name = entity.Name;
            city.CountryId = entity.CountryId;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry city not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
