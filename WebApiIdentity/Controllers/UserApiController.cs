using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiIdentity.Models;
using WebApiIdentity.Tokens;

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserApiController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "Pong!";
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            User user = new User { UserName = model.Login };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(model.Login);
            }

            return BadRequest(result.Errors);
        }

        // POST api/UserApi/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByNameAsync(model.Login);
            if (user == null)
            {
                return Unauthorized("Uncorrect login");
            }
            var result =
                await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Uncorrect password");
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            return Ok(
                new UserTokenModel { UserName = model.Login, Token = _jwtGenerator.GenerateToken(authClaims) });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await _userManager.FindByIdAsync(id));
        }


        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            User user = new User { UserName = model.Login };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(model.Login);
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody]object newLogin)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                JObject jo = JObject.Parse(newLogin.ToString());
                string newName = jo["newLogin"].ToString();
                user.UserName = newName;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest();
        }
    }
}
