using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class GeneralDepartmentRepository(AppDbContext appDbContext) : IGenericRepository<GeneralDepartment>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dept = await appDbContext.GeneralDepartments.FindAsync(id);
            if (dept is null) return NotFound();

            appDbContext.GeneralDepartments.Remove(dept);
            await Commit();
            return Success();
        }

        public async Task<List<GeneralDepartment>> GetAll() => await appDbContext.GeneralDepartments.ToListAsync();

        public async Task<GeneralDepartment?> GetById(int id) => await appDbContext.GeneralDepartments.FindAsync(id);

        public async Task<GeneralResponse> Insert(GeneralDepartment entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "General Department already exists");

            await appDbContext.GeneralDepartments.AddAsync(entity);
            await Commit();
           
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment entity)
        {
            var dept = await appDbContext.GeneralDepartments.FindAsync(entity.Id);

            if (dept is null) return NotFound();
            dept.Name = entity.Name;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
