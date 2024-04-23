using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class GeneralDepartmentRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<GeneralDepartment>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var depart = await dbContext.GeneralDepartments.FindAsync(id);

            if (depart is null)
            {
                return NotFound();
            }

            dbContext.GeneralDepartments.Remove(depart);
            await Commit();
            return Success();
        }

        public async Task<List<GeneralDepartment>> GetAll()
        {
            return await dbContext.GeneralDepartments.ToListAsync();
        }

        public async Task<GeneralDepartment> GetById(int id)
        {
            var depart = await dbContext.GeneralDepartments.FindAsync(id);
            if (depart is null)
            {
                return new GeneralDepartment();
            }
            return depart;
        }

        public async Task<GeneralResponse> Insert(GeneralDepartment item)
        {
            var checkIfNull = await CheckName(item.Name);
            if (!checkIfNull)
            {
                return new GeneralResponse(false, "General Department already added!");
            }

            dbContext.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment item)
        {
            var depart = await dbContext.GeneralDepartments.FindAsync(item.Id);
            if (depart is null)
            {
                return NotFound();
            }

            depart.Name = item.Name;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, general department not found");

        private static GeneralResponse Success() => new(true, "Process completed");

        private async Task Commit() => await dbContext.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}
