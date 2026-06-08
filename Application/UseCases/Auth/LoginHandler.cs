using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Auth;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;

    public LoginHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(LoginRequest request)
    {
        // 1. Buscamos el usuario en la base de datos
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            Console.WriteLine($"---> [LOGIN DEBUG] Usuario NO encontrado: {request.Email}");
            return null;
        }

        // 2. Verificamos la contraseña con BCrypt
        // Importante: user.Password debe ser el Hash que empieza con $2a$
        bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        // LOGS DE CONTROL (Miralos en la terminal de Linux)
        Console.WriteLine($"---> [LOGIN DEBUG] Email: {user.Email}");
        Console.WriteLine($"---> [LOGIN DEBUG] Rol en DB: {user.Role}");
        Console.WriteLine($"---> [LOGIN DEBUG] Hash en DB: {user.Password}");
        Console.WriteLine($"---> [LOGIN DEBUG] ¿Password '{request.Password}' coincide?: {isValid}");

        if (!isValid)
        {
            return null;
        }

        // 3. Si todo está bien, devolvemos el usuario completo (incluyendo el Role)
        return user;
    }
}