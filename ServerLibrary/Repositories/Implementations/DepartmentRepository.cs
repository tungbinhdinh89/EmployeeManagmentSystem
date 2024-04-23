using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DepartmentRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<Department>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var depart = await dbContext.Departments.FindAsync(id);
            if (depart is null)
            {
                return NotFound();
            }

            dbContext.Departments.Remove(depart);
            await Commit();
            return Success();
        }

        public async Task<List<Department>> GetAll() => await dbContext
            .Departments.AsNoTracking()
            .Include(gd => gd.GeneralDepartment)
            .ToListAsync();

        public async Task<Department> GetById(int id)
        {
            var depart = await dbContext.Departments.FindAsync(id);
            if (depart is null) return new Department();
            return depart;

        }

        public async Task<GeneralResponse> Insert(Department item)
        {
            if (!await CheckName(item.Name!))
            {
                return new GeneralResponse(false, "Department already added!");
            }
            dbContext.Departments.Add(item!);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var depart = await dbContext.Departments.FindAsync(item.Id);
            if (depart is null) return NotFound();
            depart.Name = item.Name;
            depart.GeneralDepartmentId = item.GeneralDepartmentId;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, Department not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}
