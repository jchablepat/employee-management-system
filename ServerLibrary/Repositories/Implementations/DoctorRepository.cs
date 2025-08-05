using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DoctorRepository(AppDbContext appDbContext) : IGenericRepository<Doctor>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var item = await appDbContext.Doctors.FirstOrDefaultAsync(d => d.EmployeeId == id);
            if(item is null) return NotFound();

            appDbContext.Doctors.Remove(item);
            await Commit();
            return Success();
        }

        public async Task<List<Doctor>> GetAll() => await appDbContext.Doctors.AsNoTracking().ToListAsync();

        public async Task<Doctor?> GetById(int id) => await appDbContext.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.EmployeeId == id);

        public async Task<GeneralResponse> Insert(Doctor entity)
        {
            appDbContext.Doctors.Add(entity);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Doctor entity)
        {
            var item = await appDbContext.Doctors.FirstOrDefaultAsync(d => d.EmployeeId == entity.EmployeeId);
            if (item is null) return NotFound();
            item.Date = entity.Date;
            item.MedicalDiagnose = entity.MedicalDiagnose;
            item.MedicalRecommendation = entity.MedicalRecommendation;
            appDbContext.Doctors.Update(item);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry data not found");
        private static GeneralResponse Success() => new(true, "Operation completed successfully");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
