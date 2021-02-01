using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                throw new ArgumentNullException(nameof(dto.Username));
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException(nameof(dto.Password));
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = dto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO(){
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrEmpty(dto.Password)) {
                return BadRequest("A username and password must be supplied");
            }

            // Check the user exists
            var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == dto.Username);
            if (user == null)
            {
                
                return Unauthorized("User name not found");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            if (!BytesEqual(computedHash, user.PasswordHash))
            {

                return Unauthorized("Invalid password");
            }

            return new UserDTO(){
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private static bool BytesEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++){
                if (a1[i] != a2[i]){
                    return false;
                }
            }

            return true;
        }
    }
}