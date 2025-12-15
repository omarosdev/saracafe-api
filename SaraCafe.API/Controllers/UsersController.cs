using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaraCafe.API.DTOs;
using SaraCafe.API.Entities;
using SaraCafe.API.Services;

namespace SaraCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UsersController(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    // POST: api/users/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        
        if (result == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(result);
    }

    // GET: api/users
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            CreatedAt = u.CreatedAt
        });

        return Ok(userDtos);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    // POST: api/users
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        // Check if username already exists
        var existingUser = await _userRepository.GetUserByUsernameAsync(createUserDto.Username);
        if (existingUser != null)
        {
            return Conflict("Username already exists");
        }

        // Check if email already exists
        var existingEmail = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
        if (existingEmail != null)
        {
            return Conflict("Email already exists");
        }

        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = _authService.HashPassword(createUserDto.Password),
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName
        };

        var createdUser = await _userRepository.CreateUserAsync(user);
        await _userRepository.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            Email = createdUser.Email,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            CreatedAt = createdUser.CreatedAt
        };

        return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Check if email is being changed and already exists
        if (updateUserDto.Email != user.Email)
        {
            var existingEmail = await _userRepository.GetUserByEmailAsync(updateUserDto.Email);
            if (existingEmail != null)
            {
                return Conflict("Email already exists");
            }
        }

        user.Email = updateUserDto.Email;
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            user.PasswordHash = _authService.HashPassword(updateUserDto.Password);
        }

        var updatedUser = await _userRepository.UpdateUserAsync(user);
        if (updatedUser == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Email = updatedUser.Email,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName,
            CreatedAt = updatedUser.CreatedAt
        };

        return Ok(userDto);
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _userRepository.DeleteUserAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

