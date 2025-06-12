using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DepartmentRepository(AppDbContext appDbContext) : IGenericRepository<Department>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dept = await appDbContext.Departments.FindAsync(id);
            if(dept is null) return NotFound();

            appDbContext.Departments.Remove(dept);
            await Commit();

            return Success();
        }

        public async Task<List<Department>> GetAll() => await appDbContext.Departments.AsNoTracking().Include(gd => gd.GeneralDepartment).ToListAsync();

        public async Task<Department?> GetById(int id) => await appDbContext.Departments.FindAsync(id);

        public async Task<GeneralResponse> Insert(Department entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "Department already exists");

            await appDbContext.Departments.AddAsync(entity);
            await Commit();

            return Success();
        }

        public async Task<GeneralResponse> Update(Department entity)
        {
            var dept = await appDbContext.Departments.FindAsync(entity.Id);

            if (dept is null) return NotFound();
            dept.Name = entity.Name;
            dept.GeneralDepartmentId = entity.GeneralDepartmentId;

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
