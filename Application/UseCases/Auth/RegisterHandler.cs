using Application.Interfaces;
using Domain.Entities;
using BCrypt.Net;

namespace Application.UseCases.Auth;

public class RegisterHandler
{
    private readonly IUserRepository _userRepository;
    

    public RegisterHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser != null)
            return false;

        // Hash de la contraseña usando BCrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            Password = hashedPassword
        };

        await _userRepository.AddAsync(user);

        return true;
    }
}