using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class TownRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<Town>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var town = await dbContext.Towns.FindAsync(id);
            if (town is null)
            {
                return NotFound();
            }

            dbContext.Towns.Remove(town);
            await Commit();
            return Success();
        }

        public async Task<List<Town>> GetAll() => await dbContext.Towns.ToListAsync();

        public async Task<Town> GetById(int id)
        {
            var town = await dbContext.Towns.FindAsync(id);
            if (town is null) return new Town();
            return town;
        }

        public async Task<GeneralResponse> Insert(Town item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Branch is already add");
            dbContext.Towns.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var town = await dbContext.Towns.FindAsync(item.Id);
            if (town is null) return NotFound();
            item.Name = town.Name;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, Branch is not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.Towns.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}

