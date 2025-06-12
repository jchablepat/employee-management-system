using BaseLibrary.Entities;

namespace ServerLibrary.Data.Seeders
{
    public class AdminDataSeeder(AppDbContext dbContext)
    {
        public void Seed()
        {
            SeedRoles();
        }

        /// <summary>
        /// Seeds the initial roles into the database.
        /// </summary>
        private void SeedRoles()
        {
            if(!dbContext.SystemRoles.Any())
            {
                dbContext.SystemRoles.AddRange(
                    new SystemRole { Name = "Admin" },
                    new SystemRole { Name = "User" }
                );

                dbContext.SaveChanges();
            }
        }
    }
}
