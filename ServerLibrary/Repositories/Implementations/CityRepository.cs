using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class CityRepository(ApplicationDbContext dbContext) : IGenericRepositoryInterface<City>
    {
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var city = await dbContext.Cities.FindAsync(id);
            if (city is null)
            {
                return NotFound();
            }

            dbContext.Cities.Remove(city);
            await Commit();
            return Success();
        }

        public async Task<List<City>> GetAll() => await dbContext
            .Cities
            .AsNoTracking()
            .Include(c => c.Country)
            .ToListAsync();

        public async Task<City> GetById(int id)
        {
            var city = await dbContext.Cities.FindAsync(id);
            if (city is null) return new City();
            return city;
        }

        public async Task<GeneralResponse> Insert(City item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Branch is already add");
            dbContext.Cities.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City item)
        {
            var city = await dbContext.Cities.FindAsync(item.Id);
            if (city is null) return NotFound();
            city.Name = item.Name;
            city.CountryId = item.CountryId;
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new(false, "Sorry, Branch is not found");
        private static GeneralResponse Success() => new(true, "Process completed");
        private async Task Commit() => await dbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await dbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Trim().Equals(name.ToLower().Trim()));
            return item is null;
        }
    }
}