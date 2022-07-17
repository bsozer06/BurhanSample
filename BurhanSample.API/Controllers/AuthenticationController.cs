using AutoMapper;
using BurhanSample.Business.Filters;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.Core.Utilities.Models.Users;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger; 
        private readonly IMapper _mapper; 
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IAuthenticationManager authManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors) 
                { 
                    ModelState.TryAddModelError(error.Code, error.Description);
                } 

                return BadRequest(ModelState); 
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuth)
        {
            if (!await _authManager.ValidateUser(userForAuth))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password."); 
                return Unauthorized();
            }

            return Ok(new { Token = await _authManager.CreateToken() });
        }



    }
}
