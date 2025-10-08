using System;
using Api.DTOs.Auth;

namespace Api.Services.Interfaces;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterDto model);
    Task<DataUserDto> GetTokenAsync(LoginDto model, CancellationToken ct = default);

    Task<string> AddRoleAsync(AddRoleDto model);

    Task<DataUserDto> RefreshTokenAsync(string refreshToken);
}
