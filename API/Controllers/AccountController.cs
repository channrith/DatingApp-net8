using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context,
  ITokenService tokenService, IMapper mapper) : BaseApiController
{
  [HttpPost("register")] // /api/account/register
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    // Console.WriteLine($"RegisterDto.Username: {registerDto.Username}");
    if (await UserExists(registerDto.Username))
      throw new Exception("Username already exists");

    using var hmac = new HMACSHA512();

    var user = mapper.Map<AppUser>(registerDto);
    user.UserName = registerDto.Username.ToLower();
    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
    user.PasswordSalt = hmac.Key;

    context.Users.Add(user);
    await context.SaveChangesAsync();

    return new UserDto
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user),
      KnownAs = user.KnownAs
    };
  }

  [HttpPost("login")] // /api/account/login
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    //  IUserRepository userRepository // inject the user repository
    // var user = await userRepository.GetUserByUsernameAsync(loginDto.username.ToLower());
    var user = await context.Users
      .Include(p => p.Photos)
      .FirstOrDefaultAsync(x => x.UserName == loginDto.username.ToLower());

    if (user == null) throw new Exception("Username not found");

    using var hmac = new HMACSHA512(user.PasswordSalt);
    var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password)); // convert a string to byte[]

    for (int i = 0; i < passwordHash.Length; i++)
    {
      if (passwordHash[i] != user.PasswordHash[i]) throw new Exception("Password is invalid");
    }

    return new UserDto
    {
      Username = user.UserName,
      KnownAs = user.KnownAs,
      Token = tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
    };
  }

  private async Task<bool> UserExists(string username)
  {
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
