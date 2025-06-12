using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;
using System;

namespace ServerLibrary.Repositories.Implementations
{
    public class CountryRepository(AppDbContext appDbContext) : IGenericRepository<Country>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var country = await appDbContext.Countries.FindAsync(id);
            if (country is null) return NotFound();

            appDbContext.Countries.Remove(country);
            await Commit();
            return Success();
        }

        public async Task<List<Country>> GetAll() => await appDbContext.Countries.ToListAsync();

        public async Task<Country?> GetById(int id) => await appDbContext.Countries.FindAsync(id);

        public async Task<GeneralResponse> Insert(Country entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "Country already exists");

            await appDbContext.Countries.AddAsync(entity);
            await Commit();

            return Success();
        }

        public async Task<GeneralResponse> Update(Country entity)
        {
            var country = await appDbContext.Countries.FindAsync(entity.Id);

            if (country is null) return NotFound();
            country.Name = entity.Name;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry country not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.Countries.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
