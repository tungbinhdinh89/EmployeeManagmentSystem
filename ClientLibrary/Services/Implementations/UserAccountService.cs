
using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ClientLibrary.Services.Contracts;

namespace ClientLibrary.Services.Implementations
{
    internal class UserAccountService : IUserAccountService
    {
        Task<GeneralResponse> IUserAccountService.CreateAsync(Register user)
        {
            throw new NotImplementedException();
        }

        Task<LoginResponse> IUserAccountService.SignInAsync(Login user)
        {
            throw new NotImplementedException();
        }

        Task<LoginResponse> IUserAccountService.RefreshTokenAsync(RefreshToken token)
        {
            throw new NotImplementedException();
        }

        Task<WeatherForecast[]> IUserAccountService.GetWeatherForecasts()
        {
            throw new NotImplementedException();
        }
    }
}
