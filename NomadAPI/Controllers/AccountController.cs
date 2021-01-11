using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email))
            {
                return BadRequest("Email is taken");
            }
            registerDto.FullName = registerDto.FirstName + " " + registerDto.LastName;

            var user = _mapper.Map<AppUser>(registerDto);

            user.Email = user.Email.ToLower();
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Nomad");

            if (!roleResult.Succeeded)
                return BadRequest(result.Errors);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                FirstName = user.FirstName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null)
                return Unauthorized("Invalid email");


            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized();

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                FirstName = user.FirstName,
                Id = user.Id
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

    }


}
