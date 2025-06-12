using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class BranchRepository(AppDbContext appDbContext) : IGenericRepository<Branch>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await appDbContext.Branches.FindAsync(id);
            if (branch is null) return NotFound();

            appDbContext.Branches.Remove(branch);
            await Commit();
            return Success();
        }

        public async Task<List<Branch>> GetAll() => await appDbContext.Branches.AsNoTracking().Include(d => d.Department).ToListAsync();

        public async Task<Branch?> GetById(int id) => await appDbContext.Branches.FindAsync(id);

        public async Task<GeneralResponse> Insert(Branch entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "Branch already exists");

            await appDbContext.Branches.AddAsync(entity);
            await Commit();

            return Success();
        }

        public async Task<GeneralResponse> Update(Branch entity)
        {
            var branch = await appDbContext.Branches.FindAsync(entity.Id);

            if (branch is null) return NotFound();
            branch.Name = entity.Name;
            branch.DepartmentId = entity.DepartmentId;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
