using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class EmployeeRepository(AppDbContext appDbContext) : IGenericRepository<Employee>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.Employees.FindAsync(id);
            if (item is null) return NotFound();

            appDbContext.Employees.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<List<Employee>> GetAll()
        {
            var employees = await appDbContext.Employees
                .AsNoTracking()
                .Include(e => e.Town!)
                .ThenInclude(c => c.City!)
                .ThenInclude(c => c.Country!)
                .Include(e => e.Branch!)
                .ThenInclude(d => d.Department!)
                .ThenInclude(d => d.GeneralDepartment!)
                .ToListAsync();

            return employees;
        }

        public async Task<Employee?> GetById(int id)
        {
            var employee = await appDbContext.Employees
                .Include(e => e.Town!)
                .ThenInclude(c => c.City!)
                .ThenInclude(c => c.Country)
                .Include(e => e.Branch!)
                .ThenInclude(d => d.Department!)
                .ThenInclude(d => d.GeneralDepartment!)
                .FirstOrDefaultAsync(e => e.Id == id);

            return employee!;
        }

        public async Task<GeneralResponse> Insert(Employee entity)
        {
            if (!await CheckName(entity.Name)) return new GeneralResponse(false, "Employee already exists");
            
            await appDbContext.Employees.AddAsync(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Employee entity)
        {
            var employee = await appDbContext.Employees.FindAsync(entity.Id);
            if (employee is null) return NotFound();

            employee.Name = entity.Name;
            employee.Other = entity.Other;
            employee.Address = entity.Address;
            employee.TelephoneNumber = entity.TelephoneNumber;
            employee.TownId = entity.TownId;
            employee.BranchId = entity.BranchId;
            employee.CivilId = entity.CivilId;
            employee.FileNumber = entity.FileNumber;
            employee.JobName = entity.JobName;
            employee.Photo = entity.Photo;

            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry employee not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var exists = await appDbContext.Employees.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));

            return exists is null;
        }
    }
}
