using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    internal class CountryRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<Country>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var country = await dbContext.Countries.FindAsync(id);
            if (country is null)
            {
                return NotFound();
            }

            dbContext.Countries.Remove(country);
            await Commit();
            return Success();
        }

        public async Task<List<Country>> GetAll() => await dbContext.Countries.ToListAsync();

        public async Task<Country> GetById(int id)
        {
            var country = await dbContext.Countries.FindAsync(id);
            if (country is null) return new Country();
            return country;
        }

        public async Task<GeneralResponse> Insert(Country item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Branch is already add");
            dbContext.Countries.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country item)
        {
            var country = await dbContext.Countries.FindAsync(item.Id);
            if (country is null) return NotFound();
            item.Name = country.Name;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, Branch is not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.Countries.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}

