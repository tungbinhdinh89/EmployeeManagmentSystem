using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class BranchRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<Branch>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var branch = await dbContext.Branches.FindAsync(id);
            if (branch is null)
            {
                return NotFound();
            }

            dbContext.Branches.Remove(branch);
            await Commit();
            return Success();
        }

        public async Task<List<Branch>> GetAll() => await dbContext.Branches
            .AsNoTracking()
            .Include(d => d.Department)
            .ToListAsync();

        public async Task<Branch> GetById(int id)
        {
            var branch = await dbContext.Branches.FindAsync(id);
            if (branch is null) return new Branch();
            return branch;
        }

        public async Task<GeneralResponse> Insert(Branch item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Branch is already add");
            dbContext.Branches.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch item)
        {
            var branch = await dbContext.Branches.FindAsync(item.Id);
            if (branch is null) return NotFound();
            branch.Name = item.Name;
            branch.DepartmentId = item.DepartmentId;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, Branch is not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.Branches.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}
