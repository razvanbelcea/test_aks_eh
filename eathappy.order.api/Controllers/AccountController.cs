using AutoMapper;
using eathappy.order.api.JwtFeatures;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Models;
using eathappy.order.domain.Roles;
using eathappy.order.domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eathappy.order.api.Controllers
{
    /// <summary>
    /// Controller for account management
    /// </summary>
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;

        /// <summary>
        /// Constructor of account controller
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="mapper"></param>
        /// <param name="jwtHandler"></param>
        public AccountController(
            UserManager<User> userManager, 
            IMapper mapper, 
            JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="userRegistrationDto"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
        {
            if (userRegistrationDto == null || !ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<User>(userRegistrationDto);

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, RoleConstants.Viewer);

            return StatusCode(201);
        }

        /// <summary>
        /// Generates login token
        /// </summary>
        /// <param name="userAuthenticationDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDto userAuthenticationDto)
        {
            var user = await _userManager.FindByNameAsync(userAuthenticationDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userAuthenticationDto.Password))
                return Unauthorized(new AuthenticationResponseDto { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            List<Claim> claims = await GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new AuthenticationResponseDto { IsAuthSuccessful = true, Token = token, User = userDto });
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = _jwtHandler.GetClaims(user);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
